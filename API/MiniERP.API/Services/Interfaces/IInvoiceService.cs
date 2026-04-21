using MiniERP.API.DTOs.Invoices;
using MiniERP.API.Services.Results;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní pro faktury
public interface IInvoiceService
{
    // Načtení seznamu faktur
    Task<List<InvoiceListItemDto>> GetAllAsync();

    // Načtení detailu faktury podle ID
    Task<InvoiceDetailDto?> GetByIdAsync(int id);

    // Vytvoření faktury z objednávky
    Task<CreateInvoiceFromOrderResult> CreateFromOrderAsync(int orderId);
}