using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.StockMovements;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;

namespace MiniERP.API.Services.Implementations;

// -- Implementace služby pro auditní pohyby skladu --
public class StockMovementService : IStockMovementService
{
    // -- Databázový kontext --
    private readonly ApplicationDbContext _db;

    public StockMovementService(ApplicationDbContext db)
    {
        _db = db;
    }

    // -- Vrátí seznam pohybů skladu --
    public async Task<List<StockMovementListItemDto>> GetAllAsync()
    {
        return await _db.StockMovements
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new StockMovementListItemDto
            {
                Id = x.Id,
                WarehouseId = x.WarehouseId,
                ProductId = x.ProductId,
                MovementType = x.MovementType,
                Quantity = x.Quantity,
                ReferenceType = x.ReferenceType,
                ReferenceId = x.ReferenceId,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    // -- Vrátí detail pohybu skladu podle ID --
    public async Task<StockMovementDetailDto?> GetByIdAsync(int id)
    {
        return await _db.StockMovements
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new StockMovementDetailDto
            {
                Id = x.Id,
                StockId = x.StockId,
                WarehouseId = x.WarehouseId,
                ProductId = x.ProductId,
                MovementType = x.MovementType,
                Quantity = x.Quantity,
                QuantityBefore = x.QuantityBefore,
                QuantityAfter = x.QuantityAfter,
                ReservedBefore = x.ReservedBefore,
                ReservedAfter = x.ReservedAfter,
                ReferenceType = x.ReferenceType,
                ReferenceId = x.ReferenceId,
                Note = x.Note,
                CreatedByUserId = x.CreatedByUserId,
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync();
    }
}