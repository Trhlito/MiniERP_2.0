using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Auth.Reports;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

// Controller zajišťuje auth reporty pro administrátora
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AuthReportsController : ControllerBase
{
    // Služba pro auth reporty
    private readonly IAuthReportService _authReportService;

    // Konstruktor controlleru
    public AuthReportsController(IAuthReportService authReportService)
    {
        _authReportService = authReportService;
    }

    // Endpoint vrátí souhrn auth audit událostí
    [HttpGet("audit-summary")]
    public async Task<ActionResult<List<AuthAuditSummaryDto>>> GetAuditSummary(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        // Načtení souhrnu auth auditu
        var result = await _authReportService.GetAuthAuditSummaryAsync(
            fromDate,
            toDate);

        // Vrácení výsledku
        return Ok(result);
    }

    // Endpoint vrátí bezpečnostní audit konkrétního uživatele
    [HttpGet("users/{id:int}/security-audit")]
    public async Task<ActionResult<List<UserSecurityAuditDto>>> GetUserSecurityAudit(int id)
    {
        // Načtení bezpečnostního auditu uživatele
        var result = await _authReportService.GetUserSecurityAuditAsync(id);

        // Vrácení výsledku
        return Ok(result);
    }

    // Endpoint vrátí neúspěšné pokusy o přihlášení
    [HttpGet("failed-logins")]
    public async Task<ActionResult<List<FailedLoginDto>>> GetFailedLogins(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        // Načtení neúspěšných přihlášení
        var result = await _authReportService.GetFailedLoginsAsync(
            fromDate,
            toDate);

        // Vrácení výsledku
        return Ok(result);
    }

    // Endpoint zneplatní refresh tokeny uživatele
    [HttpPost("users/{id:int}/revoke-refresh-tokens")]
    public async Task<ActionResult<ProcedureResultDto>> RevokeUserRefreshTokens(int id)
    {
        // Zneplatnění refresh tokenů uživatele
        var result = await _authReportService.RevokeUserRefreshTokensAsync(
            id,
            HttpContext.Connection.RemoteIpAddress?.ToString());

        // Vrácení výsledku
        return Ok(result);
    }

    // Endpoint smaže staré expirované refresh tokeny
    [HttpPost("cleanup-expired-refresh-tokens")]
    public async Task<ActionResult<ProcedureResultDto>> CleanupExpiredRefreshTokens(
        [FromQuery] int olderThanDays = 30)
    {
        // Smazání starých expirovaných refresh tokenů
        var result = await _authReportService.CleanupExpiredRefreshTokensAsync(
            olderThanDays);

        // Vrácení výsledku
        return Ok(result);
    }
}