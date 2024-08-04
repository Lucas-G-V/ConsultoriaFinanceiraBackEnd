using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XpInc.ApiConfig.Config;
using XpInc.Autenticacao.API.Data.Contexts;

namespace XpInc.Autenticacao.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorizationConfiguration(configuration);

            return services;
        }
    }
}
