namespace LeaveManagementSystem.Data.Configurations
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
                    Name = Roles.Employee,
                    NormalizedName = Roles.Employee.ToUpper()
                },
                new IdentityRole
                {
                    Id = "3f6ecb32-cc6e-4eb6-a90e-7268e00edeb3",
                    Name = Roles.Supervisor,
                    NormalizedName = Roles.Supervisor.ToUpper()
                },
                new IdentityRole
                {
                    Id = "f7e413fe-dbb4-4a48-882f-679aae30b6d7",
                    Name = Roles.Administrator,
                    NormalizedName = Roles.Administrator.ToUpper()

                }
            );
        }
    }
}
