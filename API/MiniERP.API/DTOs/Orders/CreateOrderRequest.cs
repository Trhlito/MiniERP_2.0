namespace MiniERP.API.DTOs.Orders;

// -- Request DTO pro vytvoření objednávky --
public class CreateOrderRequest
{
    // -- Číslo objednávky --
    public string OrderNumber { get; set; } = string.Empty;

    // -- ID zákazníka --
    public int CustomerId { get; set; }

    // -- Datum objednávky --
    public DateTime OrderDate { get; set; }

    // -- Požadovaný termín --
    public DateTime? RequiredDate { get; set; }

    // -- Stav objednávky --
    public string Status { get; set; } = string.Empty;

    // -- Měna --
    public string Currency { get; set; } = string.Empty;

    // -- Poznámka --
    public string? Note { get; set; }

    // -- ID uživatele, který objednávku vytváří --
    public int CreatedByUserId { get; set; }

    // -- Položky objednávky --
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}