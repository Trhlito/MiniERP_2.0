using MiniERP.API.DTOs.StockMovements;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní pro auditní pohyby skladu
public interface IStockMovementService
{
    // Načtení seznamu pohybů skladu
    Task<List<StockMovementListItemDto>> GetAllAsync();

    // Načtení detailu pohybu skladu podle ID
    Task<StockMovementDetailDto?> GetByIdAsync(int id);
}