using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using XpInc.ContaCliente.API.Configuration;
using XpInc.ContaCliente.API.Data.Contexts;
using XpInc.Cache.Configuration;
using XpInc.ApiConfig.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ContaClienteContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddAuthorizationConfiguration(builder.Configuration);
builder.Services.AddMessageBusConfiguration(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.RegisterServices();
builder.Services.AddMemoryCacheConfig(builder.Configuration.GetConnectionString("Redis"));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
