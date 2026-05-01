using MiniERP.API.DTOs.Auth.Reports;

namespace MiniERP.API.Services.Interfaces;

// Rozhraní na bezpečnostní reporty a administrativní operace nad autentizací
public interface ISecurityReportService
{
    // Vrací souhrn autentizačních událostí za zadané období
    Task<List<AuthAuditSummaryDto>> GetAuthAuditSummaryAsync(
        DateTime? fromDate,
        DateTime? toDate);
    
    // Vrací bezpečnostní historii konkrétního uživatele
    Task<List<UserSecurityAuditDto>> GetUserSecurityAuditAsync(int userId);

    // Vrací neúspěšné pokusy o přihlášení za zadané období
    Task<List<FailedLoginDto>> GetFailedLoginsAsync(
        DateTime? fromDate,
        DateTime? toDate);

    // Zneplatní aktivní refresh tokeny konkrétního uživatele
    Task<ProcedureResultDto> RevokeUserRefreshTokensAsync(
        int userId,
        string? revokedByIp);

    // Provede údržbu a odstraní expirované refresh tokeny
    Task<ProcedureResultDto> CleanupExpiredRefreshTokensAsync(int olderThanDays);
}