namespace MiniERP.API.DTOs.Invoices;

// DTO pro detail faktury
public class InvoiceDetailDto
{
    // ID faktury
    public int Id { get; set; }

    // Číslo faktury
    public string InvoiceNumber { get; set; } = string.Empty;

    // ID objednávky
    public int? OrderId { get; set; }

    // ID zákazníka
    public int CustomerId { get; set; }

    // Datum vystavení
    public DateTime IssueDate { get; set; }

    // Datum splatnosti
    public DateTime DueDate { get; set; }

    // Datum úhrady
    public DateTime? PaidDate { get; set; }

    // Stav faktury
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

    // ID uživatele co vytvořil fakturu
    public int CreatedByUserId { get; set; }

    // Datum vytvoření
    public DateTime CreatedAt { get; set; }

    // Datum poslední úpravy
    public DateTime? UpdatedAt { get; set; }

    // Položky faktury
    public List<InvoiceItemDetailDto> Items { get; set; } = new();
}