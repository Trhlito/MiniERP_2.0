using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Orders;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

// Controller pro objednávky
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    // Service vrstva pro objednávky
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // Načtení seznamu objednávek
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    // Načtení detailu objednávky podle ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);

        if (order == null)
        {
            return NotFound(new
            {
                Message = $"Objednávka s ID {id} nebyla nalezena."
            });
        }

        return Ok(order);
    }

    // Vytvoření nové objednávky
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        // Vrácení validačních chyb
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            // Vytvoření objednávky
            var newOrderId = await _orderService.CreateAsync(request);

            // Vrácení Created odpovědi s odkazem na detail
            return CreatedAtAction(
                nameof(GetById),
                new { id = newOrderId },
                new
                {
                    Id = newOrderId,
                    Message = "Objednávka byla úspěšně vytvořena."
                });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }

    // Úprava objednávky
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderRequest request)
    {
        // Vrácení validačních chyb
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var updated = await _orderService.UpdateAsync(id, request);

            if (!updated)
            {
                return NotFound(new
                {
                    Message = $"Objednávka s ID {id} nebyla nalezena."
                });
            }

            return Ok(new
            {
                Message = "Objednávka byla úspěšně upravena."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }

    // Smazání objednávky
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _orderService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    Message = $"Objednávka s ID {id} nebyla nalezena."
                });
            }

            return Ok(new
            {
                Message = "Objednávka byla úspěšně smazána."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }

    // Rezervace skladu pro objednávku
    [HttpPost("{id:int}/reserve-stock")]
    public async Task<IActionResult> ReserveStock(int id)
    {
        var result = await _orderService.ReserveStockAsync(id);

        if (!result.Success)
        {
            if (result.ErrorCode == "ORDER_NOT_FOUND")
            {
                return NotFound(new
                {
                    Message = result.Message,
                    ErrorCode = result.ErrorCode
                });
            }

            return BadRequest(new
            {
                Message = result.Message,
                ErrorCode = result.ErrorCode
            });
        }

        return Ok(new
        {
            Message = result.Message
        });
    }

    // Uvolnění rezervace skladu pro objednávku
    [HttpPost("{id:int}/release-stock")]
    public async Task<IActionResult> ReleaseStock(int id)
    {
        var result = await _orderService.ReleaseStockAsync(id);

        if (!result.Success)
        {
            if (result.ErrorCode == "ORDER_NOT_FOUND")
            {
                return NotFound(new
                {
                    Message = result.Message,
                    ErrorCode = result.ErrorCode
                });
            }

            return BadRequest(new
            {
                Message = result.Message,
                ErrorCode = result.ErrorCode
            });
        }

        return Ok(new
        {
            Message = result.Message
        });
    }
}