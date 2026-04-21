using MiniERP.API.DTOs.Orders;
using MiniERP.API.Services.Results;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní pro objednávky
public interface IOrderService
{
    // Načtení seznamu objednávek
    Task<List<OrderListItemDto>> GetAllAsync();

    // Načtení detailu objednávky podle ID
    Task<OrderDetailDto?> GetByIdAsync(int id);

    // Vytvoření nové objednávky
    Task<int> CreateAsync(CreateOrderRequest request);

    // Úprava objednávky podle ID
    Task<bool> UpdateAsync(int id, UpdateOrderRequest request);

    // Smazání objednávky podle ID
    Task<bool> DeleteAsync(int id);

    // Rezervace skladu pro objednávku
    Task<ReserveStockResult> ReserveStockAsync(int orderId);

    // Uvolnění rezervace skladu pro objednávku
    Task<ReserveStockResult> ReleaseStockAsync(int orderId);
}