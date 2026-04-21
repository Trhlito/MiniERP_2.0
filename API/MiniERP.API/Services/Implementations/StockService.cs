using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Stock;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;

namespace MiniERP.API.Services.Implementations;

// Implementace služby pro sklad
public class StockService : IStockService
{
    // Databázový kontext
    private readonly ApplicationDbContext _db;

    public StockService(ApplicationDbContext db)
    {
        _db = db;
    }

    // Načtení seznamu skladových záznamů
    public async Task<List<StockListItemDto>> GetAllAsync()
    {
        return await _db.Stock
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .Select(s => new StockListItemDto
            {
                Id = s.Id,
                WarehouseId = s.WarehouseId,
                ProductId = s.ProductId,
                Quantity = s.Quantity,
                ReservedQuantity = s.ReservedQuantity
            })
            .ToListAsync();
    }

    // Načtení detailu skladového záznamu podle ID
    public async Task<StockDetailDto?> GetByIdAsync(int id)
    {
        return await _db.Stock
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new StockDetailDto
            {
                Id = s.Id,
                WarehouseId = s.WarehouseId,
                ProductId = s.ProductId,
                Quantity = s.Quantity,
                ReservedQuantity = s.ReservedQuantity,
                LastUpdatedAt = s.LastUpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    // Úprava skladového záznamu podle ID
    public async Task<bool> UpdateAsync(int id, UpdateStockRequest request)
    {
        var stock = await _db.Stock.FirstOrDefaultAsync(s => s.Id == id);

        if (stock == null)
        {
            return false;
        }

        stock.Quantity = request.Quantity;
        stock.ReservedQuantity = request.ReservedQuantity;
        stock.LastUpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return true;
    }

    // Smazání skladového záznamu podle ID
    public async Task<bool> DeleteAsync(int id)
    {
        var stock = await _db.Stock.FirstOrDefaultAsync(s => s.Id == id);

        if (stock == null)
        {
            return false;
        }

        _db.Stock.Remove(stock);
        await _db.SaveChangesAsync();

        return true;
    }

    // Vytvoření nového skladového záznamu
    public async Task<int> CreateAsync(CreateStockRequest request)
    {
        var stock = new MiniERP.Data.Entities.Stock
        {
            WarehouseId = request.WarehouseId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            ReservedQuantity = request.ReservedQuantity,
            LastUpdatedAt = DateTime.UtcNow
        };

        _db.Stock.Add(stock);
        await _db.SaveChangesAsync();

        return stock.Id;
    }
}