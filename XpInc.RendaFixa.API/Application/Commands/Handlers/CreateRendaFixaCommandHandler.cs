using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using XpInc.ApiConfig.Services;
using XpInc.Core.Messages;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Interfaces;

namespace XpInc.RendaFixa.API.Application.Commands.Handlers
{
    public class CreateRendaFixaCommandHandler : CommandHandler,
         IRequestHandler<CreateRendaFixaCommand, ValidationResult>
    {
        private readonly IRendaFixaRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;

        public CreateRendaFixaCommandHandler(IRendaFixaRepository clienteRepository, IMapper mapper, IUsuarioService usuarioService)
        {
            _repository = clienteRepository;
            _mapper = mapper;
            _usuarioService = usuarioService;
        }

        public async Task<ValidationResult> Handle(CreateRendaFixaCommand message, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<RendaFixaProduto>(message);
            entity.UsuarioCadastroId = _usuarioService.GetUserId();
            if (!entity.EhValido()) return entity.RetornaValidationResult();
            await _repository.Add(entity);
            return await PersistirDados(_repository.UnitOfWork);
        }
    }
}
