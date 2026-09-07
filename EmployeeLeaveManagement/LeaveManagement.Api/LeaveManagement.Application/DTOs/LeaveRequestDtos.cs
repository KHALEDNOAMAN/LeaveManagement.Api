// namespace LeaveManagement.Application.DTOs;

// public class CreateLeaveRequestDto
// {
//     public int LeaveTypeId { get; set; }
//     public DateTime StartDate { get; set; }
//     public DateTime EndDate { get; set; }
//     public string? Reason { get; set; }
// }

// public class LeaveRequestResponseDto
// {
//     public int Id { get; set; }
//     public string LeaveTypeName { get; set; } = string.Empty;
//     public DateTime StartDate { get; set; }
//     public DateTime EndDate { get; set; }
//     public string Status { get; set; } = string.Empty;
//     public string? Reason { get; set; }
// }
namespace LeaveManagement.Application.DTOs;

public record CreateLeaveRequestDto(
    int LeaveTypeId,
    DateTime StartDate,
    DateTime EndDate,
    string? Reason
);

public record UpdateLeaveStatusDto(
    string Status // "Approved" or "Rejected"
);

public record LeaveRequestResponseDto(
    int Id,
    int EmployeeId,
    string EmployeeName,
    int LeaveTypeId,
    string LeaveTypeName,
    DateTime StartDate,
    DateTime EndDate,
    string? Reason,
    string Status
);