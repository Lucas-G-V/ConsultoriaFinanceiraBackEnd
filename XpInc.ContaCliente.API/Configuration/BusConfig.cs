using XpInc.ContaCliente.API.Services;
using XpInc.Bus.Configuration;
namespace XpInc.ContaCliente.API.Configuration
{
    public static class BusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration?["MessageBusConnection:MessageBus"])
                .AddHostedService<CreateClienteIntegrationHandler>()
                .AddHostedService<AlteraSaldoClienteIntegrantionHandler>();
        }
    }
}
