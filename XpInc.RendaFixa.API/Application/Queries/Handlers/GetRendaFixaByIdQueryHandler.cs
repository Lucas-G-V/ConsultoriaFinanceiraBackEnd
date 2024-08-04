using AutoMapper;
using MediatR;
using XpInc.Cache;
using XpInc.RendaFixa.API.Models.DTO.Response;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Interfaces;

namespace XpInc.RendaFixa.API.Application.Queries.Handlers
{
    public class GetRendaFixaByIdQueryHandler : IRequestHandler<GetRendaFixaById, RendaFixaDetalhadaResponse>
    {
        private readonly IRendaFixaRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheService _cache;
        private readonly IRendaFixaHistoricoRepository _rendaFixaHistoricoRepository;

        public GetRendaFixaByIdQueryHandler(IRendaFixaRepository repository, IMapper mapper, 
            IMemoryCacheService memoryCacheService, IRendaFixaHistoricoRepository rendaFixaHistoricoRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = memoryCacheService;
            _rendaFixaHistoricoRepository = rendaFixaHistoricoRepository;
        }

        public async Task<RendaFixaDetalhadaResponse> Handle(GetRendaFixaById request, CancellationToken cancellationToken)
        {
            var cache = await _cache.GetById<RendaFixaDetalhadaResponse>(request.IdProduto.ToString());
            if (cache == null || request.AtualizaCache)
            {
                var rendaFixa = await _repository.GetById(request.IdProduto);
                var historico = await _rendaFixaHistoricoRepository.GetByProductId(request.IdProduto);
                var response = _mapper.Map<RendaFixaDetalhadaResponse>(rendaFixa);
                response.Historico = _mapper.Map<IEnumerable<RendaFixaHistoricoResponse>>(historico);
                await _cache.AddMemoryCache(rendaFixa.Id.ToString(), response);
                return response;
            }
            return cache;
        }
    }
}
