using MiniERP.API.DTOs.Stock;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní pro sklad
public interface IStockService
{
    // Načtení seznamu skladových záznamů
    Task<List<StockListItemDto>> GetAllAsync();

    // Načtení detailu skladového záznamu podle ID
    Task<StockDetailDto?> GetByIdAsync(int id);

    // Úprava skladového záznamu podle ID
    Task<bool> UpdateAsync(int id, UpdateStockRequest request);

    // Smazání skladového záznamu podle ID
    Task<bool> DeleteAsync(int id);

    // Vytvoření nového skladového záznamu
    Task<int> CreateAsync(CreateStockRequest request);
}