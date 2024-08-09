using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
    public class LeaveRequestsService(ApplicationDbContext _context, IMapper _mapper, IUserService _userService, IPeriodsService _periodsService, ILeaveAllocationsService _leaveAllocationsService) : ILeaveRequestsService //141. dont't forget. once i have the service and the interface, register it (in Program.cs).
    {
        public async Task CreateLeaveRequestAsync(LeaveRequestCreateVM model)
        {
            //map data to leave request data model
            var leaveRequest = _mapper.Map<LeaveRequest>(model);
            //get logged in employee id
            //var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            leaveRequest.EmployeeId = (await _userService.GetLoggedInUserAsync()).Id;

            //set LeaveRequestStatusId to pending
            leaveRequest.LeaveRequestStatusId = LeaveRequestStatus_IdFromName("Pending");

            //save leave request
            _context.Add(leaveRequest); //save #1

            //deduct allocation days based on request
            await UpdateAllocationDays(leaveRequest, true);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeLeaveRequestListVM> GetAllLeaveRequestsAsync()
        {
            var leaveRequests = await _context.LeaveRequest
                .Include(q => q.LeaveType)
                .ToListAsync();

            var model = new EmployeeLeaveRequestListVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved),
                PendingRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending),
                DeclinedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Declined),
                LeaveRequests = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
                {
                    StartDate = q.StartDate,
                    EndDate = q.EndDate,
                    Id = q.Id,
                    LeaveType = q.LeaveType.Name,
                    LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                    NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber + 1
                }).ToList()
            };

            return model;
        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequestsAsync()
        {
            //var userid = _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
            var userid = (await _userService.GetLoggedInUserAsync()).Id;
            var leaveRequests = await _context.LeaveRequest
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == userid)
                .ToListAsync();
            //surely I can inject the mapper service to do the following:
            var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber + 1
            }).ToList();

            return model;
        }

        public async Task CancelLeaveRequestAsync(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequest.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled;

            //restore allocation days based on request
            await UpdateAllocationDays(leaveRequest, false);
            await _context.SaveChangesAsync();
        }

        public async Task<ret_bool_int> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            //get the current period based on the year
            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);

            //var userid = _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
            var userid = (await _userService.GetLoggedInUserAsync()).Id;
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber + 1;
            var allocation = await _context.LeaveAllocations
                .FirstAsync(
                    q => q.LeaveTypeId == model.LeaveTypeId
                    && q.EmployeeId == userid
                    && q.PeriodId == period.Id
                );
            return new ret_bool_int { boolValue = allocation.Days < numberOfDays, intValue = allocation.Days };
        }

        public struct ret_bool_int //mine
        {
            public bool boolValue;
            public int intValue;
        }

        public int LeaveRequestStatus_IdFromName(string name) //mine
        {
            return _context.LeaveRequestStatus.First(x => x.Name == name).Id;
        }
        public string LeaveRequestStatus_NameFromId(int id) //mine
        {
            return _context.LeaveRequestStatus.First(x => x.Id == id).Name;
        }

        public async Task ReviewLeaveRequestAsync(int leaveRequestId, bool approved)
        {
            //get the current period based on the year
            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);

            //var userid = _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
            var userid = (await _userService.GetLoggedInUserAsync()).Id;

            var leaveRequest = await _context.LeaveRequest.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = approved ? (int)LeaveRequestStatusEnum.Approved : (int)LeaveRequestStatusEnum.Declined;

            leaveRequest.ReviewerId = userid;

            if (!approved)
            {
                //restore allocation days based on request
                await UpdateAllocationDays(leaveRequest, false);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReviewAsync(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequest
                .Include(q => q.LeaveType)
                .FirstAsync(q => q.Id == leaveRequestId);

            //var user = await _userManager.FindByIdAsync(leaveRequest.EmployeeId);
            var user = await _userService.GetUserByIdAsync(leaveRequest.EmployeeId);

            var model = new ReviewLeaveRequestVM()
            {
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber + 1,
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
                Id = leaveRequest.Id,
                LeaveType = leaveRequest.LeaveType.Name,
                RequestComments = leaveRequest.RequestComments,
                Employee = new EmployeeListVM
                {
                    Id = leaveRequest.EmployeeId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                }
            };

            return model;
        }

        //----------------------------------------------------------------------

        private async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
            var allocation = await _leaveAllocationsService.GetCurrentAllocationAsync(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);
            var numberOfDays = leaveRequest.StartDate.DayNumber - leaveRequest.EndDate.DayNumber + 1;

            if (deductDays)
                allocation.Days -= numberOfDays;
            else
                allocation.Days += numberOfDays;

            _context.Entry(allocation).State = EntityState.Modified; //unneccessary??
        }
    }
}
