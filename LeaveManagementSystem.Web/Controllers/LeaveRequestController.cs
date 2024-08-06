using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize] //authentication/login is a prerequisite
    public class LeaveRequestController (ILeaveTypesService _leaveTypesService) : Controller
    {
        //Employee View requests
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //Employee Create request
        public async Task<IActionResult> Create()
        {
            var leaveTypes =await _leaveTypesService.GetAllAsync();
            var leaveTypesList = new SelectList(leaveTypes, "Id", "Name");
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LeaveTypes = leaveTypesList
            };
            return View(model);
        }

        //Employee Create request
        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model /*use VM*/)
        {
            return View();
        }

        //Employee Cancel request
        [HttpPost]
        public async Task<IActionResult> Cancel(int createID /*use VM*/)
        {
            return View();
        }

        //Admin/Super review requests
        public async Task<IActionResult> ListRequests()
        {
            return View();
        }

        //Admin/Super review requests
        public async Task<IActionResult> Review(int leaveRequestID)
        {
            return View();
        }

        //Admin/Super review requests
        [HttpPost]
        public async Task<IActionResult> Review(/*use VM*/)
        {
            return View();
        }
    }
}
