using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class LeaveTypeCreateVM
    {
        [Required]
        [Length(4, 150)]
        //[Length(4, 150, ErrorMessage = "You have violated the length requirement")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, 90)]
        public int Days { get; set; }
    }
}
