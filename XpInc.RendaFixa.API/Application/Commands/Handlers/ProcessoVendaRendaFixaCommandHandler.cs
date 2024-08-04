using FluentValidation.Results;
using MediatR;
using XpInc.ApiConfig.Services;
using XpInc.Core.MediatorHandler;
using XpInc.Core.Messages;
using XpInc.RendaFixa.API.Application.Queries;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Interfaces;

namespace XpInc.RendaFixa.API.Application.Commands.Handlers
{
    public class ProcessoVendaRendaFixaCommandHandler : CommandHandler,
         IRequestHandler<ProcessoVendaRendaFixaCommand, ValidationResult>
    {
        private readonly IRendaFixaRepository _repository;
        private readonly IMediatorHandler _mediator;

        public ProcessoVendaRendaFixaCommandHandler(IRendaFixaRepository clienteRepository, IMediatorHandler mediatorHandler)
        {
            _repository = clienteRepository;
            _mediator = mediatorHandler;
        }

        public async Task<ValidationResult> Handle(ProcessoVendaRendaFixaCommand message, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(message.Id);
            if (entity == null)
            {
                AdicionarErro("Produto não encontrado");
                return ValidationResult;
            }
            var quantidadeAnterior = entity.QuantidadeCotasDisponivel;
            message.QuantidadeDeCotasDebitadas *= -1;
            entity.DebitaQuantidadeDeCotas(message.QuantidadeDeCotasDebitadas);

            if (!entity.EhValido()) return entity.RetornaValidationResult();
            if (message.Nome != entity.Nome || message.ValorUnitario != entity.ValorUnitario)
            {
                AdicionarErro("Transação não atendida");
                return ValidationResult;
            }

            await _repository.Update(entity);
            var result = await PersistirDados(_repository.UnitOfWork);
            await AtualizaCache(entity, quantidadeAnterior);
            return result;
        }

        private async Task AtualizaCache(RendaFixaProduto rendaFixaProduto, int? quantidadeAnterior)
        {
            if ((rendaFixaProduto.QuantidadeCotasDisponivel == 0 && quantidadeAnterior != 0)
                || (rendaFixaProduto.QuantidadeCotasDisponivel > 0 && quantidadeAnterior == 0))
            {
                await _mediator.BuscarQuery(new GetAllRendaFixaQuery(true));
            }
        }
    }
}
