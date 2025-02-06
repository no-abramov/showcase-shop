namespace SalesPointServices.Models.DTOs
{
    /// <summary>
    /// Запрос на продажу товара.
    /// </summary>
    public class SellRequest
    {
        /// <summary>
        /// Идентификатор товара, который необходимо продать.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество товара, которое нужно продать.
        /// </summary>
        public int Quantity { get; set; }
    }
}
