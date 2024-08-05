using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data
{
    public class LeaveAllocation : BaseEntity
    {
        //-------------------------------------------
        //set up foreign key to the LeaveType table
        public LeaveType? LeaveType { get; set; } //make nullable 
        public int LeaveTypeId { get; set; }
        //-------------------------------------------
        //set up foreign key to the Employee table
        //note the following attribute can also be used as an override: [ForeignKey("EmployeeId")]
        public ApplicatiionUser Employee { get; set; } // <--- table name
        public string EmployeeId { get; set; } // <--- id column
        //-------------------------------------------
        //set up foreign key to the Period table
        public Period Period { get; set; }
        public int PeriodId { get; set; }
        //-------------------------------------------
        public int Days { get; set; }

    }
}
