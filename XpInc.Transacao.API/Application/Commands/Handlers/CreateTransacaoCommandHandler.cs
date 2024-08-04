using AutoMapper;
using EasyNetQ;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using XpInc.ApiConfig.Services;
using XpInc.Bus;
using XpInc.Core.Domain.IntegrantionModels;
using XpInc.Core.Messages;
using XpInc.Core.Messages.IntegrationMessages;
using XpInc.Transacao.API.Models.Entities;
using XpInc.Transacao.API.Models.Enums;
using XpInc.Transacao.API.Models.Interfaces;

namespace XpInc.Transacao.API.Application.Commands.Handlers
{
    public class CreateTransacaoCommandHandler : CommandHandler,
         IRequestHandler<CreateTransacaoCommand, ValidationResult>
    {
        private readonly ITransacaoRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        private readonly IMessageBus _bus;

        public CreateTransacaoCommandHandler(ITransacaoRepository repository, IMapper mapper, IUsuarioService usuarioService,
            IMessageBus bus)
        {
            _repository = repository;
            _mapper = mapper;
            _usuarioService = usuarioService;
            _bus = bus;
        }

        public async Task<ValidationResult> Handle(CreateTransacaoCommand message, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TransacaoCliente>(message);
            entity.ClienteId = _usuarioService.GetUserId();
            entity.AtribuiValoresDasTransacoesParaConta();
            if (!entity.EhValido()) return entity.RetornaValidationResult();
            var historicoTransacao = await _repository.GetByIdCliente(entity.ClienteId);
            if (!await ValidaTransacao(entity, historicoTransacao))
            {
                AdicionarErro("Transação Inválida");
                return ValidationResult;
            }
            var alteraDadosDeCotas = await AlteraOValorDeCotasDisponiveis(entity);
            if (!alteraDadosDeCotas.IsValid) return alteraDadosDeCotas;
            await _repository.Add(entity);
            var result = await PersistirDados(_repository.UnitOfWork);
            await EnviaEventoQuandoTransacaoEAprovada(entity, historicoTransacao);
            return result;
        }

        private async Task<ValidationResult> AlteraOValorDeCotasDisponiveis(TransacaoCliente transacao)
        {
            if(transacao.Quantidade != null ||
                (transacao.Tipo == TipoTransacao.Compra || transacao.Tipo == TipoTransacao.Venda))
            {
                var debitar = (transacao.Tipo == TipoTransacao.Compra) ? transacao.Quantidade.Value : -1 * transacao.Quantidade.Value;
                var result = await _bus.RequestAsync<UpdateQuantidadeDisponivelProdutoIntegrationEvent, ResponseMessage>(
                    new UpdateQuantidadeDisponivelProdutoIntegrationEvent(transacao.ProdutoId.Value, (int)debitar));
                if (result.ValidationResult.IsValid) transacao.Status = StatusTransacao.Concluida;
                return result.ValidationResult;
            }
            return new ValidationResult();
        }


        private async Task<bool> ValidaTransacao(TransacaoCliente transacao, IEnumerable<TransacaoCliente> historicoTransacao)
        {
            
            switch (transacao.Tipo)
            {
                case TipoTransacao.Compra:
                    return await ValidaTransacaoCompra(transacao, historicoTransacao);
                case TipoTransacao.Venda:
                    return await ValidaTransacaoVenda(transacao, historicoTransacao);
                case TipoTransacao.Deposito:
                    return await ValidaTransacaoDeposito(transacao);
                case TipoTransacao.Saque:
                    return await ValidaTransacaoSaque(transacao, historicoTransacao);
                default:
                    return false;
            }
            
        }

        private async Task<bool> ValidaTransacaoCompra(TransacaoCliente transacao, IEnumerable<TransacaoCliente> historico)
        {
            var validadeSaldoConta = transacao.VerificaSeTransacaoDeDebitoEhValida(historico, transacao);
            //disparaEventoParaRetirarCotasDisponiveisEValidarSePodeComprar()
            return validadeSaldoConta;
        }
        private async Task<bool> ValidaTransacaoVenda(TransacaoCliente transacao, IEnumerable<TransacaoCliente> historico)
        {
            var validadeSaldoConta = transacao.VerificaSeTransacaoDeDebitoEhValida(historico, transacao);
            var validaSeQuantidadeEhValida = transacao.VerificaSeTransacaoDeVendaEhValida(historico, transacao);
            //disparaEventoParaAdicionarCotasDisponiveisEValidarSePodeVender()
            return validadeSaldoConta && validaSeQuantidadeEhValida;
        }
        private async Task<bool> ValidaTransacaoDeposito(TransacaoCliente transacao)
        {
            transacao.Status = StatusTransacao.Concluida;
            return true;
        }
        private async Task<bool> ValidaTransacaoSaque(TransacaoCliente transacao, IEnumerable<TransacaoCliente> historico)
        {
            var validadeSaldoConta = transacao.VerificaSeTransacaoDeDebitoEhValida(historico, transacao);
            if (validadeSaldoConta) transacao.Status = StatusTransacao.Concluida;
            return validadeSaldoConta;
        }


        private async Task EnviaEventoQuandoTransacaoEAprovada(TransacaoCliente transacao, IEnumerable<TransacaoCliente> historico)
        {
            if (transacao.Status == StatusTransacao.Concluida)
            {
                var (saldoDisponivel, valorInvestido) = transacao.CalculaSaldoEValorInvestido(historico, transacao);
                var evento = new AlteraSaldoClienteIntegrantionEvent(transacao.ClienteId, valorInvestido, saldoDisponivel);
                await _bus.PublishAsync(evento);
            }
        }
    }
}
