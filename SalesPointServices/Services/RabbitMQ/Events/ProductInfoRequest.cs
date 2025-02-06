namespace SalesPointServices.Services.RabbitMQ.Events
{
    /// <summary>
    /// Запрос на получение информации о товаре.
    /// </summary>
    public class ProductInfoRequest
    {
        /// <summary>
        /// Идентификатор запрашиваемого товара.
        /// </summary>
        public int ProductId { get; set; }
    }
}