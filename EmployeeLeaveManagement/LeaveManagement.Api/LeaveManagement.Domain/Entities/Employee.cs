namespace LeaveManagement.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    
    public string? Department { get; set; }
    public int? UserId { get; set; } // Changed from string? to int?

    // Navigation property
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}