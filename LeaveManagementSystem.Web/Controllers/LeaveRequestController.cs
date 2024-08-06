using LeaveManagementSystem.Web.Models.LeaveRequests;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize] //authentication/login is a prerequisite
    public class LeaveRequestController : Controller
    {
        //Employee View requests
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //Employee Create request
        public async Task<IActionResult> Create()
        {
            return View();
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
