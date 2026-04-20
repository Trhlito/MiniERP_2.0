using MiniERP.API.DTOs.Stock;

namespace MiniERP.API.Services.Interfaces;

    // -- Rozhraní služby pro sklad --
public interface IStockService
{
    // -- Vrátí seznam skladových záznamů --
    Task<List<StockListItemDto>> GetAllAsync();

    // -- Vrátí detail skladového záznamu podle ID --
    Task<StockDetailDto?> GetByIdAsync(int id);

    // -- Upraví skladový záznam podle ID --
    Task<bool> UpdateAsync(int id, UpdateStockRequest request);

    // -- Smaže skladový záznam podle ID --
    Task<bool> DeleteAsync(int id);

    // -- Vytvoří nový skladový záznam --
    Task<int> CreateAsync(CreateStockRequest request);
}