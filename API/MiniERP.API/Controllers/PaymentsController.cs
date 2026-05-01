using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Payments;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    // Načtení seznamu plateb
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var payments = await _paymentService.GetAllAsync();
        return Ok(payments);
    }

    // Načtení detailu platby podle ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        return Ok(payment);
    }

    // Vytvoření nové platby
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var newPaymentId = await _paymentService.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = newPaymentId },
                new
                {
                    Id = newPaymentId,
                    Message = "Platba byla úspěšně vytvořena."
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
}