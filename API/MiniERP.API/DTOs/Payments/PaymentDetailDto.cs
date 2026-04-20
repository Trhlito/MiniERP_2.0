namespace MiniERP.API.DTOs.Payments;

// -- DTO pro detail platby --
public class PaymentDetailDto
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

    // -- Referenční číslo --
    public string? ReferenceNumber { get; set; }

    // -- Poznámka --
    public string? Note { get; set; }

    // -- ID uživatele, který platbu vytvořil --
    public int CreatedByUserId { get; set; }

    // -- Datum vytvoření --
    public DateTime CreatedAt { get; set; }
}