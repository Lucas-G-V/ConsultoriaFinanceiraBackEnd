using XpInc.Autenticacao.API.Configuration;
using XpInc.Logs;
using XpInc.Cache.Configuration;

namespace XpInc.Autenticacao.API
{
    public class Program
    {
        public  static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddIdentityConfiguration(builder.Configuration);
            builder.Services.AddMessageBusConfiguration(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCacheConfig(builder.Configuration.GetConnectionString("Redis"));
            builder.Services.AddLogHealthCheckConfig(builder.Configuration);


            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseHealthChecksConfiguration();

            await app.InitializeSeedData();

            app.Run();
        }
    }
}



