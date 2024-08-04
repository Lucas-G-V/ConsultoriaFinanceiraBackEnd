using FluentValidation.Results;
using XpInc.Bus;
using XpInc.ContaCliente.API.Application.Commands;
using XpInc.Core.Domain.IntegrantionModels;
using XpInc.Core.MediatorHandler;
using XpInc.Core.Messages.IntegrationMessages;

namespace XpInc.ContaCliente.API.Services
{
    public class CreateClienteIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CreateClienteIntegrationHandler(
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
            _bus.RespondAsync<CreateClienteIntegrationEvent, ResponseMessage>(async request =>
                await RegistrarCliente(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        private async Task<ResponseMessage> RegistrarCliente(CreateClienteIntegrationEvent message)
        {
            var clienteCommand = new CriarClienteCommand(message.NomeCompleto, message.TelefoneCelular, message.Cpf, message.Id);
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
