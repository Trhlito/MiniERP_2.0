namespace MiniERP.API.DTOs.Customers;

// Úprava existujícího zákazníka
public class UpdateCustomerRequest
{
    public string CustomerType { get; set; } = string.Empty;

    public string? CompanyName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public string? ICO { get; set; }
    public string? DIC { get; set; }
    public bool IsActive { get; set; }
}