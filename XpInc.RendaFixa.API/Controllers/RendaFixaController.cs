using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using XpInc.ApiConfig.Controllers;
using XpInc.ApiConfig.Services;
using XpInc.Cache;
using XpInc.Core.MediatorHandler;
using XpInc.RendaFixa.API.Application.Commands;
using XpInc.RendaFixa.API.Application.Queries;
using XpInc.RendaFixa.API.Models.DTO.Request;
using XpInc.RendaFixa.API.Models.DTO.Response;
using XpInc.RendaFixa.API.Models.Entities;

namespace XpInc.RendaFixa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RendaFixaController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IMapper _mapper;
        public RendaFixaController(IMediatorHandler mediatorHandler, IMapper mapper)
        {
            _mediator = mediatorHandler;
            _mapper = mapper;
        }

        [HttpPost]
        [ClaimsAuthorize("RendaFixa", "Escrever")]
        public async Task<IActionResult> Create([FromBody] CreateRendaFixaRequest request)
        {
            var command = _mapper.Map<CreateRendaFixaCommand>(request);
            var result = await _mediator.EnviarComando(command);
            if (!result.IsValid) return CustomResponse(result);
            return NoContent();
        }

        [HttpPut("AlteraProdutoFinanceiro")]
        [ClaimsAuthorize("RendaFixa", "Editar")]
        public async Task<IActionResult> Update([FromBody] UpdateRendaFixaRequest request)
        {
            var command = _mapper.Map<UpdateRendaFixaAdminCommand>(request);
            var result = await _mediator.EnviarComando(command);
            if (!result.IsValid) return CustomResponse(result);
            return NoContent();
        }

        [HttpGet("GetAll")]
        [ClaimsAuthorize("RendaFixa", "LerRestrito")]
        public async Task<ActionResult<IEnumerable<RendaFixaProduto>>> GetAll()
        {
            var query = new GetAllRendaFixaQuery();
            var rendaFixaList = await _mediator.BuscarQuery(query);
            return Ok(rendaFixaList);
        }

        [HttpGet("GetAllForClientes")]
        [ClaimsAuthorize("RendaFixa", "Ler")]
        public async Task<ActionResult<IEnumerable<RendaFixaProduto>>> GetAllForClientes()
        {
            var query = new GetAllRendaFixaQuery();
            var rendaFixaList = await _mediator.BuscarQuery(query);
            return Ok(rendaFixaList);
        }

        [HttpGet("GetById/{id}")]
        [ClaimsAuthorize("RendaFixa", "Ler")]
        public async Task<ActionResult<RendaFixaDetalhadaResponse>> GetById(Guid id)
        {
            var query = new GetRendaFixaById(id);
            var rendaFixaList = await _mediator.BuscarQuery(query);
            return Ok(rendaFixaList);
        }
    }
}
