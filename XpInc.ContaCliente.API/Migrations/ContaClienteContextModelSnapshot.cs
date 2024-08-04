﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XpInc.ContaCliente.API.Data.Contexts;

#nullable disable

namespace XpInc.ContaCliente.API.Migrations
{
    [DbContext(typeof(ContaClienteContext))]
    partial class ContaClienteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("XpInc.ContaCliente.API.Models.Entities.ContaClienteSaldo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime");

                    b.Property<string>("NomeCompleto")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<double>("SaldoDisponivel")
                        .HasColumnType("float");

                    b.Property<string>("TelefoneCelular")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<double>("TotalInvestido")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ContaClienteSaldo");
                });

            modelBuilder.Entity("XpInc.Core.Messages.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ContaClienteSaldoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("ContaClienteSaldoId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("XpInc.Core.Messages.Event", b =>
                {
                    b.HasOne("XpInc.ContaCliente.API.Models.Entities.ContaClienteSaldo", null)
                        .WithMany("Notificacoes")
                        .HasForeignKey("ContaClienteSaldoId");
                });

            modelBuilder.Entity("XpInc.ContaCliente.API.Models.Entities.ContaClienteSaldo", b =>
                {
                    b.Navigation("Notificacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
