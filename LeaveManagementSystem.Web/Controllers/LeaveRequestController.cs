﻿using LeaveManagementSystem.Application.Models.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using static LeaveManagementSystem.Application.Services.LeaveRequests.LeaveRequestsService;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize] //authentication/login is a prerequisite
    public class LeaveRequestController(ILeaveTypesService _leaveTypesService, ILeaveRequestsService _LeaveRequestsService) : Controller
    {
        //Employee View requests
        public async Task<IActionResult> Index()
        {
            var model = await _LeaveRequestsService.GetEmployeeLeaveRequestsAsync();
            return View(model);
        }

        //Employee Create request
        public async Task<IActionResult> Create(int leaveTypeId)
        {
            var leaveTypes = await _leaveTypesService.GetAllAsync();
            var leaveTypesList = new SelectList(leaveTypes, "Id", "Name", leaveTypeId);
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now), // need to format to: .ToString("yyyy/mm/dd")
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LeaveTypes = leaveTypesList
            };
            return View(model);
        }

        //Employee Create request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model)
        {
            //validate that the days don't exceed the allocation
            ret_bool_int _ret_bool_int = await _LeaveRequestsService.RequestDatesExceedAllocation(model);
            if (_ret_bool_int.boolValue)
            {
                ModelState.AddModelError(string.Empty, "You have exceeded your allocation");
                ModelState.AddModelError(nameof(model.EndDate), $"The number of requested days ({model.EndDate.DayNumber - model.StartDate.DayNumber + 1}) exceeds the allocation ({_ret_bool_int.intValue}).");
                //return RedirectToAction(nameof(Index), new { employeeId = model.Employee.Id });
            }

            if (ModelState.IsValid)
            {
                await _LeaveRequestsService.CreateLeaveRequestAsync(model);
                return RedirectToAction(nameof(Index));
            }
            //
            //note: at this point Model.Leavetypes is null because it's not bound. in Create.cshtml i populate LeaveTypeId with asp-items="@Model.LeaveTypes"
            //fix: i'll populate model similar to the Create()
            //
            var leaveTypes = await _leaveTypesService.GetAllAsync();
            model.LeaveTypes = new SelectList(leaveTypes, "Id", "Name");
            return View(model);
        }

        //Employee Cancel request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            await _LeaveRequestsService.CancelLeaveRequestAsync(id);
            //return View();
            return RedirectToAction(nameof(Index));
        }

        //Admin/Super review requests
        //[Authorize(Roles = $"{Roles.Administrator}, {Roles.Supervisor}")]
        [Authorize(Policy = "AdminSupervisorOnly")] // 164 see AddAuthorization in Program.cs
        //[Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> ListRequests()
        {
            var model = await _LeaveRequestsService.GetAllLeaveRequestsAsync();
            return View(model);
        }

        //Admin/Super review requests
        public async Task<IActionResult> Review(int id)
        {
            var model = await _LeaveRequestsService.GetLeaveRequestForReviewAsync(id);
            return View(model);
        }

        //Admin/Super review requests
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id, bool approved)
        {
            await _LeaveRequestsService.ReviewLeaveRequestAsync(id, approved);
            return RedirectToAction(nameof(ListRequests));
        }
    }
}
