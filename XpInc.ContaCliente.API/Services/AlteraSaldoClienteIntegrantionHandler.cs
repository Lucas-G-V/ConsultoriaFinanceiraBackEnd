using FluentValidation.Results;
using XpInc.Bus;
using XpInc.ContaCliente.API.Application.Commands;
using XpInc.Core.Domain.IntegrantionModels;
using XpInc.Core.MediatorHandler;
using XpInc.Core.Messages.IntegrationMessages;

namespace XpInc.ContaCliente.API.Services
{
    public class AlteraSaldoClienteIntegrantionHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public AlteraSaldoClienteIntegrantionHandler(
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
            _bus.SubscribeAsync<AlteraSaldoClienteIntegrantionEvent>("altera-saldo-cliente", async message =>
                await AlteraSaldoCliente(message));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        private async Task AlteraSaldoCliente(AlteraSaldoClienteIntegrantionEvent message)
        {
            var clienteCommand = new AlteraSaldoClienteCommand(message.IdCliente, message.SaldoConta, message.ValorInvestido);
            ValidationResult sucesso;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                sucesso = await mediator.EnviarComando(clienteCommand);
            }
        }
    }
}
