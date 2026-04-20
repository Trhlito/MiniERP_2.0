namespace MiniERP.API.DTOs.Payments;

// -- Request DTO pro vytvoření platby --
public class CreatePaymentRequest
{
    // -- ID faktury --
    public int InvoiceId { get; set; }

    // -- Datum platby --
    public DateTime PaymentDate { get; set; }

    // -- Částka platby --
    public decimal Amount { get; set; }

    // -- Metoda platby --
    public string PaymentMethod { get; set; } = string.Empty;

    // -- Referenční číslo --
    public string? ReferenceNumber { get; set; }

    // -- Poznámka --
    public string? Note { get; set; }

    // -- ID uživatele, který platbu vytváří --
    public int CreatedByUserId { get; set; }
}