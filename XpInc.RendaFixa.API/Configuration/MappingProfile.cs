using AutoMapper;
using XpInc.RendaFixa.API.Application.Commands;
using XpInc.RendaFixa.API.Models.DTO.Request;
using XpInc.RendaFixa.API.Models.Entities;

namespace XpInc.RendaFixa.API.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateRendaFixaRequest, CreateRendaFixaCommand>();
            CreateMap<UpdateRendaFixaRequest, UpdateRendaFixaCommand>();


            CreateMap<CreateRendaFixaCommand, RendaFixaProduto>();
        }
    }
}
