using MediatR;
using XpInc.RendaFixa.API.Models.Entities;

namespace XpInc.RendaFixa.API.Application.Queries
{
    public class GetRendaFixaProximaVencimentoQuery : IRequest<IEnumerable<RendaFixaProduto>>
    {
        public DateTime DataComparacao { get; set; }
        public GetRendaFixaProximaVencimentoQuery(DateTime dataComparacao)
        {
            DataComparacao = dataComparacao;
        }
    }
}
