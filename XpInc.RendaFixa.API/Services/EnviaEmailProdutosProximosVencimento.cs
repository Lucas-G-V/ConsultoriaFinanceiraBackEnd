using XpInc.Bus;
using XpInc.Core.Domain.IntegrantionModels;
using XpInc.Core.MediatorHandler;
using XpInc.Core.Messages.IntegrationMessages;
using XpInc.Email;
using XpInc.RendaFixa.API.Application.Commands;

namespace XpInc.RendaFixa.API.Services
{
    public class EnviaEmailProdutosProximosVencimento : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public EnviaEmailProdutosProximosVencimento(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(EnviaEmailProdutoProximoVencimento, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void EnviaEmailProdutoProximoVencimento(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var email = scope.ServiceProvider.GetRequiredService<IEmailService>();


                await email.EnviaEmail(
                    "lucasgvettorazzo@hotmail.com",
                    "Lucas",
                    "lucasgvettorazzo1505@gmail.com",
                    "Consultoria Financeira",
                    "Produtos Vencendo",
                    "Estes produtos estão prestes a vencer",
                    "<div>Estes produtos estão prestes a vencer</div>"
                );
            }
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(stoppingToken);
        }
    }
}
