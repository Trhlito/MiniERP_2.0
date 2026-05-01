using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Customers;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // Načtení všech zákazníků
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }

    // Načtení zákazníka podle ID
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    // Vytvoření zákazníka
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        // Vrácení validačních chyb
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            // Vytvoření zákazníka přes service vrstvu
            var newCustomerId = await _customerService.CreateAsync(request);

            // Vrácení odkazu na nový detail
            return CreatedAtAction(
                nameof(GetById),
                new { id = newCustomerId },
                new { id = newCustomerId });
        }
        catch (ArgumentException ex)
        {
            // Vrácení business chyby ze service
            return BadRequest(new { message = ex.Message });
        }
    }

    // Úprava existujícího zákazníka
    [Authorize(Roles = "Admin,Manager")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var updated = await _customerService.UpdateAsync(id, request);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            // Vrácení business chyby ze service
            return BadRequest(new { message = ex.Message });
        }
    }

    // Smazání zákazníka podle ID
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _customerService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}