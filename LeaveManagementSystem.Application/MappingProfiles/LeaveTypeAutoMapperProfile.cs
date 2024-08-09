namespace LeaveManagementSystem.Application.MappingProfiles
{
    public class LeaveTypeAutoMapperProfile : Profile
    {
        public LeaveTypeAutoMapperProfile()
        {
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
