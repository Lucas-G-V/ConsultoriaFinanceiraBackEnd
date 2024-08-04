using AutoMapper;
using FluentValidation.Results;
using MediatR;
using XpInc.ApiConfig.Services;
using XpInc.Core.MediatorHandler;
using XpInc.Core.Messages;
using XpInc.RendaFixa.API.Application.Queries;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace XpInc.RendaFixa.API.Application.Commands.Handlers
{
    public class UpdateRendaFixaCommandHandler : CommandHandler,
         IRequestHandler<UpdateRendaFixaCommand, ValidationResult>
    {
        private readonly IRendaFixaRepository _repository;
        private readonly IUsuarioService _usuarioService;
        private readonly IMediatorHandler _mediator;

        public UpdateRendaFixaCommandHandler(IRendaFixaRepository clienteRepository,IUsuarioService usuarioService, IMediatorHandler mediatorHandler)
        {
            _repository = clienteRepository;
            _usuarioService = usuarioService;
            _mediator = mediatorHandler;
        }

        public async Task<ValidationResult> Handle(UpdateRendaFixaCommand message, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(message.Id);
            if(entity == null)
            {
                AdicionarErro("Produto não encontrado");
                return ValidationResult;
            }
            var quantidadeAnterior = entity.QuantidadeCotasDisponivel;
            if(message.QuantidadeCotasDisponivel != null) entity.QuantidadeCotasDisponivel = message.QuantidadeCotasDisponivel;
            if (message.QuantidadeDeCotasDebitadas != null) entity.DebitaQuantidadeDeCotas(message.QuantidadeDeCotasDebitadas.Value);

            if (!entity.EhValido()) return entity.RetornaValidationResult();
            
            await _repository.Update(entity);
            var result =  await PersistirDados(_repository.UnitOfWork);
            await AtualizaCache(entity, quantidadeAnterior);
            return result;
        }

        private async Task AtualizaCache(RendaFixaProduto rendaFixaProduto, int? quantidadeAnterior)
        {
            if((rendaFixaProduto.QuantidadeCotasDisponivel == 0 && quantidadeAnterior != 0)
                || (rendaFixaProduto.QuantidadeCotasDisponivel > 0 && quantidadeAnterior == 0))
            {
                await _mediator.BuscarQuery(new GetAllRendaFixaQuery(true));
            }
        }
    }
}
