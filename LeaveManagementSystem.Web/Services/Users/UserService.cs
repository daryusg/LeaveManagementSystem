namespace LeaveManagementSystem.Web.Services.Users
{
    public class UserService(UserManager<ApplicatiionUser> _userManager, IHttpContextAccessor _httpContextAccessor) : IUserService
    {
        public async Task<ApplicatiionUser> GetLoggedInUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        }
        public async Task<ApplicatiionUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<IList<ApplicatiionUser>> GetUsersAsync(string Role)
        {
            return await _userManager.GetUsersInRoleAsync(Role);
        }

    }
}
