using AutoMapper;
using XpInc.Transacao.API.Application.Commands;
using XpInc.Transacao.API.Models.DTO.Request;
using XpInc.Transacao.API.Models.Entities;

namespace XpInc.Transacao.API.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTransacaoRequest, CreateTransacaoCommand>();
            CreateMap<CreateTransacaoCommand, TransacaoCliente>();
            
        }
    }
}
