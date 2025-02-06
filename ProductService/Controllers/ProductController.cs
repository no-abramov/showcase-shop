using Microsoft.AspNetCore.Mvc;
using ProductServices.Models;
using ProductServices.Services.Interfaces;

namespace ProductServices.Controllers
{
    /// <summary>
    /// Контроллер для управления товарами.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Получает список всех товаров.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductItem>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Получает информацию о товаре по ID.
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductItem>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product == null ? NotFound($"Товар с ID {id} не найден") : Ok(product);
        }

        /// <summary>
        /// Создает новый товар.
        /// </summary>
        /// <param name="product">Объект товара</param>
        [HttpPost]
        public async Task<ActionResult<ProductItem>> CreateProduct(ProductItem product)
        {
            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct?.Id }, createdProduct);
        }

        /// <summary>
        /// Обновляет существующий товар.
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <param name="product">Обновленные данные товара</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductItem product)
        {
            var success = await _productService.UpdateProductAsync(id, product);
            return success ? NoContent() : BadRequest("Ошибка обновления товара");
        }

        /// <summary>
        /// Удаляет товар по ID.
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            return success ? NoContent() : NotFound($"Товар с ID {id} не найден");
        }

        /// <summary>
        /// Получает цену товара по его ID.
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        [HttpGet("{id}/price")]
        public async Task<IActionResult> GetProductPrice(int id)
        {
            var price = await _productService.GetProductPriceAsync(id);
            return price == null ? NotFound($"Товар с ID {id} не найден") : Ok(new { id, price });
        }
    }
}