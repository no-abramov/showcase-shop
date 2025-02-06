using ProductServices.Models;

namespace ProductServices.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для управления товарами в системе.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Получает список всех товаров.
        /// </summary>
        Task<IEnumerable<ProductItem>> GetAllProductsAsync();

        /// <summary>
        /// Получает товар по его ID.
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        Task<ProductItem?> GetProductByIdAsync(int id);

        /// <summary>
        /// Создает новый товар.
        /// </summary>
        /// <param name="product">Объект товара</param>
        Task<ProductItem?> CreateProductAsync(ProductItem product);

        /// <summary>
        /// Обновляет данные существующего товара.
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <param name="product">Объект товара с новыми данными</param>
        Task<bool> UpdateProductAsync(int id, ProductItem product);

        /// <summary>
        /// Удаляет товар по его ID.
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        Task<bool> DeleteProductAsync(int id);

        /// <summary>
        /// Получает цену товара по его ID.
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        Task<decimal?> GetProductPriceAsync(int id);
    }
}
