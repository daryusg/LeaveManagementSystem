using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Data
{
    //[EntityTypeConfiguration(typeof(LeaveRequestStatusConfiguration))] //140. an alternative to using builder.ApplyConfiguration or builder.ApplyConfigurationsFromAssembly in the ApplicationDbContext class
    public class LeaveRequestStatus : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }
    }
}