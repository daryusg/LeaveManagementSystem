
namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string employeeId);
        Task<List<LeaveAllocation>> GetAllocatiionsAsync();
        Task<EmployeeAllocationVM> GetEmployeeAllocationsAsync();
    }
}
