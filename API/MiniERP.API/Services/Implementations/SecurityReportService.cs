using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniERP.API.DTOs.Auth.Reports;
using MiniERP.API.Services.Interfaces;
using MiniERP.Data;

namespace MiniERP.API.Services.Implementations;

// Služba načítá bezpečnostní reporty z SQL procedur
public class SecurityReportService : ISecurityReportService
{
    private readonly ApplicationDbContext _dbContext;

    public SecurityReportService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Souhrn autentizačních událostí za zadané období
    public async Task<List<AuthAuditSummaryDto>> GetAuthAuditSummaryAsync(
        DateTime? fromDate,
        DateTime? toDate)
    {
        var fromDateParameter = new SqlParameter(
            "@FromDate",
            fromDate ?? (object)DBNull.Value);

        var toDateParameter = new SqlParameter(
            "@ToDate",
            toDate ?? (object)DBNull.Value);

        return await _dbContext.Database
            .SqlQueryRaw<AuthAuditSummaryDto>(
                "EXEC dbo.sp_GetAuthAuditSummary @FromDate, @ToDate",
                fromDateParameter,
                toDateParameter)
            .ToListAsync();
    }

    // Bezpečnostní historie konkrétního uživatele
    public async Task<List<UserSecurityAuditDto>> GetUserSecurityAuditAsync(int userId)
    {
        var userIdParameter = new SqlParameter("@UserId", userId);

        return await _dbContext.Database
            .SqlQueryRaw<UserSecurityAuditDto>(
                "EXEC dbo.sp_GetUserSecurityAudit @UserId",
                userIdParameter)
            .ToListAsync();
    }

    // Vrací neúspěšné pokusy o přihlášení za zadané období
    public async Task<List<FailedLoginDto>> GetFailedLoginsAsync(
        DateTime? fromDate,
        DateTime? toDate)
    {
        var fromDateParameter = new SqlParameter(
            "@FromDate",
            fromDate ?? (object)DBNull.Value);

        var toDateParameter = new SqlParameter(
            "@ToDate",
            toDate ?? (object)DBNull.Value);

        return await _dbContext.Database
            .SqlQueryRaw<FailedLoginDto>(
                "EXEC dbo.sp_GetFailedLogins @FromDate, @ToDate",
                fromDateParameter,
                toDateParameter)
            .ToListAsync();
    }

    // Zneplatnění aktivních refresh tokenů konkrétního uživatele
    public async Task<ProcedureResultDto> RevokeUserRefreshTokensAsync(
        int userId,
        string? revokedByIp)
    {
        var userIdParameter = new SqlParameter("@UserId", userId);

        var revokedByIpParameter = new SqlParameter(
            "@RevokedByIp",
            revokedByIp ?? (object)DBNull.Value);

        var result = await _dbContext.Database
            .SqlQueryRaw<ProcedureResultDto>(
                "EXEC dbo.sp_RevokeUserRefreshTokens @UserId, @RevokedByIp",
                userIdParameter,
                revokedByIpParameter)
            .ToListAsync();

        return result.FirstOrDefault() ?? new ProcedureResultDto();
    }

    // Údržba a odstranění expirovaných refresh tokenů
    public async Task<ProcedureResultDto> CleanupExpiredRefreshTokensAsync(int olderThanDays)
    {
        var olderThanDaysParameter = new SqlParameter(
            "@OlderThanDays",
            olderThanDays);

        var result = await _dbContext.Database
            .SqlQueryRaw<ProcedureResultDto>(
                "EXEC dbo.sp_CleanupExpiredRefreshTokens @OlderThanDays",
                olderThanDaysParameter)
            .ToListAsync();

        return result.FirstOrDefault() ?? new ProcedureResultDto();
    }
}