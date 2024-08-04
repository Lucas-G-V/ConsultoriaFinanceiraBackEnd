using Microsoft.AspNetCore.Mvc;
using XpInc.ApiConfig.Controllers;
using XpInc.ApiConfig.Services;
using XpInc.Cache;
using XpInc.ContaCliente.API.Application.Queries;
using XpInc.ContaCliente.API.Models.Entities;
using XpInc.Core.MediatorHandler;

namespace XpInc.ContaCliente.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaClienteController : MainController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMediatorHandler _mediator;
        public ContaClienteController(IUsuarioService usuarioService, IMediatorHandler mediatorHandler)
        {
            _usuarioService = usuarioService;
            _mediator = mediatorHandler;
        }

        [HttpGet("GetSaldoAtual")]
        [ClaimsAuthorize("ContaCliente", "Ler")]
        public async Task<ActionResult<ContaClienteSaldo>>  GetDatosAtuais()
        {
            var clientId = _usuarioService.GetUserId();
            var query = new SaldoClienteQuery(clientId);
            var saldoAtual = await _mediator.BuscarQuery(query);

            return Ok(saldoAtual);
        }
    }
}
