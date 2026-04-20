using MiniERP.API.DTOs.Orders;
using MiniERP.API.Services.Results;

namespace MiniERP.API.Services.Interfaces;

// -- Rozhraní služby pro objednávky --
public interface IOrderService
{
    Task<List<OrderListItemDto>> GetAllAsync();
    Task<OrderDetailDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateOrderRequest request);
    Task<bool> UpdateAsync(int id, UpdateOrderRequest request);
    Task<bool> DeleteAsync(int id);

    Task<ReserveStockResult> ReserveStockAsync(int orderId);
    Task<ReserveStockResult> ReleaseStockAsync(int orderId);
}