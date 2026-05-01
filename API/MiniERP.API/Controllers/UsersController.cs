using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP.API.DTOs.Users;
using MiniERP.API.Services.Interfaces;

namespace MiniERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // Endpoint vrátí seznam uživatelů
    [HttpGet]
    public async Task<ActionResult<List<UserListItemDto>>> GetAll()
    {
        return Ok(await _userService.GetAllAsync());
    }

    // Endpoint vrátí detail uživatele
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDetailDto>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    // Endpoint vytvoří nového uživatele
    [HttpPost]
    public async Task<ActionResult<UserDetailDto>> Create(CreateUserRequest request)
    {
        try
        {
            var user = await _userService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Endpoint upraví uživatele
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateUserRequest request)
    {
        var updated = await _userService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    // Endpoint upraví role uživatele
    [HttpPut("{id:int}/roles")]
    public async Task<IActionResult> UpdateRoles(int id, UpdateUserRolesRequest request)
    {
        try
        {
            var updated = await _userService.UpdateRolesAsync(id, request);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Endpoint nastaví nové heslo uživatele
    [HttpPost("{id:int}/reset-password")]
    public async Task<IActionResult> ResetPassword(int id, ResetPasswordRequest request)
    {
        var updated = await _userService.ResetPasswordAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}