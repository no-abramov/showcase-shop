using Microsoft.EntityFrameworkCore;
using SalesPointServices.Data;
using SalesPointServices.Models;
using SalesPointServices.Models.DTOs;
using SalesPointServices.Services.RabbitMQ;

namespace SalesPointServices.Services.Core
{
    /// <summary>
    /// Реализация сервиса управления точками продаж.
    /// </summary>
    public class SalesPointService : ISalesPointService
    {
        private readonly SalesPointDbContext _context;
        private readonly ProductPriceRequester _priceRequester;

        public SalesPointService(SalesPointDbContext context, ProductPriceRequester priceRequester)
        {
            _context = context;
            _priceRequester = priceRequester;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<SalesPoint>> GetAllSalesPointsAsync()
        {
            return await _context.SalesPoints.Include(sp => sp.ProvidedProducts).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<SalesPoint?> GetSalesPointByIdAsync(int id)
        {
            return await _context.SalesPoints.Include(sp => sp.ProvidedProducts)
                .FirstOrDefaultAsync(sp => sp.Id == id);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateSalesPointAsync(int id, SalesPoint salesPoint)
        {
            var existing = await _context.SalesPoints.FindAsync(id);
            if (existing == null) return false;

            existing.Name = salesPoint.Name;
            _context.Entry(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteSalesPointAsync(int id)
        {
            var salesPoint = await _context.SalesPoints.FindAsync(id);
            if (salesPoint == null) return false;

            _context.SalesPoints.Remove(salesPoint);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<SalesPoint?> CreateSalesPointAsync(SalesPoint salesPoint)
        {
            if (salesPoint.ProvidedProducts != null)
            {
                foreach (var product in salesPoint.ProvidedProducts)
                {
                    product.SalesPointId = salesPoint.Id;
                }
            }

            _context.SalesPoints.Add(salesPoint);
            await _context.SaveChangesAsync();
            return salesPoint;
        }

        /// <inheritdoc />
        public async Task<Sale?> ProcessSaleAsync(int salesPointId, List<SellRequest> requests)
        {
            var salesPoint = await _context.SalesPoints.Include(sp => sp.ProvidedProducts)
                .FirstOrDefaultAsync(sp => sp.Id == salesPointId);
            if (salesPoint == null) return null;

            var sale = new Sale { SalesPointId = salesPointId, SalesData = new List<SaleData>() };

            foreach (var request in requests)
            {
                var providedProduct = salesPoint.ProvidedProducts.FirstOrDefault(p => p.ProductId == request.ProductId);

                if (providedProduct == null)
                    throw new KeyNotFoundException($"Товар с ID {request.ProductId} не найден в данной точке продажи");

                if (providedProduct.Quantity < request.Quantity)
                    throw new InvalidOperationException($"Недостаточно товара для ID {request.ProductId}");

                var price = await _priceRequester.GetProductPriceAsync(request.ProductId);
                if (price == null)
                    throw new InvalidOperationException("Ошибка получения цены товара через RabbitMQ");

                providedProduct.Quantity -= request.Quantity;

                sale.SalesData.Add(new SaleData
                {
                    ProductId = request.ProductId,
                    ProductQuantity = request.Quantity,
                    ProductAmount = request.Quantity * price.Value
                });
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return sale;
        }
    }
}