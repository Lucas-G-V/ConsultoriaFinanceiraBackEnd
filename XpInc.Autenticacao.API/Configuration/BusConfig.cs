using System.Xml.Linq;
using XpInc.Bus.Configuration;

namespace XpInc.Autenticacao.API.Configuration
{
    public  static class BusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration?["MessageBusConnection:MessageBus"]);
        }
    }
}
