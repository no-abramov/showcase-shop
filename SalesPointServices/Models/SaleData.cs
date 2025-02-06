namespace SalesPointServices.Models
{
    /// <summary>
    /// Детали проданных товаров в рамках одной продажи.
    /// </summary>
    public class SaleData
    {
        /// <summary>
        /// Уникальный идентификатор записи.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор проданного товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество проданного товара.
        /// </summary>
        public int ProductQuantity { get; set; }

        /// <summary>
        /// Общая стоимость проданного товара (quantity * price).
        /// </summary>
        public decimal ProductAmount { get; set; }
    }
}
