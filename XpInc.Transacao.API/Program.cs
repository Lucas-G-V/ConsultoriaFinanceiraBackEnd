using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using XpInc.Cache.Configuration;
using XpInc.ApiConfig.Config;
using XpInc.Transacao.API.Data.Contexts;
using XpInc.Transacao.API.Configuration;
using XpInc.Logs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransacaoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddAuthorizationConfiguration(builder.Configuration);
builder.Services.AddMessageBusConfiguration(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.RegisterServices();
builder.Services.AddMemoryCacheConfig(builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddLogHealthCheckConfig(builder.Configuration);
builder.Services.AddAutoMapperConfiguration();


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


app.Run();
