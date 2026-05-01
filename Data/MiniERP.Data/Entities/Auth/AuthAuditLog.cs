namespace MiniERP.Data.Entities.Auth;

// Entita uchovává audit autentizačních událostí
public class AuthAuditLog
{
    // Primární klíč záznamu
    public long Id { get; set; }

    // Id uživatele
    public int? UserId { get; set; }

    // Uživatelské jméno
    public string? UserName { get; set; }

    // Typ akce
    public string ActionType { get; set; } = string.Empty;

    // Informace o úspěchu operace
    public bool Success { get; set; }

    // IP adresa klienta
    public string? IpAddress { get; set; }

    // Důvod selhání
    public string? FailureReason { get; set; }

    // Datum vytvoření záznamu
    public DateTime CreatedAt { get; set; }
}