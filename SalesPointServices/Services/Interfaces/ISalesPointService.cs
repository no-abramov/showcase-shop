using SalesPointServices.Models;
using SalesPointServices.Models.DTOs;

namespace SalesPointServices.Services.Core
{
    /// <summary>
    /// Интерфейс для управления точками продаж.
    /// </summary>
    public interface ISalesPointService
    {
        /// <summary>
        /// Получает список всех точек продаж.
        /// </summary>
        Task<IEnumerable<SalesPoint>> GetAllSalesPointsAsync();

        /// <summary>
        /// Получает точку продаж по ID.
        /// </summary>
        /// <param name="id">Идентификатор точки продаж.</param>
        Task<SalesPoint?> GetSalesPointByIdAsync(int id);

        /// <summary>
        /// Создает новую точку продаж.
        /// </summary>
        /// <param name="salesPoint">Объект точки продаж.</param>
        Task<SalesPoint?> CreateSalesPointAsync(SalesPoint salesPoint);

        /// <summary>
        /// Обновляет существующую точку продаж.
        /// </summary>
        /// <param name="id">Идентификатор точки продаж.</param>
        /// <param name="salesPoint">Обновленные данные.</param>
        Task<bool> UpdateSalesPointAsync(int id, SalesPoint salesPoint);

        /// <summary>
        /// Удаляет точку продаж по ID.
        /// </summary>
        /// <param name="id">Идентификатор точки продаж.</param>
        Task<bool> DeleteSalesPointAsync(int id);

        /// <summary>
        /// Обрабатывает продажу товаров в указанной точке продаж.
        /// </summary>
        /// <param name="salesPointId">Идентификатор точки продаж.</param>
        /// <param name="requests">Список товаров для продажи.</param>
        Task<Sale?> ProcessSaleAsync(int salesPointId, List<SellRequest> requests);
    }
}
