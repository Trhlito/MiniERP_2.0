namespace MiniERP.Data.Entities;

// Entita auditního pohybu skladu
public class StockMovement
{
    // Primární klíč
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

    // Množství na skladě před změnou
    public decimal QuantityBefore { get; set; }

    // Množství na skladě po změně
    public decimal QuantityAfter { get; set; }

    // Rezervované množství před změnou
    public decimal ReservedBefore { get; set; }

    // Rezervované množství po změně
    public decimal ReservedAfter { get; set; }

    // Typ reference
    public string? ReferenceType { get; set; }

    // ID reference
    public int? ReferenceId { get; set; }

    // Poznámka
    public string? Note { get; set; }

    // Uživatel co akci provedl
    public int CreatedByUserId { get; set; }

    // Datum vytvoření záznamu
    public DateTime CreatedAt { get; set; }
}