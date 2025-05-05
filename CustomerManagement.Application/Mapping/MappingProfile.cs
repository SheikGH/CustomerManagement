using AutoMapper;
using CustomerManagement.Core.Entities;
using CustomerManagement.Common.DTOs;

namespace CustomerManagement.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Appraisal, AppraisalDto>().ReverseMap();
            CreateMap<Appraisal, AppraisalTaskDto>().ReverseMap();
            CreateMap<AppraisalDto, AppraisalTaskDto > ().ReverseMap();
        }
    }
}
