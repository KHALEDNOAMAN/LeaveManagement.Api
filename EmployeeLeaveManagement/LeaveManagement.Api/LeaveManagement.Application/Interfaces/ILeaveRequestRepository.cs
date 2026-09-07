using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Interfaces;

public interface ILeaveRequestRepository
{
    Task<LeaveRequest?> GetByIdAsync(int id);
    Task<IEnumerable<LeaveRequest>> GetAllAsync();
    Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId);
    Task AddAsync(LeaveRequest request);
    Task UpdateAsync(LeaveRequest request);
}