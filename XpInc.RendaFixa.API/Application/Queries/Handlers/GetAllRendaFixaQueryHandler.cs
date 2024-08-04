using MediatR;
using XpInc.Cache;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Interfaces;

namespace XpInc.RendaFixa.API.Application.Queries.Handlers
{
    public class GetAllRendaFixaQueryHandler : IRequestHandler<GetAllRendaFixaQuery, IEnumerable<RendaFixaProduto>>
    {
        private readonly IRendaFixaRepository _repository;
        private readonly IMemoryCacheService _cache;

        public GetAllRendaFixaQueryHandler(IRendaFixaRepository repository, IMemoryCacheService memoryCacheService)
        {
            _repository = repository;
            _cache = memoryCacheService;
        }

        public async Task<IEnumerable<RendaFixaProduto>> Handle(GetAllRendaFixaQuery request, CancellationToken cancellationToken)
        {
            var cache =  await _cache.GetById<IEnumerable<RendaFixaProduto>>("rendaFixa");
            if (cache == null || request.AtualizaCache)
            {
                var rendasFixa = await _repository.GetAll();
                await _cache.AddMemoryCache("rendaFixa", rendasFixa);
                return rendasFixa;
            }
            return cache;
        }
    }
}
