
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicatiionUser> _userManager, IMapper _mapper) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            //get all the unallocated leave types (?? how does sick work? also, how do we allocate multiples holidays?
            //var leaveTypes = await _context.LeaveTypes.ToListAsync();
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();
            //get the current period based on the year
            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var monthsRemaining = period.EndDate.Month - currentDate.Month;
            //calculate leave based on the number of months left in the period
            //foreach leave type, create an alllocation entry
            foreach (var leaveType in leaveTypes)
            {
                var allocationExists = await AllocationExists(employeeId, period.Id, leaveType.Id);
                //works, but less efficiently than the "var leaveTypes..." lambda expression (see above)
                //if (allocationExists)
                //    continue;
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

        public async Task<List<LeaveAllocation>> GetAllocatiionsAsync(string? employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                employeeId = _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
            }

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
                .Where(q => q.EmployeeId == employeeId && q.Period.EndDate.Year == currentDate.Year)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocationsAsync(string? employeeId)
        {
            ApplicatiionUser? user = string.IsNullOrEmpty(employeeId)
                ? await GetUserAsync()
                : await _userManager.FindByIdAsync(employeeId);

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
            var users = await GetUsersAsync(Roles.Employee);

            return _mapper.Map<List<EmployeeListVM>>(users);
        }

        //----------------------------------------------------------------------

        //works, but less efficiently than the "var leaveTypes..." lambda expression (see above)
        private async Task<bool> AllocationExists(string employeeId, int periodId, int leaveTypeId)
        {
            var exists = await _context.LeaveAllocations.AnyAsync(q =>
                q.EmployeeId == employeeId &&
                q.LeaveTypeId == leaveTypeId &&
                q.PeriodId == periodId);

            return exists;
        }

        private async Task<ApplicatiionUser> GetUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        }

        private async Task<IList<ApplicatiionUser>> GetUsersAsync(string Role)
        {
            return await _userManager.GetUsersInRoleAsync(Role);
        }
    }
}
