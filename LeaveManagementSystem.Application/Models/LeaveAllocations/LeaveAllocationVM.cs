using LeaveManagementSystem.Application.Models.Periods;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Application.Models.LeaveAllocations
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }
        [Display(Name = "Number Of Days")]
        public int NumberOfDays { get; set; }
        [Display(Name = "Allocation Period")]
        public PeriodVM Period { get; set; } = new(); // avoid null reference

        public LeaveTypeReadOnlyVM LeaveType { get; set; } = new();
    }
}