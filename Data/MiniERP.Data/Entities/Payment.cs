namespace MiniERP.Data.Entities;

// Entita platby
public class Payment
{
    // Primární klíč
    public int Id { get; set; }

    // Odkaz na fakturu
    public int InvoiceId { get; set; }

    // Datum platby
    public DateTime PaymentDate { get; set; }

    // Částka platby
    public decimal Amount { get; set; }

    // Metoda platby
    public string PaymentMethod { get; set; } = string.Empty;

    // Referenční číslo platby
    public string? ReferenceNumber { get; set; }

    // Poznámka
    public string? Note { get; set; }

    // Uživatel co vytvořil platbu
    public int CreatedByUserId { get; set; }

    // Datum vytvoření
    public DateTime CreatedAt { get; set; }
}