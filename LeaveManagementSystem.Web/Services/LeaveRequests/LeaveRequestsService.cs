using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public class LeaveRequestsService(ApplicationDbContext _context, IMapper _mapper, UserManager<ApplicatiionUser> _userManager, IHttpContextAccessor _httpContextAccessor) : ILeaveRequestsService //141. dont't forget. once i have the service and the interface, register it (in Program.cs).
    {
        public async Task CreateLeaveRequestAsync(LeaveRequestCreateVM model)
        {
            //map data to leave request data model
            var leaveRequest = _mapper.Map<LeaveRequest>(model);
            //get logged in employee id
            //var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var userid = _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
            leaveRequest.EmployeeId = userid;

            //set LeaveRequestStatusId to pending
            leaveRequest.LeaveRequestStatusId = LeaveRequestStatus_IDFromName("Pending");

            //save leave request
            _context.Add(leaveRequest); //save #1

            //deduct allocation days based on request
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber + 1;
            var allocationToDeduct = await _context.LeaveAllocations
                .FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == userid);

            allocationToDeduct.Days -= numberOfDays; //save #2

            await _context.SaveChangesAsync();
        }

        public Task<EmployeeLeaveRequestListVM> GetAllLeaveRequestsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequestsAsync()
        {
            var userid = _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
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

            return (model);
        }

        public async Task CancelLeaveRequestAsync(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequest.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled;

            //restore allocation days based on request
            var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber + 1;
            var allocationToRestoreDaysTo = await _context.LeaveAllocations
                .FirstAsync(q => q.LeaveTypeId == leaveRequest.LeaveTypeId && q.EmployeeId != leaveRequest.EmployeeId);
            allocationToRestoreDaysTo.Days += numberOfDays;

            await _context.SaveChangesAsync();
        }

        public async Task<ret_bool_int> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            var userid = _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber + 1;
            var allocation = await _context.LeaveAllocations
                .FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == userid);
            return new ret_bool_int { boolValue = (allocation.Days < numberOfDays), intValue = allocation.Days };
        }

        public struct ret_bool_int //mine
        {
            public bool boolValue;
            public int intValue;
        }

        public int LeaveRequestStatus_IDFromName(string name) //mine
        {
            return _context.LeaveRequestStatus.First(x => x.Name == name).Id;
        }
        public string LeaveRequestStatus_NameFromID(int id) //mine
        {
            return _context.LeaveRequestStatus.First(x => x.Id == id).Name;
        }

        public async Task ReviewLeaveRequestAsync(ReviewLeaveRequestVM model)
        {
            throw new NotImplementedException();
        }
    }
}
