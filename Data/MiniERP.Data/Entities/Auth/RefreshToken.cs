using System.ComponentModel.DataAnnotations.Schema;

namespace MiniERP.Data.Entities.Auth;

// Entita uchovává refresh token pro prodloužení přihlášení
public class RefreshToken
{
    // Primární klíč tokenu
    public int Id { get; set; }

    // Id uživatele, kterému token patří
    public int UserId { get; set; }

    // Hodnota refresh tokenu
    public string Token { get; set; } = string.Empty;

    // Datum expirace tokenu
    public DateTime ExpiresAt { get; set; }

    // Datum vytvoření tokenu
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Datum zneplatnění tokenu
    public DateTime? RevokedAt { get; set; }

    // Token, kterým byl tento token nahrazen
    public string? ReplacedByToken { get; set; }

    // IP adresa při vytvoření tokenu
    public string? CreatedByIp { get; set; }

    // IP adresa při zneplatnění tokenu
    public string? RevokedByIp { get; set; }

    // Navigace na uživatele
    public ApplicationUser User { get; set; } = null!;

    // Určuje, zda je token expirovaný
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    // Určuje, zda byl token ručně zneplatněný
    [NotMapped]
    public bool IsRevoked => RevokedAt is not null;

    // Určuje, zda je token aktivní
    [NotMapped]
    public bool IsActive => !IsExpired && !IsRevoked;
}