using MiniERP.API.DTOs.StockMovements;

namespace MiniERP.API.Services.Interfaces;

// -- Rozhraní služby pro auditní pohyby skladu --
public interface IStockMovementService
{
    // -- Vrátí seznam pohybů skladu --
    Task<List<StockMovementListItemDto>> GetAllAsync();

    // -- Vrátí detail pohybu skladu podle ID --
    Task<StockMovementDetailDto?> GetByIdAsync(int id);
}