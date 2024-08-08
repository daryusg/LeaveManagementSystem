

namespace LeaveManagementSystem.Web.Services.Users
{
    public interface IUserService
    {
        Task<ApplicatiionUser> GetLoggedInUserAsync();
        Task<ApplicatiionUser> GetUserByIdAsync(string userId);
        Task<IList<ApplicatiionUser>> GetUsersAsync(string Role);
    }
}