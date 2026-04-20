namespace MiniERP.API.DTOs.Orders;

// -- DTO pro seznam objednávek --
public class OrderListItemDto
{
    public int Id { get; set; }

    // -- Číslo objednávky --
    public string OrderNumber { get; set; } = string.Empty;

    // -- ID zákazníka --
    public int CustomerId { get; set; }

    // -- Datum objednávky --
    public DateTime OrderDate { get; set; }

    // -- Stav objednávky --
    public string Status { get; set; } = string.Empty;

    // -- Celková částka --
    public decimal TotalAmount { get; set; }

    // -- Měna --
    public string Currency { get; set; } = string.Empty;
}