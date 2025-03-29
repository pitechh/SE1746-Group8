using AutoMapper;
using FLearning.EnrollmentService.DTOs;
using FLearning.EnrollmentService.Models;

namespace FLearning.EnrollmentService.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Enrollment, EnrollmentResponseDTO>().ReverseMap();
            CreateMap<EnrollmentRequestDTO, Enrollment>().ReverseMap();
            CreateMap<Enrollment, EnrollmentDetailDTO>().ReverseMap();
            CreateMap<Student, StudentDTO>().ReverseMap();
        }

    }
}
