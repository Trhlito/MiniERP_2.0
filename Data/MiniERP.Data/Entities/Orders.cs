namespace MiniERP.Data.Entities;

// Entita objednávky
public class Order
{
    // Primární klíč
    public int Id { get; set; }

    // Číslo objednávky
    public string OrderNumber { get; set; } = string.Empty;

    // Odkaz na zákazníka
    public int CustomerId { get; set; }

    // Datum objednávky
    public DateTime OrderDate { get; set; }

    // Požadovaný termín
    public DateTime? RequiredDate { get; set; }

    // Stav objednávky
    public string Status { get; set; } = string.Empty;

    // Mezisoučet bez DPH
    public decimal Subtotal { get; set; }

    // Celkové DPH
    public decimal VatTotal { get; set; }

    // Celková částka
    public decimal TotalAmount { get; set; }

    // Měna
    public string Currency { get; set; } = string.Empty;

    // Poznámka
    public string? Note { get; set; }

    // Uživatel který objednávku vytvořil
    public int CreatedByUserId { get; set; }

    // Datum vytvoření
    public DateTime CreatedAt { get; set; }

    // Datum poslední úpravy
    public DateTime? UpdatedAt { get; set; }
}