using ASPNetMicroserviceTemplate.Dtos;
using ASPNetMicroserviceTemplate.Model;
using AutoMapper;

namespace ASPNetMicroserviceTemplate.Profiles 
{
    /// Should be removed from the real project!
    public class SomeModelProfile : Profile
    {
        public SomeModelProfile()
        {
            // Source --> Target 
            CreateMap<SomeModel, SomeModelReadDto>();
            CreateMap<SomeModelCreateDto, SomeModel>();
        }
    }
}