using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace LeaveManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicatiionUser>
    //public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            //add default data. to add default LeaveTypes, use builder.Entity<LeaveType>().HasData(  new LeaveType {...});
            //------------------------------------------------------------------
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //scans through the assembly for any class that inherits from IEntityTypeConfiguration
            //builder.ApplyConfiguration(new IdentityRoleConfiguration()); //140
            //builder.ApplyConfiguration(new ApplicatiionUserConfiguration()); //140
            //builder.ApplyConfiguration(new IdentityUserRoleConfiguration()); //140

            //builder.ApplyConfiguration(new LeaveRequestStatusConfiguration()); //140
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            //------------------------------------------------------------------
        }

        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<LeaveRequestStatus> LeaveRequestStatus { get; set; } //139
        public DbSet<LeaveRequest> LeaveRequest { get; set; } //139
    }
}
