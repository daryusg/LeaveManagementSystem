using static LeaveManagementSystem.Application.Services.LeaveRequests.LeaveRequestsService;

namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequestAsync(LeaveRequestCreateVM model);
        Task<EmployeeLeaveRequestListVM> GetAllLeaveRequestsAsync();
        Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequestsAsync();
        Task CancelLeaveRequestAsync(int leaveRequestId);
        Task ReviewLeaveRequestAsync(int leaveRequestId, bool approved);
        Task<ret_bool_int> RequestDatesExceedAllocation(LeaveRequestCreateVM model);
        Task<ReviewLeaveRequestVM> GetLeaveRequestForReviewAsync(int leaveRequestId);
    }
}