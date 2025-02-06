using Microsoft.AspNetCore.Mvc;
using SalesPointServices.Models;
using SalesPointServices.Models.DTOs;
using SalesPointServices.Services.Core;

namespace SalesPointServices.Controllers
{
    /// <summary>
    /// Контроллер для управления точками продаж.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SalesPointController : ControllerBase
    {
        private readonly ISalesPointService _salesPointService;

        public SalesPointController(ISalesPointService salesPointService)
        {
            _salesPointService = salesPointService;
        }

        /// <summary>
        /// Получает список всех точек продаж.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesPoint>>> GetAll()
        {
            var result = await _salesPointService.GetAllSalesPointsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Получает точку продаж по ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesPoint>> GetById(int id)
        {
            var result = await _salesPointService.GetSalesPointByIdAsync(id);
            return result == null ? NotFound($"Точка продажи с ID {id} не найдена") : Ok(result);
        }

        /// <summary>
        /// Обновляет точку продаж.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SalesPoint salesPoint)
        {
            var success = await _salesPointService.UpdateSalesPointAsync(id, salesPoint);
            return success ? NoContent() : NotFound($"Точка продажи с ID {id} не найдена");
        }

        /// <summary>
        /// Удаляет точку продаж.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _salesPointService.DeleteSalesPointAsync(id);
            return success ? NoContent() : NotFound($"Точка продажи с ID {id} не найдена");
        }

        /// <summary>
        /// Создает новую точку продаж.
        /// </summary>
        /// <param name="salesPoint">Объект точки продаж.</param>
        [HttpPost]
        public async Task<ActionResult> Create(SalesPoint salesPoint)
        {
            var result = await _salesPointService.CreateSalesPointAsync(salesPoint);
            if (result == null) return BadRequest("Ошибка создания точки продаж");

            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обрабатывает продажу товаров в указанной точке продаж.
        /// </summary>
        /// <param name="id">Идентификатор точки продаж.</param>
        /// <param name="requests">Список товаров для продажи.</param>
        [HttpPost("{id}/sell")]
        public async Task<IActionResult> SellProducts(int id, [FromBody] List<SellRequest> requests)
        {
            try
            {
                var sale = await _salesPointService.ProcessSaleAsync(id, requests);
                if (sale == null) return NotFound("Точка продажи не найдена");

                return Ok(sale);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
}
