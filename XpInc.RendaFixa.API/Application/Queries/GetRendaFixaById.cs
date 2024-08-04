using MediatR;
using XpInc.RendaFixa.API.Models.DTO.Response;
using XpInc.RendaFixa.API.Models.Entities;

namespace XpInc.RendaFixa.API.Application.Queries
{
    public class GetRendaFixaById : IRequest<RendaFixaDetalhadaResponse>
    {
        public Guid IdProduto { get; set; }
        public bool AtualizaCache { get; set; }
        public GetRendaFixaById(Guid idProduto, bool atualizaCache = false)
        {
            IdProduto = idProduto;
            AtualizaCache = atualizaCache;
        }
    }
}
