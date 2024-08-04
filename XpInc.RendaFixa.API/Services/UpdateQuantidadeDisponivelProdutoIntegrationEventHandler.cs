using EasyNetQ;
using FluentValidation.Results;
using XpInc.Bus;
using XpInc.Core.Domain.IntegrantionModels;
using XpInc.Core.MediatorHandler;
using XpInc.Core.Messages.IntegrationMessages;
using XpInc.RendaFixa.API.Application.Commands;

namespace XpInc.RendaFixa.API.Services
{
    public class UpdateQuantidadeDisponivelProdutoIntegrationEventHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public UpdateQuantidadeDisponivelProdutoIntegrationEventHandler(
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
            _bus.RespondAsync<UpdateQuantidadeDisponivelProdutoIntegrationEvent, ResponseMessage>(async request =>
                await RegistrarAlteracaoQuantidadeAcao(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        private async Task<ResponseMessage> RegistrarAlteracaoQuantidadeAcao(UpdateQuantidadeDisponivelProdutoIntegrationEvent message)
        {
            var clienteCommand = new UpdateRendaFixaCommand(message.IdProduto, null, message.QuantidadeCotas);
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
