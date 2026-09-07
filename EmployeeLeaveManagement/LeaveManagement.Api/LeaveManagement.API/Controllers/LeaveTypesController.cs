using LeaveManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Controllers;

[Authorize] // Requires valid JWT for all endpoints in this controller
[ApiController]
[Route("api/[controller]")]
public class LeaveTypesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LeaveTypesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetLeaveTypes()
    {
        var leaveTypes = await _context.LeaveTypes.ToListAsync();
        return Ok(leaveTypes);
    }

    [Authorize(Roles = "Admin")] // Requires Admin role specifically
    [HttpPost]
    public async Task<IActionResult> CreateLeaveType([FromBody] LeaveManagement.Domain.Entities.LeaveType leaveType)
    {
        _context.LeaveTypes.Add(leaveType);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLeaveTypes), new { id = leaveType.Id }, leaveType);
    }
}