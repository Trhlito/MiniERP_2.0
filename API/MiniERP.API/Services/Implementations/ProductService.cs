using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Products;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;

namespace MiniERP.API.Services.Implementations;

// Implementace služby pro produkty
public class ProductService : IProductService
{
    // Databázový kontext
    private readonly ApplicationDbContext _db;

    public ProductService(ApplicationDbContext db)
    {
        _db = db;
    }

    // Načtení seznamu produktů
    public async Task<List<ProductListItemDto>> GetAllAsync()
    {
        return await _db.Products
            .AsNoTracking()
            .Select(p => new ProductListItemDto
            {
                Id = p.Id,
                Name = p.Name,
                SalePrice = p.SalePrice,
                Unit = p.Unit,
                IsActive = p.IsActive
            })
        .ToListAsync();    
    }    

    // Vytvoření nového produktu
    public async Task<int> CreateAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            SupplierId = request.SupplierId,
            PurchasePrice = request.PurchasePrice,
            SalePrice = request.SalePrice,
            VatRate = request.VatRate,
            Unit = request.Unit,
            MinimumStock = request.MinimumStock,
            IsService = request.IsService,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return product.Id;
    }

    // Načtení detailu produktu podle ID
    public async Task<ProductDetailDto?> GetByIdAsync(int id)
    {
        return await _db.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductDetailDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                SupplierId = p.SupplierId,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                VatRate = p.VatRate,
                Unit = p.Unit,
                MinimumStock = p.MinimumStock,
                IsService = p.IsService,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync();
    }

    // Úprava existujícího produktu podle ID
    public async Task<bool> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return false;
        }

        product.Code = request.Code;
        product.Name = request.Name;
        product.Description = request.Description;
        product.CategoryId = request.CategoryId;
        product.SupplierId = request.SupplierId;
        product.PurchasePrice = request.PurchasePrice;
        product.SalePrice = request.SalePrice;
        product.VatRate = request.VatRate;
        product.Unit = request.Unit;
        product.MinimumStock = request.MinimumStock;
        product.IsService = request.IsService;
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return true;
    }

    // Smazání produktu podle ID
    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return false;
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        return true;
    }
}