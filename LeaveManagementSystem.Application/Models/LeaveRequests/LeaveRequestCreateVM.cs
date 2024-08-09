using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Application.Models.LeaveRequests
{
    public class LeaveRequestCreateVM : IValidatableObject
    {
        [Display(Name = "Start Date")]
        [Required]
        //[DisplayFormat(DataFormatString = "{0:yy-mm-dd}", ApplyFormatInEditMode = true)] //https://stackoverflow.com/questions/18589270/displaying-date-only-mvc
        public DateOnly StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        //[DisplayFormat(DataFormatString = "{0:yy-mm-dd}", ApplyFormatInEditMode = true)] //https://stackoverflow.com/questions/18589270/displaying-date-only-mvc
        public DateOnly EndDate { get; set; }
        [Required]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }
        [Display(Name = "Additional Information")]
        //[StringLength(250)] //see Validate() below
        public string? RequestComments { get; set; }
        public SelectList? LeaveTypes { get; set; } //146 unbound items can fail due to nulls 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //start date < enddate
            if (StartDate > EndDate)
            {
                yield return new ValidationResult("The End Date must be after the Start Date", new[]
                {nameof(StartDate), nameof(EndDate)});
            }

            int maxLength = 250;
            int commentLength = (RequestComments + "").Length;
            if (commentLength > maxLength)
            {
                yield return new ValidationResult($"The maximum size allowed is 250 characters ({commentLength})", new[]
                {nameof(RequestComments)});
            }
        }
    }
}