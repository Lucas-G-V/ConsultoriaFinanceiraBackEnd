using FluentValidation.Results;
using MediatR;
using XpInc.ContaCliente.API.Application.Commands;
using XpInc.ContaCliente.API.Application.Commands.Handler;
using XpInc.ContaCliente.API.Application.Queries;
using XpInc.ContaCliente.API.Application.Queries.Handler;
using XpInc.ContaCliente.API.Data.Contexts;
using XpInc.ContaCliente.API.Data.Repositories;
using XpInc.ContaCliente.API.Models.Entities;
using XpInc.ContaCliente.API.Models.Interfaces;
using XpInc.Core.MediatorHandler;

namespace XpInc.ContaCliente.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IContaClienteSaldoRepository, ContaClienteSaldoRepository>();
            services.AddScoped<ContaClienteContext>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<CriarClienteCommand, ValidationResult>, ClienteCommandHandler>();

            services.AddScoped<IRequestHandler<SaldoClienteQuery, ContaClienteSaldo>, SaldoClienteQueryHandler>();

        }
    }
}
