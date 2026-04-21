namespace MiniERP.API.DTOs.Orders;

// Request DTO pro vytvoření položky objednávky
public class CreateOrderItemRequest
{
    // ID produktu
    public int ProductId { get; set; }

    // Název položky
    public string ItemName { get; set; } = string.Empty;

    // Množství
    public decimal Quantity { get; set; }

    // Cena za jednotku
    public decimal UnitPrice { get; set; }

    // Sazba DPH
    public decimal VatRate { get; set; }

    // Sleva v procentech
    public decimal? DiscountPercent { get; set; }
}