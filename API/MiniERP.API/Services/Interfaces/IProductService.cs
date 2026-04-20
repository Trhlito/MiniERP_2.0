using MiniERP.API.DTOs.Products;

namespace MiniERP.API.Services.Interfaces;

// -- Rozhraní služby pro produkty --
public interface IProductService
{
    Task<List<ProductListItemDto>> GetAllAsync();
    Task<ProductDetailDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateProductRequest request);
    Task<bool> UpdateAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteAsync(int id);
}