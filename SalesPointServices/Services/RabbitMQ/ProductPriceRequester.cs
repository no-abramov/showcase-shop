using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using SalesPointServices.Services.RabbitMQ.Events;

namespace SalesPointServices.Services.RabbitMQ
{
    /// <summary>
    /// Класс для отправки запроса на получение цены товара через RabbitMQ.
    /// </summary>
    public class ProductPriceRequester
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly AsyncEventingBasicConsumer _consumer;
        private readonly Dictionary<string, TaskCompletionSource<ProductInfoResponse>> _callbackMapper;

        public ProductPriceRequester()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _replyQueueName = _channel.QueueDeclare().QueueName;
            _callbackMapper = new Dictionary<string, TaskCompletionSource<ProductInfoResponse>>();

            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.Received += async (model, ea) =>
            {
                if (_callbackMapper.TryGetValue(ea.BasicProperties.CorrelationId, out var tcs))
                {
                    var responseMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var response = JsonSerializer.Deserialize<ProductInfoResponse>(responseMessage);
                    tcs.TrySetResult(response);
                    _callbackMapper.Remove(ea.BasicProperties.CorrelationId);
                }
                await Task.Yield();
            };

            _channel.BasicConsume(queue: _replyQueueName, autoAck: true, consumer: _consumer);
        }

        /// <summary>
        /// Запрашивает цену товара по его ID через RabbitMQ.
        /// </summary>
        /// <param name="productId">Идентификатор товара.</param>
        /// <returns>Цена товара или null, если запрос не выполнен.</returns>
        public async Task<decimal?> GetProductPriceAsync(int productId)
        {
            var correlationId = Guid.NewGuid().ToString();
            var props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = _replyQueueName;

            var message = JsonSerializer.Serialize(new ProductInfoRequest { ProductId = productId });
            var messageBytes = Encoding.UTF8.GetBytes(message);

            var tcs = new TaskCompletionSource<ProductInfoResponse>();
            _callbackMapper[correlationId] = tcs;

            _channel.BasicPublish(exchange: "", routingKey: "product_price_request", basicProperties: props, body: messageBytes);

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                try
                {
                    var result = await tcs.Task.WaitAsync(cts.Token);
                    return result?.Price;
                }
                catch (OperationCanceledException)
                {
                    _callbackMapper.Remove(correlationId);
                    return null;
                }
            }
        }
    }
}