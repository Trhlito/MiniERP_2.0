namespace MiniERP.API.DTOs.Payments;

// -- DTO pro seznam plateb --
public class PaymentListItemDto
{
    // -- ID platby --
    public int Id { get; set; }

    // -- ID faktury --
    public int InvoiceId { get; set; }

    // -- Datum platby --
    public DateTime PaymentDate { get; set; }

    // -- Částka platby --
    public decimal Amount { get; set; }

    // -- Metoda platby --
    public string PaymentMethod { get; set; } = string.Empty;

    // -- Referenční číslo platby --
    public string? ReferenceNumber { get; set; }
}