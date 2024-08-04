using MediatR;
using XpInc.Cache;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Interfaces;

namespace XpInc.RendaFixa.API.Application.Queries.Handlers
{
    public class GetRendaFixaByIdQueryHandler : IRequestHandler<GetRendaFixaById, RendaFixaProduto>
    {
        private readonly IRendaFixaRepository _repository;

        public GetRendaFixaByIdQueryHandler(IRendaFixaRepository repository)
        {
            _repository = repository;
        }

        public async Task<RendaFixaProduto> Handle(GetRendaFixaById request, CancellationToken cancellationToken)
        {
            var rendaFixa = await _repository.GetById(request.IdProduto);
            return rendaFixa;
        }
    }
}
