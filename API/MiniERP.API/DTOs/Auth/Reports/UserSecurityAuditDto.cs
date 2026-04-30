namespace MiniERP.API.DTOs.Auth.Reports;

// DTO vrací bezpečnostní audit konkrétního uživatele
public class UserSecurityAuditDto
{
    // Id auditního záznamu
    public int Id { get; set; }

    // Id uživatele
    public int? UserId { get; set; }

    // Uživatelské jméno
    public string? UserName { get; set; }

    // Typ auditní události
    public string ActionType { get; set; } = string.Empty;

    // Stav úspěšnosti události
    public bool Success { get; set; }

    // IP adresa požadavku
    public string? IpAddress { get; set; }

    // Důvod selhání
    public string? FailureReason { get; set; }

    // Datum vytvoření záznamu
    public DateTime CreatedAt { get; set; }
}