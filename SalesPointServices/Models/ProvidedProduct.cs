using System.Text.Json.Serialization;

namespace SalesPointServices.Models
{
    /// <summary>
    /// Представляет товар, доступный в конкретной точке продаж.
    /// </summary>
    public class ProvidedProduct
    {
        /// <summary>
        /// Уникальный идентификатор записи.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Идентификатор точки продаж.
        /// </summary>
        public int SalesPointId { get; set; }

        /// <summary>
        /// Количество товара, доступного для продажи.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Связь с точкой продаж (не сериализуется в JSON).
        /// </summary>
        [JsonIgnore]
        public SalesPoint? SalesPoint { get; set; }
    }
}
