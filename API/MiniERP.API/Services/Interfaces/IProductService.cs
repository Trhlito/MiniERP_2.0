using MiniERP.API.DTOs.Products;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní pro produkty
public interface IProductService
{
    // Načtení seznamu produktů
    Task<List<ProductListItemDto>> GetAllAsync();

    // Načtení detailu produktu podle ID
    Task<ProductDetailDto?> GetByIdAsync(int id);

    // Vytvoření nového produktu
    Task<int> CreateAsync(CreateProductRequest request);

    // Úprava produktu podle ID
    Task<bool> UpdateAsync(int id, UpdateProductRequest request);

    // Smazání produktu podle ID
    Task<bool> DeleteAsync(int id);
}