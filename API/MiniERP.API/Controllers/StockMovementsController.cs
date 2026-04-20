using Microsoft.AspNetCore.Mvc;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

// -- Controller pro auditní pohyby skladu --
[ApiController]
[Route("api/[controller]")]
public class StockMovementsController : ControllerBase
{
    private readonly IStockMovementService _stockMovementService;

    public StockMovementsController(IStockMovementService stockMovementService)
    {
        _stockMovementService = stockMovementService;
    }

    // -- Endpoint pro načtení seznamu pohybů skladu --
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movements = await _stockMovementService.GetAllAsync();
        return Ok(movements);
    }

    // -- Endpoint pro načtení detailu pohybu skladu podle ID --
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var movement = await _stockMovementService.GetByIdAsync(id);

        if (movement == null)
        {
            return NotFound();
        }

        return Ok(movement);
    }
}