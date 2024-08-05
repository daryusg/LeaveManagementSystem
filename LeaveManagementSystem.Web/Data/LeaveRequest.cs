namespace LeaveManagementSystem.Web.Data
{
    public class LeaveRequest : BaseEntity
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        //-------------------------------------------
        //set up foreign key to the LeaveType table
        public LeaveType? LeaveType { get; set; } //make nullable 
        public int LeaveTypeId { get; set; }
        //-------------------------------------------
        //set up foreign key to the LeaveType table
        public LeaveRequestStatus? LeaveRequestStatus { get; set; } //make nullable 
        public int LeaveRequestStatusId { get; set; }
        //-------------------------------------------
        //set up foreign key to the Employee table
        //note the following attribute can also be used as an override: [ForeignKey("EmployeeId")]
        public ApplicatiionUser? Employee { get; set; } // <--- table name
        public string EmployeeId { get; set; } = default!; // <--- id column. set to null and remove warning
        //-------------------------------------------
        //set up foreign key to the Reviewer table
        public ApplicatiionUser? Reviewer { get; set; } // <--- table name
        public string? ReviewerId { get; set; } // <--- id column. nullable because, at set up, the reviewer may not be known
        //-------------------------------------------
        public string? RequestComments { get; set; }
    }
}