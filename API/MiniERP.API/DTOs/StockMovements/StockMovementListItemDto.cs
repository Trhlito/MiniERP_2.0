namespace MiniERP.API.DTOs.StockMovements;

// -- DTO pro seznam pohybů skladu --
public class StockMovementListItemDto
{
    // -- ID pohybu --
    public int Id { get; set; }

    // -- ID skladu --
    public int WarehouseId { get; set; }

    // -- ID produktu --
    public int ProductId { get; set; }

    // -- Typ pohybu --
    public string MovementType { get; set; } = string.Empty;

    // -- Množství pohybu --
    public decimal Quantity { get; set; }

    // -- Typ reference --
    public string? ReferenceType { get; set; }

    // -- ID reference --
    public int? ReferenceId { get; set; }

    // -- Datum vytvoření --
    public DateTime CreatedAt { get; set; }
}