using MiniERP.API.DTOs.Invoices;
using MiniERP.API.Services.Results;

namespace MiniERP.API.Services.Interfaces;

// -- Rozhraní služby pro faktury --
public interface IInvoiceService
{
    // -- Vrátí seznam faktur --
    Task<List<InvoiceListItemDto>> GetAllAsync();

    // -- Vrátí detail faktury podle ID --
    Task<InvoiceDetailDto?> GetByIdAsync(int id);

    // -- Vytvoří fakturu z objednávky --
    Task<CreateInvoiceFromOrderResult> CreateFromOrderAsync(int orderId);
}