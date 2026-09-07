namespace LeaveManagement.API.DTOs;

public record CreateLeaveRequestDto(int LeaveTypeId, DateOnly StartDate, DateOnly EndDate, string? Reason);

public record UpdateLeaveStatusDto(string Status, string? Comment);

public record LeaveRequestResponseDto(
    int Id,
    string EmployeeName,
    int LeaveTypeId,
    string LeaveTypeName,
    DateOnly StartDate,
    DateOnly EndDate,
    string? Reason,
    string Status
);

public record LeaveBalanceDto(
    string LeaveTypeName,
    int TotalDays,
    int UsedDays,
    int RemainingDays
);