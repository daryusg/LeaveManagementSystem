using LeaveManagementSystem.Application.Services.Email;
using LeaveManagementSystem.Application.Services.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LeaveManagementSystem.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<ILeaveTypesService, LeaveTypesService>(); //90
            services.AddScoped<ILeaveAllocationsService, LeaveAllocationsService>(); //122
            services.AddScoped<ILeaveRequestsService, LeaveRequestsService>(); //141
            services.AddScoped<IPeriodsService, PeriodsService>(); //161
            services.AddScoped<IUserService, UserService>(); //161
            services.AddTransient<IEmailSender, EmailSender>(); //109
            return services;
        }

    }
}
