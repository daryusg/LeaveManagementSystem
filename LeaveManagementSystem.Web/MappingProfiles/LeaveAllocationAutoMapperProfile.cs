using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class LeaveAllocationAutoMapperProfile : Profile
    {
        public LeaveAllocationAutoMapperProfile() {
            CreateMap<LeaveAllocation, LeaveAllocationVM>()
                .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.Days)).ReverseMap();
            CreateMap<LeaveAllocation, LeaveAllocationEditVM>() //133
                .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.Days)).ReverseMap();

            CreateMap<ApplicatiionUser, EmployeeListVM>(); //130

            CreateMap<Period, PeriodVM>();
        }
    }
}
