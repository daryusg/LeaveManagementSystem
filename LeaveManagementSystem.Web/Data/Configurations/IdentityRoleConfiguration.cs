using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole> //140
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            //add default role(s)
            builder.HasData(
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

                }
            );
        }
    }
}
