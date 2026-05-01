namespace MiniERP.API.DTOs.Orders;

// Žádost pro vytvoření položky objednávky
public class CreateOrderItemRequest
{
    public int ProductId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatRate { get; set; }
    public decimal? DiscountPercent { get; set; }
}