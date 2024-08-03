using LeaveManagementSystem.Web.Data;
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

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? id)
        {
            await _leaveAllocationService.AllocateLeaveAsync(id);
            return RedirectToAction(nameof(Details), new {employeeId = id});
        }

        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _leaveAllocationService.GetEmployeeAllocationAsync(id.Value);
            if (allocation == null)
            {
                return NotFound();
            }
            return View(allocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocationEditVM)
        {
            await _leaveAllocationService.EditAllocationAsync(allocationEditVM);
            return RedirectToAction(nameof(Details), new { employeeId = allocationEditVM.Employee.Id});
        }

        public async Task<IActionResult> Details(string? employeeId)
        {
            var employeeVm = await _leaveAllocationService.GetEmployeeAllocationsAsync(employeeId);
            return View(employeeVm);
        }
    }
}
