using FluentValidation.Results;
using MediatR;
using XpInc.Core.MediatorHandler;
using XpInc.Email;
using XpInc.RendaFixa.API.Application.Commands;
using XpInc.RendaFixa.API.Application.Commands.Handlers;
using XpInc.RendaFixa.API.Application.Queries;
using XpInc.RendaFixa.API.Application.Queries.Handlers;
using XpInc.RendaFixa.API.Data.Context;
using XpInc.RendaFixa.API.Data.Repositories;
using XpInc.RendaFixa.API.Models.DTO.Response;
using XpInc.RendaFixa.API.Models.Entities;
using XpInc.RendaFixa.API.Models.Interfaces;

namespace XpInc.RendaFixa.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IRendaFixaRepository, RendaFixaRepository>();
            services.AddScoped<IRendaFixaHistoricoRepository, RendaFixaHistoricoRepository>();
            services.AddScoped<RendaFixaContext>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IRequestHandler<CreateRendaFixaCommand, ValidationResult>, CreateRendaFixaCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateRendaFixaCommand, ValidationResult>, UpdateRendaFixaCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateRendaFixaAdminCommand, ValidationResult>, UpdateRendaFixaAdminCommandHandler>();
            services.AddScoped<IRequestHandler<GetAllRendaFixaQuery, IEnumerable<RendaFixaProduto>>, GetAllRendaFixaQueryHandler>();
            services.AddScoped<IRequestHandler<GetRendaFixaById, RendaFixaDetalhadaResponse>, GetRendaFixaByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetRendaFixaProximaVencimentoQuery, 
                IEnumerable<RendaFixaProduto>>, GetRendaFixaProximaVencimentoQueryHandler>();

        }
    }
}
