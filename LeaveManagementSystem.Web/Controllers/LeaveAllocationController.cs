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

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? Id)
        {
            await _leaveAllocationService.AllocateLeave(Id);
            return RedirectToAction(nameof(Details), new {employeeId = Id});
        }
    }
}
