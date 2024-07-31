using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            //CreateMap<LeaveType, IndexVM>();
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>()
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays)).ReverseMap();
            //CreateMap<LeaveTypeReadOnlyVM, LeaveType>()
            //    .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.Days));

            CreateMap<LeaveType, LeaveTypeEditVM>()
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays)).ReverseMap();

            CreateMap<LeaveType, LeaveTypeCreateVM>()
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays)).ReverseMap();
            //CreateMap<LeaveType, LeaveTypeCreateVM>()
            //    .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays));
        }
    }
}
