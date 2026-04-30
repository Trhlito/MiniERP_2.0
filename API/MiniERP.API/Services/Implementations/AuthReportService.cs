using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Auth.Reports;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;

namespace MiniERP.API.Services.Implementations;

// Služba zajišťuje auth reporty přes SQL procedury
public class AuthReportService : IAuthReportService
{
    // Databázový kontext aplikace
    private readonly ApplicationDbContext _dbContext;

    // Konstruktor služby
    public AuthReportService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Metoda vrátí souhrn auth audit událostí
    public async Task<List<AuthAuditSummaryDto>> GetAuthAuditSummaryAsync(
        DateTime? fromDate,
        DateTime? toDate)
    {
        // Parametr pro začátek období
        var fromDateParameter = new SqlParameter(
            "@FromDate",
            fromDate ?? (object)DBNull.Value);

        // Parametr pro konec období
        var toDateParameter = new SqlParameter(
            "@ToDate",
            toDate ?? (object)DBNull.Value);

        // Spuštění uložené procedury
        var result = await _dbContext.Database
            .SqlQueryRaw<AuthAuditSummaryDto>(
                "EXEC dbo.sp_GetAuthAuditSummary @FromDate, @ToDate",
                fromDateParameter,
                toDateParameter)
            .ToListAsync();

        // Vrácení výsledku procedury
        return result;
    }

    // Metoda vrátí bezpečnostní audit konkrétního uživatele
    public async Task<List<UserSecurityAuditDto>> GetUserSecurityAuditAsync(int userId)
    {
        // Parametr pro id uživatele
        var userIdParameter = new SqlParameter("@UserId", userId);

        // Spuštění uložené procedury
        var result = await _dbContext.Database
            .SqlQueryRaw<UserSecurityAuditDto>(
                "EXEC dbo.sp_GetUserSecurityAudit @UserId",
                userIdParameter)
            .ToListAsync();

        // Vrácení výsledku procedury
        return result;
    }

    // Metoda vrátí neúspěšné pokusy o přihlášení
    public async Task<List<FailedLoginDto>> GetFailedLoginsAsync(
        DateTime? fromDate,
        DateTime? toDate)
    {
        // Parametr pro začátek období
        var fromDateParameter = new SqlParameter(
            "@FromDate",
            fromDate ?? (object)DBNull.Value);

        // Parametr pro konec období
        var toDateParameter = new SqlParameter(
            "@ToDate",
            toDate ?? (object)DBNull.Value);

        // Spuštění uložené procedury
        var result = await _dbContext.Database
            .SqlQueryRaw<FailedLoginDto>(
                "EXEC dbo.sp_GetFailedLogins @FromDate, @ToDate",
                fromDateParameter,
                toDateParameter)
            .ToListAsync();

        // Vrácení výsledku procedury
        return result;
    }

    // Metoda zneplatní refresh tokeny uživatele
    public async Task<ProcedureResultDto> RevokeUserRefreshTokensAsync(
        int userId,
        string? revokedByIp)
    {
        // Parametr pro id uživatele
        var userIdParameter = new SqlParameter("@UserId", userId);

        // Parametr pro IP adresu
        var revokedByIpParameter = new SqlParameter(
            "@RevokedByIp",
            revokedByIp ?? (object)DBNull.Value);

        // Spuštění uložené procedury
        var result = await _dbContext.Database
            .SqlQueryRaw<ProcedureResultDto>(
                "EXEC dbo.sp_RevokeUserRefreshTokens @UserId, @RevokedByIp",
                userIdParameter,
                revokedByIpParameter)
            .ToListAsync();

        // Vrácení výsledku procedury
        return result.FirstOrDefault() ?? new ProcedureResultDto();
    }

    // Metoda smaže staré expirované refresh tokeny
    public async Task<ProcedureResultDto> CleanupExpiredRefreshTokensAsync(int olderThanDays)
    {
        // Parametr pro stáří tokenů
        var olderThanDaysParameter = new SqlParameter(
            "@OlderThanDays",
            olderThanDays);

        // Spuštění uložené procedury
        var result = await _dbContext.Database
            .SqlQueryRaw<ProcedureResultDto>(
                "EXEC dbo.sp_CleanupExpiredRefreshTokens @OlderThanDays",
                olderThanDaysParameter)
            .ToListAsync();

        // Vrácení výsledku procedury
        return result.FirstOrDefault() ?? new ProcedureResultDto();
    }   
}