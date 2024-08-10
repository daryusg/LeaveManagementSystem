using LeaveManagementSystem.Application;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); //171 end. similarly to AddApplicationServices below this can be moved to LeaveManagementSystem.Data
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
*/
//
//
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); 171 moved to ApplicationServicesRegistration.cs (LeaveManagementSystem.Application) where we add below.
DataServicesRegistration.AddDataServices(builder.Services, builder.Configuration);
ApplicationServicesRegistration.AddApplicationServices(builder.Services); //171

builder.Host.UseSerilog((ctx, config) => 
    config.WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration)
);

builder.Services.AddHttpContextAccessor(); //126

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminSupervisorOnly", policy =>
    {
        policy.RequireRole(Roles.Administrator, Roles.Supervisor); // = x OR y (note: x OR y ie can be in either role)
        //policy.RequireRole(Roles.Administrator); //x (note: x AND y ie must be in both roles)
        //policy.RequireRole(Roles.Supervisor);    //y (note: x AND y ie must be in both roles)
    });
}); //164
//
//
//builder.Services.AddDefaultIdentity<ApplicatiionUser>(options => options.SignIn.RequireConfirmedAccount = true) 177
builder.Services.AddDefaultIdentity<ApplicatiionUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 3;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>() /* <---inserted */
    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
