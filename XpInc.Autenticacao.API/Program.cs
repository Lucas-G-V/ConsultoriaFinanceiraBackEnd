using XpInc.Autenticacao.API.Configuration;

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


            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            await app.InitializeSeedData();

            app.Run();
        }
    }
}



