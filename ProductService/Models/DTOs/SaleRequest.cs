namespace ProductServices.Models.DTOs
{
    /// <summary>
    /// DTO-запрос на продажу товара.
    /// </summary>
    public class SaleRequest
    {
        /// <summary>
        /// ID товара, который необходимо продать.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество товара, которое нужно продать.
        /// </summary>
        public int Quantity { get; set; }
    }
}