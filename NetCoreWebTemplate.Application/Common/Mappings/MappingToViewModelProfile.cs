using AutoMapper;
using NetCoreWebTemplate.Application.Clients.Commands.CreateClient;
using NetCoreWebTemplate.Domain.Entities;

namespace NetCoreWebTemplate.Application.Common.Mappings
{
    public class MappingToViewModelProfile : Profile
    {
        public MappingToViewModelProfile()
        {
            CreateMap<Client, ClientViewModel>();
        }
    }
}
