using MiniERP.API.DTOs.Payments;

namespace MiniERP.API.Services.Interfaces;

// -- Rozhraní služby pro platby --
public interface IPaymentService
{
    // -- Vrátí seznam plateb --
    Task<List<PaymentListItemDto>> GetAllAsync();

    // -- Vrátí detail platby podle ID --
    Task<PaymentDetailDto?> GetByIdAsync(int id);

    // -- Vytvoří novou platbu --
    Task<int> CreateAsync(CreatePaymentRequest request);
}