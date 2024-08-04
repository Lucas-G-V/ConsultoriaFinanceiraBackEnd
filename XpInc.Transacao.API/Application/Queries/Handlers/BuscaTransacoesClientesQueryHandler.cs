using MediatR;
using XpInc.Cache;
using XpInc.Transacao.API.Models.Entities;
using XpInc.Transacao.API.Models.Interfaces;

namespace XpInc.Transacao.API.Application.Queries.Handlers
{
    public class BuscaTransacoesClientesQueryHandler : IRequestHandler<BuscaTransacoesClientesQuery, IEnumerable<TransacaoCliente>>
    {
        private readonly ITransacaoRepository _repository;
        private readonly IMemoryCacheService _cache;

        public BuscaTransacoesClientesQueryHandler(ITransacaoRepository repository, IMemoryCacheService memoryCacheService)
        {
            _repository = repository;
            _cache = memoryCacheService;
        }

        public async Task<IEnumerable<TransacaoCliente>> Handle(BuscaTransacoesClientesQuery request, CancellationToken cancellationToken)
        {
            var cache = await _cache.GetById<IEnumerable<TransacaoCliente>>($"{request.IdCliente.ToString()}extrato");
            if (cache == null)
            {
                var extratoCliente = await _repository.GetByIdCliente(request.IdCliente);
                await _cache.AddMemoryCache($"{request.IdCliente.ToString()}extrato", extratoCliente);
                return extratoCliente;
            }
            return cache;
        }
    }
}
