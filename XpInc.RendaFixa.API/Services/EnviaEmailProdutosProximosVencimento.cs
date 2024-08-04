using XpInc.Bus;
using XpInc.Core.Domain.IntegrantionModels;
using XpInc.Core.MediatorHandler;
using XpInc.Core.Messages.IntegrationMessages;
using XpInc.Email;
using XpInc.RendaFixa.API.Application.Commands;
using XpInc.RendaFixa.API.Application.Queries;

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
            _timer = new Timer(EnviaEmailProdutoProximoVencimento, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void EnviaEmailProdutoProximoVencimento(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                var query = new GetRendaFixaProximaVencimentoQuery(DateTime.Now.Date);
                var rendaFixaList = await mediator.BuscarQuery(query);

                var emailsAdministradores = rendaFixaList
                    .Where(p => p.EmailAdministrador != null)
                    .Select(p => p.EmailAdministrador)
                    .Distinct()
                    .ToList();

                var textoBody = "Estes produtos estão prestes a vencer: " +
                                string.Join(";", rendaFixaList.Select(p => p.Nome));

                foreach (var admin in emailsAdministradores)
                {
                    //await emailService.EnviaEmail(
                    //    admin,
                    //    "Administrador",
                    //    "lucasgvettorazzo1505@gmail.com",
                    //    "Consultoria Financeira",
                    //    "Produtos Vencendo",
                    //    textoBody,
                    //    $"<div>{textoBody}</div>"
                    //);
                }
            }
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(stoppingToken);
        }
    }
}
