using LeaveManagementSystem.Web.Services.Email;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using LeaveManagementSystem.Web.Services.LeaveAllocations;
using LeaveManagementSystem.Web.Services.LeaveRequests;
using LeaveManagementSystem.Web.Services.Periods;
using LeaveManagementSystem.Web.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpContextAccessor(); //126
//
//
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<ILeaveTypesService, LeaveTypesService>(); //90
builder.Services.AddScoped<ILeaveAllocationsService, LeaveAllocationsService>(); //122
builder.Services.AddScoped<ILeaveRequestsService, LeaveRequestsService>(); //141
builder.Services.AddScoped<IPeriodsService, PeriodsService>(); //161
builder.Services.AddScoped<IUserService, UserService>(); //161
builder.Services.AddTransient<IEmailSender, EmailSender>(); //109

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
builder.Services.AddDefaultIdentity<ApplicatiionUser>(options => options.SignIn.RequireConfirmedAccount = true)
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
