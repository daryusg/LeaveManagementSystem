using LeaveManagementSystem.Web.Models.LeaveRequests;
using static LeaveManagementSystem.Web.Services.LeaveRequests.LeaveRequestsService;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequestAsync(LeaveRequestCreateVM model);
        Task<EmployeeLeaveRequestListVM> GetAllLeaveRequestsAsync();
        Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequestsAsync();
        Task CancelLeaveRequestAsync(int leaveRequestId);
        Task ReviewLeaveRequestAsync(ReviewLeaveRequestVM model);
        Task<ret_bool_int> RequestDatesExceedAllocation(LeaveRequestCreateVM model);
        Task<ReviewLeaveRequestVM> GetLeaveRequestForReviewAsync(int leaveRequestId);
    }
}