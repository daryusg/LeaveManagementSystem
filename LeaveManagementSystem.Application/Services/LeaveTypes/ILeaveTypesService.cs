﻿namespace LeaveManagementSystem.Application.Services.LeaveTypes
{
    public interface ILeaveTypesService
    {
        Task CreateAsync(LeaveTypeCreateVM model);
        Task EditAsync(LeaveTypeEditVM model);
        Task<T?> GetAsync<T>(int id) where T : class;
        Task<List<LeaveTypeReadOnlyVM>> GetAllAsync();
        Task RemoveAsync(int id);
        Task<bool> CheckIfLeaveTypeNameExistsAsync(string name);
        Task<bool> CheckIfLeaveTypeNameExistsForEditAsync(LeaveTypeEditVM leaveTypeEdit);
        Task<bool> LeaveTypeExistsAsync(int id);
        Task<bool> DaysExceedMaximumAsync(int leaveTypeID, int days);
    }
}