using MiniERP.API.DTOs.Payments;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní pro platby
public interface IPaymentService
{
    // Načtení seznamu plateb
    Task<List<PaymentListItemDto>> GetAllAsync();

    // Načtení detailu platby podle ID
    Task<PaymentDetailDto?> GetByIdAsync(int id);

    // Vytvoření nové platby
    Task<int
    > CreateAsync(CreatePaymentRequest request);
}