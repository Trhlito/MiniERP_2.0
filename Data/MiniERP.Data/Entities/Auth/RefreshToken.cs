using System.ComponentModel.DataAnnotations.Schema;

namespace MiniERP.Data.Entities.Auth;

// Uchování refresh tokenu 
public class RefreshToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? CreatedByIp { get; set; }
    public string? RevokedByIp { get; set; }
    public ApplicationUser User { get; set; } = null!;
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    [NotMapped]
    public bool IsRevoked => RevokedAt is not null;
    [NotMapped]
    public bool IsActive => !IsExpired && !IsRevoked;
}