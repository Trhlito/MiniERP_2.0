namespace MiniERP.API.DTOs.Auth.Reports;

// Bezpečnostní audit konkrétního uživatele
public class UserSecurityAuditDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? IpAddress { get; set; }
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; set; }
}