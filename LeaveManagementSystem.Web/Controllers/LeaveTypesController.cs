using LeaveManagementSystem.Application.Models.LeaveTypes;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)] // <--- added
    public class LeaveTypesController(ILeaveTypesService _leaveTypesService, ILogger<LeaveTypesController> _logger) : Controller
    {
        private const string NameExistsValidationMessage = "Leave Type already exists";

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Loading Leave Types"); //176
            var viewData = await _leaveTypesService.GetAllAsync();
            return View(viewData);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Parameterization - key for preventing SQL Injection attacks
            //SELECT * FROM LeaveTypes WHERE (Id = @id)
            var leaveType = await _leaveTypesService.GetAsync<LeaveTypeReadOnlyVM>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(leaveType);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
        {
            //adding custom validation and model state error
            //prevent duplicates
            if (await _leaveTypesService.CheckIfLeaveTypeNameExistsAsync(leaveTypeCreate.Name))
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.Name), NameExistsValidationMessage);
            }

            if (ModelState.IsValid)
            {
                _logger.LogWarning("Leave Type creation failed"); //176
                await _leaveTypesService.CreateAsync(leaveTypeCreate);
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeCreate);
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //SELECT * FROM LeaveTypes WHERE (Id = @id)
            var leaveType = await _leaveTypesService.GetAsync<LeaveTypeEditVM>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumberOfDays")] LeaveType leaveType)
        {
            if (id != leaveTypeEdit.Id)
            {
                return NotFound();
            }

            //adding custom validation and model state error
            //prevent duplicates
            if (await _leaveTypesService.CheckIfLeaveTypeNameExistsForEditAsync(leaveTypeEdit))
            {
                ModelState.AddModelError(nameof(leaveTypeEdit.Name), NameExistsValidationMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _leaveTypesService.EditAsync(leaveTypeEdit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _leaveTypesService.LeaveTypeExistsAsync(leaveTypeEdit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeEdit);
        }

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.GetAsync<LeaveTypeReadOnlyVM>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(leaveType);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveType = await _leaveTypesService.GetAsync<LeaveTypeReadOnlyVM>(id);
            {
                await _leaveTypesService.RemoveAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
