using Microsoft.AspNetCore.Mvc;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

// Controller pro faktury
[ApiController]
[Route("api/[controller]")]
public class InvoicesController : ControllerBase
{
    // Service vrstva pro faktury
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    // Načtení seznamu faktur
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var invoices = await _invoiceService.GetAllAsync();
        return Ok(invoices);
    }

    // Načtení detailu faktury podle ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var invoice = await _invoiceService.GetByIdAsync(id);

        if (invoice == null)
        {
            return NotFound();
        }

        return Ok(invoice);
    }

    // Vytvoření faktury z objednávky
    [HttpPost("from-order/{orderId:int}")]
    public async Task<IActionResult> CreateFromOrder(int orderId)
    {
        var result = await _invoiceService.CreateFromOrderAsync(orderId);

        if (!result.Success)
        {
            if (result.ErrorCode == "ORDER_NOT_FOUND")
            {
                return NotFound(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.InvoiceId!.Value },
            new { id = result.InvoiceId.Value });
    }
}