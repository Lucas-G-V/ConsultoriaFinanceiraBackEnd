using XpInc.Bus.Configuration;
using XpInc.RendaFixa.API.Services;
namespace XpInc.RendaFixa.API.Configuration
{
    public static class BusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration?["MessageBusConnection:MessageBus"])
                .AddHostedService<UpdateQuantidadeDisponivelProdutoIntegrationEventHandler>();
        }
    }
}
