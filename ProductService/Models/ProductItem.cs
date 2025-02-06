namespace ProductServices.Models
{
    /// <summary>
    /// Представляет товар в системе.
    /// </summary>
    public class ProductItem
    {
        /// <summary>
        /// Уникальный идентификатор товара.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название товара.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Цена товара.
        /// </summary>
        public decimal Price { get; set; }
    }
}