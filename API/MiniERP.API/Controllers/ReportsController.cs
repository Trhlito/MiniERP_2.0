using Microsoft.AspNetCore.Mvc;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    // Souhrn obchodních dat za zadané období
    [HttpGet("sales-summary")]
    public async Task<IActionResult> GetSalesSummary([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        // Načtení reportu ze služby
        var result = await _reportService.GetSalesSummaryAsync(dateFrom, dateTo);

        // Vrácení výsledku do API odpovědi
        return Ok(result);
    }

    // Přehled neuhrazených faktur
    [HttpGet("unpaid-invoices")]
    public async Task<IActionResult> GetUnpaidInvoices()
    {
        var result = await _reportService.GetUnpaidInvoicesAsync();

        return Ok(result);
    }
    // Přehled skladových upozornění
    [HttpGet("stock-alerts")]
    public async Task<IActionResult> GetStockAlerts()
    {
        var result = await _reportService.GetStockAlertsAsync();

        return Ok(result);
    }

    // Obchodní výsledky podle zákazníků za zadané období
    [HttpGet("sales-by-customer")]
    public async Task<IActionResult> GetSalesByCustomer([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        var result = await _reportService.GetSalesByCustomerAsync(dateFrom, dateTo);

        return Ok(result);
    }

    // Přehled nejprodávanějších produktů za zadané období
    [HttpGet("top-selling-products")]
    public async Task<IActionResult> GetTopSellingProducts([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        var result = await _reportService.GetTopSellingProductsAsync(dateFrom, dateTo);
        
        return Ok(result);
    }
}