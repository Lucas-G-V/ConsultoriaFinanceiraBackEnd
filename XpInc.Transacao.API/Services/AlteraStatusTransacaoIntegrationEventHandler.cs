using FluentValidation.Results;
using XpInc.Bus;
using XpInc.Core.Domain.IntegrantionModels;
using XpInc.Core.MediatorHandler;
using XpInc.Core.Messages.IntegrationMessages;
using XpInc.Transacao.API.Application.Commands;
using XpInc.Transacao.API.Models.Enums;

namespace XpInc.Transacao.API.Services
{
    public class AlteraStatusTransacaoIntegrationEventHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public AlteraStatusTransacaoIntegrationEventHandler(
                            IServiceProvider serviceProvider,
                            IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            return Task.CompletedTask;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<AlteraStatusTransacaoIntegrationEvent, ResponseMessage>(async request =>
                await AlteraStatusTransacao(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        private async Task<ResponseMessage> AlteraStatusTransacao(AlteraStatusTransacaoIntegrationEvent message)
        {
            var situacao = StatusTransacao.Falha;
            if (message.Aprovada)
            {
                situacao = StatusTransacao.Concluida;
            }
            var clienteCommand = new AlteraStatusTransacaoCommand(situacao, message.Id);
            ValidationResult sucesso;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                sucesso = await mediator.EnviarComando(clienteCommand);
            }

            return new ResponseMessage(sucesso);
        }
    }
}
