using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Customers;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

// -- Controller pro práci se zákazníky přes API --
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    // -- Service vrstva pro logiku zákazníků --
    private readonly ICustomerService _customerService;

    // -- Konstruktor pro dependency injection CustomerService --
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // -- Endpoint pro načtení seznamu všech zákazníků --
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }

    // -- Endpoint pro načtení detailu jednoho zákazníka podle ID --
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

    // -- Endpoint pro vytvoření nového zákazníka --
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        // -- Pokud validace neprošla, vrátíme chyby --
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            // -- Vytvoření nového zákazníka přes service vrstvu --
            var newCustomerId = await _customerService.CreateAsync(request);

            // -- Vrácení 201 Created + odkaz na detail nového zákazníka --
            return CreatedAtAction(
                nameof(GetById),
                new { id = newCustomerId },
                new { id = newCustomerId });
        }
        catch (ArgumentException ex)
        {
            // -- Dočasně ponechané pro business chyby ze service --
            return BadRequest(new { message = ex.Message });
        }
    }

    // -- Endpoint pro úpravu existujícího zákazníka --
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerRequest request)
    {
        // -- Pokud validace neprošla, vrátíme chyby --
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            // -- Pokus o update zákazníka --
            var updated = await _customerService.UpdateAsync(id, request);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            // -- Dočasně ponechané pro business chyby ze service --
            return BadRequest(new { message = ex.Message });
        }
    }

    // -- Endpoint pro smazání zákazníka podle ID --
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