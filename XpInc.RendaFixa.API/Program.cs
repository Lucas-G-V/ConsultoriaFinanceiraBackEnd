using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using XpInc.Cache.Configuration;
using XpInc.ApiConfig.Config;
using XpInc.RendaFixa.API.Configuration;
using XpInc.RendaFixa.API.Data.Context;
using XpInc.Email.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RendaFixaContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddAuthorizationConfiguration(builder.Configuration);
builder.Services.AddMessageBusConfiguration(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.RegisterServices();
builder.Services.AddMemoryCacheConfig(builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddEmailSettings(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
