using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.Services
{
    public interface ILeaveTypesService
    {
        Task CreateAsync(LeaveTypeCreateVM model);
        Task EditAsync(LeaveTypeEditVM model);
        Task<T?> GetAsync<T>(int id) where T : class;
        Task<List<LeaveTypeReadOnlyVM>> GetAllAsync();
        Task RemoveAsync(int id);
    }
}