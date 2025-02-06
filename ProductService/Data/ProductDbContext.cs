using Microsoft.EntityFrameworkCore;
using ProductServices.Models;

namespace ProductServices.Data
{
    /// <summary>
    /// Контекст базы данных для управления товарами.
    /// </summary>
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        /// <summary>
        /// Таблица товаров.
        /// </summary>
        public DbSet<ProductItem> Products { get; set; }
    }
}
