
using AutoMapper;
using LeaveManagementSystem.Web.Services.Periods;
using LeaveManagementSystem.Web.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, IUserService _userService, IMapper _mapper, IPeriodsService _periodsService) : ILeaveAllocationsService
    {
        public async Task AllocateLeaveAsync(string employeeId)
        {
            //get all the unallocated leave types (?? how does sick work? also, how do we allocate multiples holidays?
            //var leaveTypes = await _context.LeaveTypes.ToListAsync();
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();
            //get the current period based on the year
            var currentDate = DateTime.Now;
            var period = await _periodsService.GetCurrentPeriodAsync();
            var monthsRemaining = period.EndDate.Month - currentDate.Month;
            //calculate leave based on the number of months left in the period
            //foreach leave type, create an alllocation entry
            foreach (var leaveType in leaveTypes)
            {
                var allocationExists = await AllocationExistsAsync(employeeId, period.Id, leaveType.Id);
                //works, but less efficiently than the "var leaveTypes..." lambda expression (see above)
                //if (allocationExists)
                //    continue;
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    Days = Convert.ToInt32(leaveType.NumberOfDays * ((decimal)monthsRemaining / 12))
                };
                _context.Add(leaveAllocation);
            }
            await _context.SaveChangesAsync();
        }

        public async Task EditAllocationAsync(LeaveAllocationEditVM allocationEditVM)
        {
            //var leaveAllocation = await GetEmployeeAllocationAsync(allocationEditVM.Id) ?? throw new Exception("Leave Allocation record does not exist.");
            //if (leaveAllocation == null)
            //{
            //    throw new Exception("Leave Allocation record does not exist.");
            //}
            //leaveAllocation.NumberOfDays = allocationEditVM.NumberOfDays;
            //option 1: _context.Update(leaveAllocation);
            //option 2a: _context.Entry(leaveAllocation).State =  EntityState.Modified;
            //option 2b: await _context.SaveChangesAsync();
            await _context.LeaveAllocations
                .Where(q => q.Id == allocationEditVM.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.Days, allocationEditVM.NumberOfDays));
        }

        public async Task<LeaveAllocation> GetCurrentAllocationAsync(int leaveTypeId, string employeeId)
        {
            var period = await _periodsService.GetCurrentPeriodAsync();
            var allocation = await _context.LeaveAllocations
                .FirstAsync(
                    q => q.LeaveTypeId == leaveTypeId
                    && q.EmployeeId == employeeId
                    && q.PeriodId == period.Id
                );
            return allocation;
        }

        public async Task<List<LeaveAllocation>> GetAllocatiionsAsync(string? employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                //get the logged in (admin) user's id
                employeeId = (await _userService.GetLoggedInUserAsync()).Id;//_userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
            }

            var currentDate = DateTime.Now;
            var period = await _periodsService.GetCurrentPeriodAsync();
            //var leaveAllocations = await _context.LeaveAllocations
            //    .Include(q => q.LeaveType)
            //    .Include(q => q.Period)
            //    .Where(q => q.EmployeeId == user.Id && q.PeriodId == period.Id)
            //    .ToListAsync();
            var leaveAllocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Period)
                //.Where(q => q.EmployeeId == employeeId && q.Period.EndDate.Year == currentDate.Year)
                .Where(q => q.EmployeeId == employeeId && q.Period == period)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<LeaveAllocationVM> GetEmployeeAllocationAsync(int allocationId)
        {
            var allocation = _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefault(q => q.Id == allocationId);

            var model = _mapper.Map<LeaveAllocationEditVM>(allocation);
            return model;
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocationsAsync(string? employeeId)
        {
            ApplicatiionUser? user = string.IsNullOrEmpty(employeeId)
                ? await _userService.GetLoggedInUserAsync()
                : await _userService.GetUserByIdAsync(employeeId);

            var allocations = await GetAllocatiionsAsync(employeeId);
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
            var leaveTypesCount = await _context.LeaveTypes.CountAsync();

            var employeeVm = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList,
                IsCompletedAllocation = leaveTypesCount == allocations.Count()
            };

            return employeeVm;
        }

        public async Task<List<EmployeeListVM>> GetEmployeesAsync()
        {
            var users = await _userService.GetUsersAsync(Roles.Employee);

            return _mapper.Map<List<EmployeeListVM>>(users);
        }

        //----------------------------------------------------------------------

        private async Task<bool> AllocationExistsAsync(string employeeId, int periodId, int leaveTypeId)
        {
            var exists = await _context.LeaveAllocations.AnyAsync(q =>
                q.EmployeeId == employeeId &&
                q.LeaveTypeId == leaveTypeId &&
                q.PeriodId == periodId);

            return exists;
        }

        /*
        private async Task<ApplicatiionUser> GetUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        }
        */
    }
}
