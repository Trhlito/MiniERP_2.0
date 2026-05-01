namespace MiniERP.API.DTOs.Payments;

// Detail platby
public class PaymentDetailDto
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public string? Note { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}