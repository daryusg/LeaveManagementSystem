using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data
{
    public class LeaveType : BaseEntity
    {
        [Column(TypeName = "nvarchar(150)")] //or [MaxLength(150)]
        public  string Name { get; set; } = string.Empty;
        [Display(Name = "Number of days")]
        public int NumberOfDays { get; set; }
    }
}
