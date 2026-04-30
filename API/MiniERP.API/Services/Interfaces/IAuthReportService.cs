using MiniERP.API.DTOs.Auth.Reports;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní definuje reporty pro autentizaci
public interface IAuthReportService
{
    // Metoda vrátí souhrn auth audit událostí
    Task<List<AuthAuditSummaryDto>> GetAuthAuditSummaryAsync(
        DateTime? fromDate,
        DateTime? toDate);
    
    // Metoda vrátí bezpečnostní audit konkrétního uživatele
    Task<List<UserSecurityAuditDto>> GetUserSecurityAuditAsync(int userId);

    // Metoda vrátí neúspěšné pokusy o přihlášení
    Task<List<FailedLoginDto>> GetFailedLoginsAsync(
        DateTime? fromDate,
        DateTime? toDate);

    // Metoda zneplatní refresh tokeny uživatele
    Task<ProcedureResultDto> RevokeUserRefreshTokensAsync(
        int userId,
        string? revokedByIp);

    // Metoda smaže staré expirované refresh tokeny
    Task<ProcedureResultDto> CleanupExpiredRefreshTokensAsync(int olderThanDays);
}
