using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data
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
            //add default data. to add default LeaveTypes, use  builder.Entity<LeaveType>().HasData(  new LeaveType {...});
            //------------------------------------------------------------------
            //add default role(s)
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "ecb2c98b-8f29-41a2-b892-75ebeb75ccd0",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new IdentityRole
                {
                    Id = "3f6ecb32-cc6e-4eb6-a90e-7268e00edeb3",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"
                },
                new IdentityRole
                {
                    Id = "f7e413fe-dbb4-4a48-882f-679aae30b6d7",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                });
            //add default user(s)
            var hasher = new PasswordHasher<ApplicatiionUser>();
            //var hasher = new PasswordHasher<ApplicatiionUser>();
            builder.Entity<ApplicatiionUser>().HasData(
                new ApplicatiionUser
                //builder.Entity<ApplicatiionUser>().HasData(
                //  new ApplicatiionUser
                {
                    Id = "680742ad-1384-47eb-92ae-26c0050c3b3c",
                    Email = "daryus@europe.com",
                    NormalizedEmail = "DARYUS@EUROPE.COM",
                    UserName = "daryus@europe.com",
                    PasswordHash = hasher.HashPassword(null, "P@ssw0rd"),
                    EmailConfirmed = true,
                    FirstName = "Default",
                    LastName = "Admin",
                    DateOfBirth = new DateOnly(1950, 12, 01)
                }
                );
            //link the admin user to the admin role
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "f7e413fe-dbb4-4a48-882f-679aae30b6d7",
                    UserId = "680742ad-1384-47eb-92ae-26c0050c3b3c"
                });
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            //------------------------------------------------------------------
        }

        public DbSet<LeaveType> LeaveTypes { get; set; }
    }
}
