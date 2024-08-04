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
    public class ProcessoCompraRendaFixaCommandHandler: CommandHandler,
         IRequestHandler<ProcessoCompraRendaFixaCommand, ValidationResult>
    {
        private readonly IRendaFixaRepository _repository;
        private readonly IUsuarioService _usuarioService;
        private readonly IMediatorHandler _mediator;

        public ProcessoCompraRendaFixaCommandHandler(IRendaFixaRepository clienteRepository, IUsuarioService usuarioService, IMediatorHandler mediatorHandler)
        {
            _repository = clienteRepository;
            _usuarioService = usuarioService;
            _mediator = mediatorHandler;
        }

        public async Task<ValidationResult> Handle(ProcessoCompraRendaFixaCommand message, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(message.Id);
            if (entity == null)
            {
                AdicionarErro("Produto não encontrado");
                return ValidationResult;
            }
            var quantidadeAnterior = entity.QuantidadeCotasDisponivel;
            entity.DebitaQuantidadeDeCotas(message.QuantidadeDeCotasDebitadas);

            if (!entity.EhValido()) return entity.RetornaValidationResult();
            if (message.Nome != entity.Nome || message.ValorUnitario != entity.ValorUnitario ||
                entity.ValorMinimo > (message.QuantidadeDeCotasDebitadas * entity.ValorUnitario))
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
            await _mediator.BuscarQuery(new GetRendaFixaById(rendaFixaProduto.Id, true));
            if ((rendaFixaProduto.QuantidadeCotasDisponivel == 0 && quantidadeAnterior != 0)
                || (rendaFixaProduto.QuantidadeCotasDisponivel > 0 && quantidadeAnterior == 0))
            {
                await _mediator.BuscarQuery(new GetAllRendaFixaQuery(true));
            }
        }
    }
}
