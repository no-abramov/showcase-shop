using Microsoft.EntityFrameworkCore;
using SalesPointServices.Models;

namespace SalesPointServices.Data
{
    /// <summary>
    /// Контекст базы данных для управления точками продаж.
    /// </summary>
    public class SalesPointDbContext : DbContext
    {
        public SalesPointDbContext(DbContextOptions<SalesPointDbContext> options) : base(options) { }

        /// <summary>
        /// Таблица точек продаж.
        /// </summary>
        public DbSet<SalesPoint> SalesPoints { get; set; }

        /// <summary>
        /// Таблица товаров, доступных в точках продаж.
        /// </summary>
        public DbSet<ProvidedProduct> ProvidedProducts { get; set; }

        /// <summary>
        /// Таблица актов продаж.
        /// </summary>
        public DbSet<Sale> Sales { get; set; }
    }
}