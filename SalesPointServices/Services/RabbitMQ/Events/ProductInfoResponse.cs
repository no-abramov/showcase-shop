namespace SalesPointServices.Services.RabbitMQ.Events
{
    /// <summary>
    /// Ответ на запрос информации о товаре.
    /// </summary>
    public class ProductInfoResponse
    {
        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Цена товара.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Флаг, указывающий, доступен ли товар.
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}