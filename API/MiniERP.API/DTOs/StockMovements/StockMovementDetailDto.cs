namespace MiniERP.API.DTOs.StockMovements;

// DTO pro detail pohybu skladu
public class StockMovementDetailDto
{
    // ID pohybu
    public int Id { get; set; }

    // Odkaz na skladový záznam
    public int StockId { get; set; }

    // Odkaz na sklad
    public int WarehouseId { get; set; }

    // Odkaz na produkt
    public int ProductId { get; set; }

    // Typ pohybu
    public string MovementType { get; set; } = string.Empty;

    // Množství pohybu
    public decimal Quantity { get; set; }

    // Stav množství před změnou
    public decimal QuantityBefore { get; set; }

    // Stav množství po změně
    public decimal QuantityAfter { get; set; }

    // Stav rezervace před změnou
    public decimal ReservedBefore { get; set; }

    // Stav rezervace po změně
    public decimal ReservedAfter { get; set; }

    // Typ reference
    public string? ReferenceType { get; set; }

    // ID reference
    public int? ReferenceId { get; set; }

    // Poznámka
    public string? Note { get; set; }

    // Uživatel co vytvořil záznam
    public int CreatedByUserId { get; set; }

    // Datum vytvoření
    public DateTime CreatedAt { get; set; }
}