namespace SalesPointServices.Models
{
    /// <summary>
    /// Представляет точку продаж в системе.
    /// </summary>
    public class SalesPoint
    {
        /// <summary>
        /// Уникальный идентификатор точки продаж.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название точки продаж.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Список товаров, доступных к продаже в этой точке.
        /// </summary>
        public List<ProvidedProduct> ProvidedProducts { get; set; } = new();
    }
}
