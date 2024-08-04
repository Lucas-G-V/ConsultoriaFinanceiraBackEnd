using FluentValidation.Results;
using MediatR;
using XpInc.Core.MediatorHandler;
using XpInc.Transacao.API.Application.Commands;
using XpInc.Transacao.API.Application.Commands.Handlers;
using XpInc.Transacao.API.Application.Queries;
using XpInc.Transacao.API.Application.Queries.Handlers;
using XpInc.Transacao.API.Data.Contexts;
using XpInc.Transacao.API.Data.Repositories;
using XpInc.Transacao.API.Models.Entities;
using XpInc.Transacao.API.Models.Interfaces;

namespace XpInc.Transacao.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();
            services.AddScoped<TransacaoContext>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IRequestHandler<CreateTransacaoCommand, ValidationResult>, CreateTransacaoCommandHandler>();
            services.AddScoped<IRequestHandler<AlteraStatusTransacaoCommand, ValidationResult>, AlteraStatusTransacaoCommandHandler>();

            services.AddScoped<IRequestHandler<BuscaTransacoesClientesQuery, IEnumerable<TransacaoCliente>>, BuscaTransacoesClientesQueryHandler>();
        }
    }
}
