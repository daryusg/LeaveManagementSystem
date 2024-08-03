using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize] // <--- added
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationService) : Controller
    {
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
            var employeeVm = await _leaveAllocationService.GetEmployeesAsync();
            return View(employeeVm);
        }

        public async Task<IActionResult> Details(string? employeeId)
        {
            var employeeVm = await _leaveAllocationService.GetEmployeeAllocationsAsync(employeeId);
            return View(employeeVm);
        }
    }
}
