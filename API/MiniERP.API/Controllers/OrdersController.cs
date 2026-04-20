using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Orders;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

// -- Controller pro objednávky --
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    // -- Service vrstva pro objednávky --
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // -- Endpoint pro načtení seznamu objednávek --
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    // -- Endpoint pro načtení detailu objednávky podle ID --
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

    // -- Endpoint pro vytvoření nové objednávky --
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        // -- Pokud validace neprošla, vrátíme chyby --
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            // -- Vytvoření objednávky --
            var newOrderId = await _orderService.CreateAsync(request);

            // -- Vrácení Created odpovědi s odkazem na detail --
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

    // -- Endpoint pro úpravu objednávky --
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderRequest request)
    {
        // -- Pokud validace neprošla, vrátíme chyby --
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

    // -- Endpoint pro smazání objednávky --
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

    // -- Endpoint pro rezervaci skladu pro objednávku --
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

    // -- Endpoint pro uvolnění rezervace skladu pro objednávku --
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