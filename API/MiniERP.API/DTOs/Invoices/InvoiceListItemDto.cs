namespace MiniERP.API.DTOs.Invoices;

// Seznam faktur
public class InvoiceListItemDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int? OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime IssueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
}