using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Customers;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;
using MiniERP.Data.Entities;

namespace MiniERP.API.Services.Implementations;

// -- Implementace služby pro práci se zákazníky --
public class CustomerService : ICustomerService
{
    // -- Databázový kontext pro přístup k tabulce Customers --
    private readonly ApplicationDbContext _db;

    // -- Konstruktor pro injektování databázového kontextu --
    public CustomerService(ApplicationDbContext db)
    {
        _db = db;
    }

    // -- Vrátí seznam všech zákazníků ve formě list DTO --
    public async Task<List<CustomerListItemDto>> GetAllAsync()
    {
        return await _db.Customers
            .AsNoTracking()
            .OrderBy(c => c.CompanyName ?? c.LastName ?? c.FirstName)
            .Select(c => new CustomerListItemDto
            {
                Id = c.Id,
                CustomerType = c.CustomerType,
                CustomerName = c.CustomerType == "Company"
                    ? (c.CompanyName ?? string.Empty)
                    : $"{c.FirstName} {c.LastName}".Trim(),
                Email = c.Email,
                Phone = c.Phone,
                IsActive = c.IsActive
            })
            .ToListAsync();
    }

    // -- Vrátí detail jednoho zákazníka podle ID --
    public async Task<CustomerDetailDto?> GetByIdAsync(int id)
    {
        return await _db.Customers
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CustomerDetailDto
            {
                Id = c.Id,
                CustomerType = c.CustomerType,
                CompanyName = c.CompanyName,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Street = c.Street,
                City = c.City,
                ZipCode = c.ZipCode,
                Country = c.Country,
                ICO = c.ICO,
                DIC = c.DIC,
                IsActive = c.IsActive
            })
            .FirstOrDefaultAsync();
    }

    // -- Vytvoří nového zákazníka a vrátí jeho ID --
    public async Task<int> CreateAsync(CreateCustomerRequest request)
    {
        ValidateCustomer(
            request.CustomerType,
            request.CompanyName,
            request.FirstName,
            request.LastName);

        var customer = new Customer
        {
            CustomerType = request.CustomerType,
            CompanyName = request.CompanyName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Street = request.Street,
            City = request.City,
            ZipCode = request.ZipCode,
            Country = request.Country,
            ICO = request.ICO,
            DIC = request.DIC,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();

        return customer.Id;
    }

    // -- Upraví existujícího zákazníka podle ID --
    public async Task<bool> UpdateAsync(int id, UpdateCustomerRequest request)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null)
        {
            return false;
        }

        ValidateCustomer(
            request.CustomerType,
            request.CompanyName,
            request.FirstName,
            request.LastName);

        customer.CustomerType = request.CustomerType;
        customer.CompanyName = request.CompanyName;
        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.Street = request.Street;
        customer.City = request.City;
        customer.ZipCode = request.ZipCode;
        customer.Country = request.Country;
        customer.ICO = request.ICO;
        customer.DIC = request.DIC;
        customer.IsActive = request.IsActive;
        customer.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return true;
    }

    // -- Smaže zákazníka podle ID --
    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null)
        {
            return false;
        }

        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();

        return true;
    }

    // -- Privátní validace typu zákazníka a povinných polí --
    private static void ValidateCustomer(
        string customerType,
        string? companyName,
        string? firstName,
        string? lastName)
    {
        if (customerType != "Company" && customerType != "Person")
        {
            throw new ArgumentException("CustomerType musí být 'Company' nebo 'Person'.");
        }

        if (customerType == "Company" && string.IsNullOrWhiteSpace(companyName))
        {
            throw new ArgumentException("Firma musí mít vyplněný CompanyName.");
        }

        if (customerType == "Person" &&
            (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName)))
        {
            throw new ArgumentException("Osoba musí mít vyplněné FirstName a LastName.");
        }
    }
}