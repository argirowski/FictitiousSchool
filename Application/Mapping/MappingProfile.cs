using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ParticipantDTO, Participant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SubmitApplicationId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CourseDTO, Course>()
                .ForMember(dest => dest.CourseDates, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CourseDateDTO, CourseDate>()
                .ForMember(dest => dest.CourseId, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<FictitiousSchoolApplicationDTO, FictitiousSchoolApplication>()
                .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
