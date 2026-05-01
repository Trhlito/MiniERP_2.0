namespace MiniERP.API.DTOs.Orders;

// Žádost pro vytvoření objednávky
public class CreateOrderRequest
{
    public string OrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string? Note { get; set; }
    public int CreatedByUserId { get; set; }
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}