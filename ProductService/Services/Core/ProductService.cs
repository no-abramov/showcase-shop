using Microsoft.EntityFrameworkCore;
using ProductServices.Data;
using ProductServices.Models;
using ProductServices.Services.Interfaces;

namespace ProductServices.Services.Core
{
    /// <summary>
    /// Реализация сервиса управления товарами.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ProductItem>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<ProductItem?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<ProductItem?> CreateProductAsync(ProductItem product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateProductAsync(int id, ProductItem product)
        {
            if (id != product.Id)
                return false;

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<decimal?> GetProductPriceAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product?.Price;
        }
    }
}
