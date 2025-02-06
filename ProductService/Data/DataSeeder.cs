using ProductServices.Models;

namespace ProductServices.Data
{
    /// <summary>
    /// Заполняет базу данных начальными тестовыми данными.
    /// </summary>
    public static class DataSeeder
    {
        /// <summary>
        /// Добавляет начальные данные в базу данных, если она пуста.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public static void SeedDatabase(ProductDbContext context)
        {
            if (!context.Products.Any())
            {
                // Инициализация товаров
                var products = new List<ProductItem>
                {
                    new ProductItem { Id = 1, Name = "Товар 1", Price = 100 },
                    new ProductItem { Id = 2, Name = "Товар 2", Price = 200 },
                    new ProductItem { Id = 3, Name = "Товар 3", Price = 300 },
                };

                context.Products.AddRange(products);
            }

            context.SaveChanges();
        }
    }
}
