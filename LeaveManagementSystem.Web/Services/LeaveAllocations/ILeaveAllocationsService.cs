
namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string employeeId);
        Task<List<LeaveAllocation>> GetAllocatiionsAsync(string? employeeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocationsAsync(string? employeeId);
        Task<List<EmployeeListVM>> GetEmployeesAsync();
    }
}
