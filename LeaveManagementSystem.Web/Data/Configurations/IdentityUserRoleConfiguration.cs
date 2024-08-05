using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>> //140
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            //link the admin user to the admin role
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "f7e413fe-dbb4-4a48-882f-679aae30b6d7",
                    UserId = "680742ad-1384-47eb-92ae-26c0050c3b3c"
                });
        }
    }
}
