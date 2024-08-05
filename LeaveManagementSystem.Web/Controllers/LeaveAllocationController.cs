using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize] // <--- added
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService, ILeaveTypesService _leaveTypesService) : Controller
    {
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
            var employeeVm = await _leaveAllocationsService.GetEmployeesAsync();
            return View(employeeVm);
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? id)
        {
            await _leaveAllocationsService.AllocateLeaveAsync(id);
            return RedirectToAction(nameof(Details), new {employeeId = id});
        }

        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _leaveAllocationsService.GetEmployeeAllocationAsync(id.Value);
            if (allocation == null)
            {
                return NotFound();
            }
            return View(allocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocation)
        {
            if (await _leaveTypesService.DaysExceedMaximumAsync(allocation.LeaveType.Id, allocation.NumberOfDays))
            {
                ModelState.AddModelError(nameof(allocation.NumberOfDays), "The allocation exceeds the maximum leave type value.");
            }
            if (ModelState.IsValid)
            {
                await _leaveAllocationsService.EditAllocationAsync(allocation);
                return RedirectToAction(nameof(Details), new { employeeId = allocation.Employee.Id });
            }
            //save days (from post)
            var days = allocation.NumberOfDays;
            //reload  allocation
            allocation = (LeaveAllocationEditVM)await _leaveAllocationsService.GetEmployeeAllocationAsync(allocation.Id);
            //set days (from post)
            allocation.NumberOfDays = days;
            return View(allocation);
        }

        public async Task<IActionResult> Details(string? employeeId)
        {
            var employeeVm = await _leaveAllocationsService.GetEmployeeAllocationsAsync(employeeId);
            return View(employeeVm);
        }
    }
}
