
namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeaveAsync(string employeeId);
        Task EditAllocationAsync(LeaveAllocationEditVM allocationEditVM);
        Task<List<LeaveAllocation>> GetAllocatiionsAsync(string? employeeId);
        Task<LeaveAllocation> GetCurrentAllocationAsync(int leaveTypeId, string employeeId);
        Task<LeaveAllocationVM> GetEmployeeAllocationAsync(int allocationId); //133
        Task<EmployeeAllocationVM> GetEmployeeAllocationsAsync(string? employeeId);
        Task<List<EmployeeListVM>> GetEmployeesAsync();
    }
}
