using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using XpInc.ApiConfig.Controllers;
using XpInc.ApiConfig.Services;
using XpInc.Cache;
using XpInc.Core.MediatorHandler;
using XpInc.Transacao.API.Application.Commands;
using XpInc.Transacao.API.Application.Queries;
using XpInc.Transacao.API.Models.DTO.Request;
using XpInc.Transacao.API.Models.DTO.Response;
using XpInc.Transacao.API.Models.Entities;
using XpInc.Transacao.API.Models.Enums;

namespace XpInc.Transacao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacaoController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        public TransacaoController(IMediatorHandler mediatorHandler, IMapper mapper, IUsuarioService usuarioService)
        {
            _mediator = mediatorHandler;
            _mapper = mapper;
            _usuarioService = usuarioService;
        }

        [HttpPost("Deposito")]
        [ClaimsAuthorize("Transacao", "Escrever")]
        public async Task<IActionResult> Deposito([FromBody] DepositoSaqueRequest request)
        {
            var command = _mapper.Map<CreateTransacaoCommand>(request);
            command.NomeProduto = "Venda";
            command.Tipo = TipoTransacao.Deposito;
            var result = await _mediator.EnviarComando(command);
            if (!result.IsValid) return CustomResponse(result);
            return NoContent();
        }

        [HttpPost("Saque")]
        [ClaimsAuthorize("Transacao", "Escrever")]
        public async Task<IActionResult> Saque([FromBody] DepositoSaqueRequest request)
        {
            var command = _mapper.Map<CreateTransacaoCommand>(request);
            command.NomeProduto = "Saque";
            command.Tipo = TipoTransacao.Saque;
            var result = await _mediator.EnviarComando(command);
            if (!result.IsValid) return CustomResponse(result);
            return NoContent();
        }

        [HttpPost("Compra")]
        [ClaimsAuthorize("Transacao", "Escrever")]
        public async Task<IActionResult> Compra([FromBody] CompraVendaRequest request)
        {
            var command = _mapper.Map<CreateTransacaoCommand>(request);
            command.Tipo = TipoTransacao.Compra;
            var result = await _mediator.EnviarComando(command);
            if (!result.IsValid) return CustomResponse(result);
            return NoContent();
        }

        [HttpPost("Venda")]
        [ClaimsAuthorize("Transacao", "Escrever")]
        public async Task<IActionResult> Venda([FromBody] CompraVendaRequest request)
        {
            var command = _mapper.Map<CreateTransacaoCommand>(request);
            command.Tipo = TipoTransacao.Venda;
            var result = await _mediator.EnviarComando(command);
            if (!result.IsValid) return CustomResponse(result);
            return NoContent();
        }

        [HttpGet("GetExtratoCliente")]
        [ClaimsAuthorize("Transacao", "Escrever")]
        public async Task<ActionResult<IEnumerable<TransacaoResponse>>> GetExtratoCliente()
        {
            var clientId = _usuarioService.GetUserId();
            var query = new BuscaTransacoesClientesQuery(clientId);
            var saldoAtual = await _mediator.BuscarQuery(query);
            return Ok(_mapper.Map<IEnumerable<TransacaoResponse>>(saldoAtual));
        }

        [HttpGet("GetExtratoClienteAdmin/{idCliente}")]
        [ClaimsAuthorize("Transacao", "Escrever")]
        public async Task<IActionResult> GetExtradoClienteAdmin(Guid idCliente)
        {
            var query = new BuscaTransacoesClientesQuery(idCliente);
            var saldoAtual = await _mediator.BuscarQuery(query);
            return Ok(_mapper.Map<IEnumerable<TransacaoResponse>>(saldoAtual));
        }
    }
}
