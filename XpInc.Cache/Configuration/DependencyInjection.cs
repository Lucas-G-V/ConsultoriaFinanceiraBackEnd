using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpInc.Cache.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMemoryCacheConfig(this IServiceCollection services, string connection)
        {
            if (string.IsNullOrEmpty(connection)) throw new ArgumentNullException();
            services.AddSingleton<IMemoryCacheService>(new MemoryCacheService(connection));

            return services;
        }
    }
}
