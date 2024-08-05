using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public class LeaveRequestsService : ILeaveRequestsService //141. dont't forget. once i have the service and the interface, register it (in Program.cs).
    {
        public Task CancelLeaveRequestAsync(int leaveRequestId)
        {
            throw new NotImplementedException();
        }

        public Task CreateLeaveRequestAsync(LeaveRequestVM model)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeLeaveRequestListVM> EmployeeLeaveRequestAsync(string employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveRequestListVM> GetAllLeaveRequestsAsync()
        {
            throw new NotImplementedException();
        }

        public Task ReviewLeaveRequestAsync(ReviewLeaveRequestVM model)
        {
            throw new NotImplementedException();
        }
    }
}
