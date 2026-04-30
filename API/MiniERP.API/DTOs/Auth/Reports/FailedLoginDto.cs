namespace MiniERP.API.DTOs.Auth.Reports;

// DTO vrací neúspěšný pokus o přihlášení
public class FailedLoginDto
{
    // Id auditního záznamu
    public int Id { get; set; }

    // Id uživatele
    public int? UserId { get; set; }

    // Uživatelské jméno
    public string? UserName { get; set; }

    // IP adresa požadavku
    public string? IpAddress { get; set; }

    // Důvod selhání
    public string? FailureReason { get; set; }

    // Datum vytvoření záznamu
    public DateTime CreatedAt { get; set; }
}