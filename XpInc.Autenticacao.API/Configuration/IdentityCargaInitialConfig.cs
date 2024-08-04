using Microsoft.AspNetCore.Identity;
using XpInc.Autenticacao.API.Data.InitializeData;

namespace XpInc.Autenticacao.API.Configuration
{
    public static class IdentityCargaInitialConfig
    {
        public static async Task InitializeSeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedDataRoles.Initialize(services, userManager, roleManager);
            }
        }
    }
}
