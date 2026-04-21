using MiniERP.API.DTOs.Customers;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní pro práci se zákazníky
public interface ICustomerService
{
    // Načtení seznamu všech zákazníků
    Task<List<CustomerListItemDto>> GetAllAsync();

    // Načtení detailu zákazníka podle ID
    Task<CustomerDetailDto?> GetByIdAsync(int id);

    // Vytvoření nového zákazníka
    Task<int> CreateAsync(CreateCustomerRequest request);

    // Úprava existujícího zákazníka podle ID
    Task<bool> UpdateAsync(int id, UpdateCustomerRequest request);

    // Smazání zákazníka podle ID
    Task<bool> DeleteAsync(int id);
}