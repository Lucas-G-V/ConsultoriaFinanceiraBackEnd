using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using XpInc.Core.Data;
using XpInc.Core.Domain;
using XpInc.Core.MediatorHandler;
using XpInc.RendaFixa.API.Models.Entities;

namespace XpInc.RendaFixa.API.Data.Context
{
    public class RendaFixaContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;
        public RendaFixaContext(DbContextOptions<RendaFixaContext> options, IMediatorHandler mediatorHandler)
        : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<RendaFixaProduto> RendaFixaProduto { get; set; }
        public DbSet<RendaFixaHistorico> RendaFixaHistorico { get; set; }

        public async Task<bool> Commit()
        {
            var sucesso = await base.SaveChangesAsync() > 0;
            return sucesso;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        property.SetMaxLength(500);
                        property.SetColumnType("varchar(500)");
                    }
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetColumnType("datetime");
                    }
                }
            }

            modelBuilder.Entity<RendaFixaHistorico>(entity =>
            {
                entity.HasOne<RendaFixaProduto>()
                    .WithMany()
                    .HasForeignKey(e => e.ProdutoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.PublicarEvento(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
