using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize] // <--- added
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationService) : Controller
    {
        public async Task<IActionResult> Details()
        {
            var employeeVm = await _leaveAllocationService.GetEmployeeAllocationsAsync();
            return View(employeeVm);
        }
    }
}
