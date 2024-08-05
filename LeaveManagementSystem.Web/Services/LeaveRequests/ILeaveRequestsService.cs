using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequestAsync(LeaveRequestVM model);
        Task<EmployeeLeaveRequestListVM> EmployeeLeaveRequestAsync(string employeeId);
        Task<LeaveRequestListVM> GetAllLeaveRequestsAsync();

        Task CancelLeaveRequestAsync(int leaveRequestId);
        Task ReviewLeaveRequestAsync(ReviewLeaveRequestVM model);
    }
}