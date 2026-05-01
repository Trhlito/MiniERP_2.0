namespace MiniERP.API.DTOs.Auth.Reports;

// Souhrn auth audit událostí
public class AuthAuditSummaryDto
{
    public string ActionType { get; set; } = string.Empty;
    public int TotalCount { get; set; }
    public int SuccessCount { get; set; }
    public int FailedCount { get; set; }
    public DateTime? FirstEventAt { get; set; }
    public DateTime? LastEventAt { get; set; }
}