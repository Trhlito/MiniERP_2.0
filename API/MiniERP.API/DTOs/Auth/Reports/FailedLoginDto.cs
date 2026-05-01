namespace MiniERP.API.DTOs.Auth.Reports;

// Neúspěšný pokus o přihlášení
public class FailedLoginDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string? IpAddress { get; set; }
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; set; }
}