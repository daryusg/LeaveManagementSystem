﻿
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicatiionUser> _userManager, IMapper _mapper) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            //get all the leave types
            var leaveTypes = await _context.LeaveTypes.ToListAsync();
            //get the current period based on the year
            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var monthsRemaining = period.EndDate.Month - currentDate.Month;
            //calculate leave based on the number of months left in the period
            //foreach leave type, create an alllocation entry
            foreach (var leaveType in leaveTypes)
            {
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    Days =
                    Convert.ToInt32(leaveType.NumberOfDays * ((decimal)monthsRemaining / 12))
                };
                _context.Add(leaveAllocation);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<LeaveAllocation>> GetAllocatiionsAsync()
        {
            var user = await GetUserAsync();
            var currentDate = DateTime.Now;
            //var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            //var leaveAllocations = await _context.LeaveAllocations
            //    .Include(q => q.LeaveType)
            //    .Include(q => q.Period)
            //    .Where(q => q.EmployeeId == user.Id && q.PeriodId == period.Id)
            //    .ToListAsync();
            var leaveAllocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == user.Id && q.Period.EndDate.Year == currentDate.Year)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocationsAsync()
        {
            var allocations = await GetAllocatiionsAsync();
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);

            var user = await GetUserAsync();
            var employeeVm = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList
            };

            return employeeVm;
        }

        private async Task<ApplicatiionUser?> GetUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        }
    }
}
