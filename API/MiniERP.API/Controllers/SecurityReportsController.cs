using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Auth.Reports;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

[ApiController]
[Route("api/security-reports")]
[Authorize(Roles = "Admin")]
public class SecurityReportsController : ControllerBase
{
    private readonly ISecurityReportService _securityReportService;

    public SecurityReportsController(ISecurityReportService securityReportService)
    {
        _securityReportService = securityReportService;
    }

    // Souhrn autentizačních událostí za zadané období
    [HttpGet("audit-summary")]
    public async Task<ActionResult<List<AuthAuditSummaryDto>>> GetAuditSummary(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var result = await _securityReportService.GetAuthAuditSummaryAsync(
            fromDate,
            toDate);

        return Ok(result);
    }

    // Bezpečnostní audit konkrétního uživatele
    [HttpGet("users/{id:int}/security-audit")]
    public async Task<ActionResult<List<UserSecurityAuditDto>>> GetUserSecurityAudit(int id)
    {
        var result = await _securityReportService.GetUserSecurityAuditAsync(id);

        return Ok(result);
    }

    // Přehled neúspěšných pokusů o přihlášení
    [HttpGet("failed-logins")]
    public async Task<ActionResult<List<FailedLoginDto>>> GetFailedLogins(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var result = await _securityReportService.GetFailedLoginsAsync(
            fromDate,
            toDate);

        return Ok(result);
    }

    // Zneplatnění aktivních refresh tokenů konkrétního uživatele
    [HttpPost("users/{id:int}/revoke-refresh-tokens")]
    public async Task<ActionResult<ProcedureResultDto>> RevokeUserRefreshTokens(int id)
    {
        var result = await _securityReportService.RevokeUserRefreshTokensAsync(
            id,
            HttpContext.Connection.RemoteIpAddress?.ToString());

        return Ok(result);
    }

    // Údržba expirovaných a zneplatněných refresh tokenů
    [HttpPost("cleanup-expired-refresh-tokens")]
    public async Task<ActionResult<ProcedureResultDto>> CleanupExpiredRefreshTokens(
        [FromQuery] int olderThanDays = 30)
    {
        var result = await _securityReportService.CleanupExpiredRefreshTokensAsync(
            olderThanDays);

        return Ok(result);
    }
}