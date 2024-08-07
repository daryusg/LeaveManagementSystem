using LeaveManagementSystem.Web.Models.LeaveRequests;
using static LeaveManagementSystem.Web.Services.LeaveRequests.LeaveRequestsService;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequestAsync(LeaveRequestCreateVM model);
        Task<EmployeeLeaveRequestListVM> EmployeeLeaveRequestAsync(string employeeId);
        Task<LeaveRequestReadOnlyVM> GetAllLeaveRequestsAsync();

        Task CancelLeaveRequestAsync(int leaveRequestId);
        Task ReviewLeaveRequestAsync(ReviewLeaveRequestVM model);
        Task<ret_bool_int> RequestDatesExceedAllocation(LeaveRequestCreateVM model);
    }
}