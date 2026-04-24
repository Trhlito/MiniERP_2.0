using MiniERP.API.DTOs.Reports;

namespace MiniERP.API.Services.Interfaces;

// Služba pro reportovací výstupy
public interface IReportService
{
    // Vrací souhrn obchodních dat za zadané období
    Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime dateFrom, DateTime dateTo);

    // Vrací přehled neuhrazených faktur
    Task<List<UnpaidInvoiceDto>> GetUnpaidInvoicesAsync();

    // Vrací přehled skladových upozornění
    Task<List<StockAlertDto>> GetStockAlertsAsync();

    // Vrací obchodní výsledky podle zákazníků za zadané období
    Task<List<SalesByCustomerDto>> GetSalesByCustomerAsync(DateTime dateFrom, DateTime dateTo);

    // Vrací přehled nejprodávanějších produktů za zadané období
    Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(DateTime dateFrom, DateTime dateTo);
}