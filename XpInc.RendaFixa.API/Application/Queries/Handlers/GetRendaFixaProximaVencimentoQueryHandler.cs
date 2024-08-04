using MediatR;
using XpInc.Cache;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Interfaces;

namespace XpInc.RendaFixa.API.Application.Queries.Handlers
{
    public class GetRendaFixaProximaVencimentoQueryHandler : IRequestHandler<GetRendaFixaProximaVencimentoQuery, IEnumerable<RendaFixaProduto>>
    {
        private readonly IRendaFixaRepository _repository;

        public GetRendaFixaProximaVencimentoQueryHandler(IRendaFixaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RendaFixaProduto>> Handle(GetRendaFixaProximaVencimentoQuery request, CancellationToken cancellationToken)
        {
            var rendasFixa = await _repository.GetProximasAoVencimento(request.DataComparacao, 3);
            return rendasFixa;
        }
    }
}
