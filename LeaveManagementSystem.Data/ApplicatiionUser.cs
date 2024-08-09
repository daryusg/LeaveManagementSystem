namespace LeaveManagementSystem.Data
{
    public class ApplicatiionUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
