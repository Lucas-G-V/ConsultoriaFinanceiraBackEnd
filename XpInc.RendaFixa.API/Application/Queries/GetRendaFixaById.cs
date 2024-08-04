using MediatR;
using XpInc.RendaFixa.API.Models.Entities;

namespace XpInc.RendaFixa.API.Application.Queries
{
    public class GetRendaFixaById : IRequest<RendaFixaProduto>
    {
        public Guid IdProduto { get; set; }
    }
}
