namespace MiniERP.API.DTOs.Orders;

// Žádost pro úpravu hlavičky objednávky
public class UpdateOrderRequest
{
    public DateTime? RequiredDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string? Note { get; set; }
}