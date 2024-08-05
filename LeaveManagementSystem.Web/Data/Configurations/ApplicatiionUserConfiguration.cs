using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class ApplicatiionUserConfiguration : IEntityTypeConfiguration<ApplicatiionUser> //140
    {
        public void Configure(EntityTypeBuilder<ApplicatiionUser> builder)
        {
            //add default user(s)
            var hasher = new PasswordHasher<ApplicatiionUser>();
            builder.HasData(
                new ApplicatiionUser
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
        }
    }
}
