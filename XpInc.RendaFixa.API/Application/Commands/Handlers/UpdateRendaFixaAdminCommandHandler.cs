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
    public class UpdateRendaFixaAdminCommandHandler : CommandHandler,
         IRequestHandler<UpdateRendaFixaAdminCommand, ValidationResult>
    {
        private readonly IRendaFixaRepository _repository;
        private readonly IRendaFixaHistoricoRepository _historicoRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IMediatorHandler _mediator;

        public UpdateRendaFixaAdminCommandHandler(IRendaFixaRepository clienteRepository, IUsuarioService usuarioService,
            IMediatorHandler mediatorHandler, IRendaFixaHistoricoRepository rendaFixaHistoricoRepository)
        {
            _repository = clienteRepository;
            _usuarioService = usuarioService;
            _mediator = mediatorHandler;
            _historicoRepository = rendaFixaHistoricoRepository;
        }

        public async Task<ValidationResult> Handle(UpdateRendaFixaAdminCommand message, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(message.Id);
            if (entity == null)
            {
                AdicionarErro("Produto não encontrado");
                return ValidationResult;
            }
            entity.UsuarioCadastroId = _usuarioService.GetUserId();
            entity.EmailAdministrador = message.EmailAdministrador;
            entity.ValorMinimo = message.ValorMinimo;
            entity.ValorUnitario = message.ValorUnitario;
            var quantidadeAnterior = entity.QuantidadeCotasDisponivel;
            if (message.QuantidadeCotasDisponivel != null) entity.QuantidadeCotasDisponivel = message.QuantidadeCotasDisponivel;

            if (!entity.EhValido()) return entity.RetornaValidationResult();

            await _repository.Update(entity);
            await _historicoRepository.Add(new RendaFixaHistorico(entity.UsuarioCadastroId, 
                entity.Id, entity.Nome, entity.ValorMinimo, entity.ValorUnitario));
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
