using LeaveManagement.Domain.Entities;         // <-- Resolves LeaveRequest & LeaveType
using LeaveManagement.Infrastructure.Data;     // <-- Resolves ApplicationDbContext
using LeaveManagement.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Controllers;

[ApiController]
[Route("api/leave-requests")]
public class LeaveRequestsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LeaveRequestsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/leave-requests/types
    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<LeaveType>>> GetLeaveTypes()
    {
        return await _context.LeaveTypes.ToListAsync();
    }

    // GET: api/leave-requests
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveRequestResponseDto>>> GetLeaveRequests()
    {
        var requests = await _context.LeaveRequests
            .Include(r => r.LeaveType)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new LeaveRequestResponseDto(
                r.Id,
                r.EmployeeName,
                r.LeaveTypeId,
                r.LeaveType != null ? r.LeaveType.Name : "General",
                r.StartDate,
                r.EndDate,
                r.Reason,
                r.Status
            ))
            .ToListAsync();

        return Ok(requests);
    }

    // GET: api/leave-requests/balances
    [HttpGet("balances")]
    public async Task<ActionResult<IEnumerable<LeaveBalanceDto>>> GetUserBalances()
    {
        var types = await _context.LeaveTypes.ToListAsync();
        var approvedRequests = await _context.LeaveRequests
            .Where(r => r.Status == "Approved")
            .ToListAsync();

        var balances = types.Select(t =>
        {
            var usedDays = approvedRequests
                .Where(r => r.LeaveTypeId == t.Id)
                .Sum(r => (r.EndDate.DayNumber - r.StartDate.DayNumber) + 1);

            return new LeaveBalanceDto(
                t.Name,
                t.DefaultDays,
                usedDays,
                Math.Max(0, t.DefaultDays - usedDays)
            );
        });

        return Ok(balances);
    }

    // POST: api/leave-requests
    [HttpPost]
    public async Task<ActionResult<LeaveRequestResponseDto>> CreateLeaveRequest(CreateLeaveRequestDto dto)
    {
        if (dto.EndDate < dto.StartDate)
        {
            return BadRequest(new { message = "End date cannot be earlier than start date." });
        }

        var leaveType = await _context.LeaveTypes.FindAsync(dto.LeaveTypeId);
        if (leaveType == null)
        {
            return NotFound(new { message = "Selected leave type does not exist." });
        }

        var request = new LeaveRequest
        {
            EmployeeId = 1, // Using integer EmployeeId matching Domain model
            EmployeeName = "Barie Wakjira",
            LeaveTypeId = dto.LeaveTypeId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Reason = dto.Reason,
            Status = "Pending"
        };

        _context.LeaveRequests.Add(request);
        await _context.SaveChangesAsync();

        var response = new LeaveRequestResponseDto(
            request.Id,
            request.EmployeeName,
            request.LeaveTypeId,
            leaveType.Name,
            request.StartDate,
            request.EndDate,
            request.Reason,
            request.Status
        );

        return CreatedAtAction(nameof(GetLeaveRequests), new { id = request.Id }, response);
    }

    // PATCH: api/leave-requests/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateLeaveStatusDto dto)
    {
        var request = await _context.LeaveRequests.FindAsync(id);
        if (request == null)
        {
            return NotFound(new { message = "Leave request not found." });
        }

        if (dto.Status != "Approved" && dto.Status != "Rejected")
        {
            return BadRequest(new { message = "Invalid status. Must be 'Approved' or 'Rejected'." });
        }

        request.Status = dto.Status;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/leave-requests/pending-count
    [HttpGet("pending-count")]
    public async Task<ActionResult<object>> GetPendingCount()
    {
        var count = await _context.LeaveRequests.CountAsync(r => r.Status == "Pending");
        return Ok(new { count });
    }
}