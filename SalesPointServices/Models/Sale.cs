namespace SalesPointServices.Models
{
    /// <summary>
    /// Представляет акт продажи в системе.
    /// </summary>
    public class Sale
    {
        /// <summary>
        /// Уникальный идентификатор продажи.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата совершения продажи.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Идентификатор точки продаж, где была совершена продажа.
        /// </summary>
        public int SalesPointId { get; set; }

        /// <summary>
        /// Список товаров, проданных в рамках данной продажи.
        /// </summary>
        public List<SaleData> SalesData { get; set; } = new();
    }
}
