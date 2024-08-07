using LeaveManagementSystem.Web.Services.LeaveRequests;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
    public class LeaveRequestReadOnlyVM
    {
        public int Id { get; set; }

        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }
        [Display(Name = "Total Days")]
        public int NumberOfDays { get; set; }
        [Display(Name = "Leave Type")]
        public string LeaveType { get; set; } = string.Empty;
        [Display(Name = "Status")]
        /*//mine to enable LeaveRequestStatusEnum bypass
        public string LeaveRequestStatus { get; set; }
        public int LeaveRequestStatusID { get; set; }
        */
        public LeaveRequestStatusEnum LeaveRequestStatus { get; set; }
    }
}