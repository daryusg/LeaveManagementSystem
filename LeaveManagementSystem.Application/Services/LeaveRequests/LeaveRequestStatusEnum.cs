namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
    public enum LeaveRequestStatusEnum //based on/mirrors the dbo.LeaveRequestStatus table
    {
        Pending = 1,
        Approved = 2,
        Declined = 3,
        Cancelled = 4
    }
}
