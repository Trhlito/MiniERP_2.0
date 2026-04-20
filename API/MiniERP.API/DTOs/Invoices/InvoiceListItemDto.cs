namespace MiniERP.API.DTOs.Invoices;

// -- DTO pro seznam faktur --
public class InvoiceListItemDto
{
    // -- ID faktury --
    public int Id { get; set; }

    // -- Číslo faktury --
    public string InvoiceNumber { get; set; } = string.Empty;

    // -- ID objednávky --
    public int? OrderId { get; set; }

    // -- ID zákazníka --
    public int CustomerId { get; set; }

    // -- Datum vystavení --
    public DateTime IssueDate { get; set; }

    // -- Stav faktury --
    public string Status { get; set; } = string.Empty;

    // -- Celková částka --
    public decimal TotalAmount { get; set; }

    // -- Měna --
    public string Currency { get; set; } = string.Empty;
}