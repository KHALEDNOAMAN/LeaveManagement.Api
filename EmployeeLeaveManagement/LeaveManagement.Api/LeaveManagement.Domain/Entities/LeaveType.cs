namespace LeaveManagement.Domain.Entities;

public class LeaveType
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int DefaultDays { get; set; } = 20;
}