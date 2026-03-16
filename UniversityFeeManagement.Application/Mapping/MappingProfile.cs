using AutoMapper;
using UniversityFeeManagement.Domain.Entities;
using UniversityFeeManagement.Application.DTOs;

namespace UniversityFeeManagement.Application.Mapping;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<CreateStudentDto, Student>();
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Fee, FeeDto>().ReverseMap();
        }
    }
