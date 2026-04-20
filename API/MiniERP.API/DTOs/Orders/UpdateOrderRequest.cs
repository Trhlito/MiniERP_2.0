namespace MiniERP.API.DTOs.Orders;

// -- Request DTO pro úpravu hlavičky objednávky --
public class UpdateOrderRequest
{
    // -- Požadovaný termín --
    public DateTime? RequiredDate { get; set; }

    // -- Stav objednávky --
    public string Status { get; set; } = string.Empty;

    // -- Měna --
    public string Currency { get; set; } = string.Empty;

    // -- Poznámka --
    public string? Note { get; set; }
}