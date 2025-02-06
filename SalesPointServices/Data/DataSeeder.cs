﻿using SalesPointServices.Models;

namespace SalesPointServices.Data
{
    /// <summary>
    /// Заполняет базу данных начальными тестовыми данными.
    /// </summary>
    public static class DataSeeder
    {
        /// <summary>
        /// Добавляет начальные данные в базу данных, если она пуста.
        /// </summary>
        public static void SeedDatabase(SalesPointDbContext context)
        {
            if (!context.SalesPoints.Any())
            {
                var salesPoint1 = new SalesPoint
                {
                    Name = "Магазин №1",
                    ProvidedProducts = new List<ProvidedProduct>
                    {
                        new ProvidedProduct { ProductId = 1, Quantity = 50, SalesPointId = 1 },
                        new ProvidedProduct { ProductId = 2, Quantity = 30, SalesPointId = 1 }
                    }
                };

                var salesPoint2 = new SalesPoint
                {
                    Name = "Магазин №2",
                    ProvidedProducts = new List<ProvidedProduct>
                    {
                        new ProvidedProduct { ProductId = 1, Quantity = 20, SalesPointId = 2 },
                        new ProvidedProduct { ProductId = 2, Quantity = 10, SalesPointId = 2 },
                        new ProvidedProduct { ProductId = 3, Quantity = 100, SalesPointId = 2 }
                    }
                };

                context.SalesPoints.Add(salesPoint1);
                context.SalesPoints.Add(salesPoint2);
                context.SaveChanges();
            }
        }
    }
}