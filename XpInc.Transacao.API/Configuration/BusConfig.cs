using XpInc.Bus.Configuration;
using XpInc.Transacao.API.Services;
namespace XpInc.Transacao.API.Configuration
{
    public static class BusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration?["MessageBusConnection:MessageBus"])
                .AddHostedService<AlteraStatusTransacaoIntegrationEventHandler>();
        }
    }
}
