using LeaveManagement.Domain.Entities;
using LeaveManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    // Ensure the parameter uses LeaveManagement.Infrastructure.Data.ApplicationDbContext
    public static async Task SeedAsync(LeaveManagement.Infrastructure.Data.ApplicationDbContext context)
    {
        if (!await context.LeaveTypes.AnyAsync())
        {
            await context.LeaveTypes.AddRangeAsync(new List<LeaveType>
            {
                new LeaveType { Name = "Annual Leave", DefaultDays = 20 },
                new LeaveType { Name = "Sick Leave", DefaultDays = 10 },
                new LeaveType { Name = "Casual Leave", DefaultDays = 5 }
            });
            await context.SaveChangesAsync();
        }

        if (!await context.Employees.AnyAsync())
        {
            await context.Employees.AddAsync(new Employee
            {
                FirstName = "Barie",
                LastName = "Wakjira",
                Email = "barie@example.com",
                Department = "Software Engineering",
                UserId = 1
            });
            await context.SaveChangesAsync();
        }
    }
}