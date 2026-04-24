using Microsoft.AspNetCore.Mvc;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// Controller pro reportovací endpointy
public class ReportsController : ControllerBase
{
    // Reportovací služba
    private readonly IReportService _reportService;

    // Konstruktor pro injekci reportovací služby
    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("sales-summary")]
    // Endpoint vrací souhrn obchodních dat za zadané období
    public async Task<IActionResult> GetSalesSummary([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        // Načtení reportu ze služby
        var result = await _reportService.GetSalesSummaryAsync(dateFrom, dateTo);

        // Vrácení výsledku do API odpovědi
        return Ok(result);
    }

    [HttpGet("unpaid-invoices")]
    // Endpoint vrací přehled neuhrazených faktur
    public async Task<IActionResult> GetUnpaidInvoices()
    {
        // Načtení reportu ze služby
        var result = await _reportService.GetUnpaidInvoicesAsync();

        // Vrácení výsledku do API odpovědi
        return Ok(result);
    }

    [HttpGet("stock-alerts")]
    // Endpoint vrací přehled skladových upozornění
    public async Task<IActionResult> GetStockAlerts()
    {
        // Načtení reportu ze služby
        var result = await _reportService.GetStockAlertsAsync();

        // Vrácení výsledku do API odpovědi
        return Ok(result);
    }

    [HttpGet("sales-by-customer")]
    // Endpoint vrací obchodní výsledky podle zákazníků za zadané období
    public async Task<IActionResult> GetSalesByCustomer([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        // Načtení reportu ze služby
        var result = await _reportService.GetSalesByCustomerAsync(dateFrom, dateTo);

        // Vrácení výsledku do API odpovědi
        return Ok(result);
    }

    [HttpGet("top-selling-products")]
        // Endpoint vrací přehled nejprodávanějších produktů za zadané období
    public async Task<IActionResult> GetTopSellingProducts([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        // Načtení reportu ze služby
        var result = await _reportService.GetTopSellingProductsAsync(dateFrom, dateTo);

        // Vrácení výsledku do API odpovědi
        return Ok(result);
    }
}