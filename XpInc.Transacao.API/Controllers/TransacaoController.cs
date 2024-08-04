using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using XpInc.ApiConfig.Controllers;
using XpInc.ApiConfig.Services;
using XpInc.Cache;
using XpInc.Core.MediatorHandler;
using XpInc.Transacao.API.Application.Commands;
using XpInc.Transacao.API.Models.DTO.Request;
using XpInc.Transacao.API.Models.Enums;

namespace XpInc.Transacao.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransacaoController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IMapper _mapper;
        public TransacaoController(IMediatorHandler mediatorHandler, IMapper mapper)
        {
            _mediator = mediatorHandler;
            _mapper = mapper;
        }

        [HttpPost]
        [ClaimsAuthorize("Transacao", "Escrever")]
        public async Task<IActionResult> Create([FromBody] CreateTransacaoRequest request)
        {
            var command = _mapper.Map<CreateTransacaoCommand>(request);
            command.Status = StatusTransacao.Pendente;
            command.DataTransacao = DateTime.UtcNow;
            var result = await _mediator.EnviarComando(command);
            if (!result.IsValid) return CustomResponse(result);
            return NoContent();
        }
    }
}
