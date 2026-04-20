using MiniERP.API.DTOs.Customers;

namespace MiniERP.API.Services.Interfaces;

// -- Rozhraní definující operace pro práci se zákazníky --
public interface ICustomerService
{
    // -- Vrátí seznam všech zákazníků pro přehled / grid --
    Task<List<CustomerListItemDto>> GetAllAsync();

    // -- Vrátí detail jednoho zákazníka podle ID --
    Task<CustomerDetailDto?> GetByIdAsync(int id);

    // -- Vytvoří nového zákazníka a vrátí jeho nově vytvořené ID --
    Task<int> CreateAsync(CreateCustomerRequest request);

    // -- Upraví existujícího zákazníka podle ID a vrátí informaci o úspěchu --
    Task<bool> UpdateAsync(int id, UpdateCustomerRequest request);

    // -- Smaže zákazníka podle ID a vrátí informaci o úspěchu --
    Task<bool> DeleteAsync(int id);
}