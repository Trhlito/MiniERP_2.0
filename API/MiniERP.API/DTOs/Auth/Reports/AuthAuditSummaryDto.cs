namespace MiniERP.API.DTOs.Auth.Reports;

// DTO vrací souhrn auth audit událostí
public class AuthAuditSummaryDto
{
    // Typ auditní události
    public string ActionType { get; set; } = string.Empty;

    // Celkový počet událostí
    public int TotalCount { get; set; }

    // Počet úspěšných událostí
    public int SuccessCount { get; set; }

    // Počet neúspěšných událostí
    public int FailedCount { get; set; }

    // První výskyt události
    public DateTime? FirstEventAt { get; set; }

    // Poslední výskyt události
    public DateTime? LastEventAt { get; set; }
}