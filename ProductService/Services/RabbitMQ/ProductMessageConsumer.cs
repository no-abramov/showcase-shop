using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using ProductServices.Data;
using System.Text;
using System.Text.Json;
using ProductServices.Services.RabbitMQ.Events;

namespace ProductServices.Services.RabbitMQ
{
    /// <summary>
    /// Фоновая служба для обработки запросов RabbitMQ о цене товаров.
    /// </summary>
    public class ProductMessageConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public ProductMessageConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

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

            // Объявляем очереди для получения запроса цены и отправки ответа
            _channel.QueueDeclare(queue: "product_price_request", durable: false, exclusive: false, autoDelete: false);
            _channel.QueueDeclare(queue: "product_price_response", durable: false, exclusive: false, autoDelete: false);
        }

        /// <summary>
        /// Основной метод обработки сообщений RabbitMQ.
        /// </summary>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

                    // Получаем и десериализуем запрос
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var request = JsonSerializer.Deserialize<ProductInfoRequest>(message);

                    // Ищем товар в базе данных
                    var product = await dbContext.Products.FindAsync(request?.ProductId);

                    // Формируем ответ
                    var response = new ProductInfoResponse
                    {
                        ProductId = request.ProductId,
                        Price = product?.Price ?? 0,
                        IsAvailable = product != null
                    };

                    // Сериализуем и отправляем ответное сообщение
                    var responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
                    var responseProps = _channel.CreateBasicProperties();
                    responseProps.CorrelationId = ea.BasicProperties.CorrelationId;

                    _channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: responseProps, body: responseBytes);
                }
            };

            // Подписываемся на очередь запросов цены
            _channel.BasicConsume(queue: "product_price_request", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}