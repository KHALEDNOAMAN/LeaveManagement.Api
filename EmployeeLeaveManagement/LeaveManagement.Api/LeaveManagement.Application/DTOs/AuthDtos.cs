namespace LeaveManagement.Application.DTOs;

public record RegisterDto(
    string FirstName, 
    string LastName, 
    string Email, 
    string Password, 
    string Department,
    string Role = "Employee" // Default role
);

public record LoginDto(string Email, string Password);

public record AuthResponseDto(string Token, string Email, string Role, int EmployeeId);