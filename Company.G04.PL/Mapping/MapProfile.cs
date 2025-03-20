using AutoMapper;
using Company.G04.DAL.Moudel;
using Company.G04.PL.Dtos;

namespace Company.G04.PL.Mapping
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>().ReverseMap();
            CreateMap<CreateDepartmentDto, Department>().ReverseMap();
        }
    }
}
