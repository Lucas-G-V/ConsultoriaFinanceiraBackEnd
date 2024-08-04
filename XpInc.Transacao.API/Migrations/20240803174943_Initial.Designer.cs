﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XpInc.Transacao.API.Data.Contexts;

#nullable disable

namespace XpInc.Transacao.API.Migrations
{
    [DbContext(typeof(TransacaoContext))]
    [Migration("20240803174943_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("XpInc.Core.Messages.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("TransacaoClienteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TransacaoClienteId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("XpInc.Transacao.API.Models.Entities.TransacaoCliente", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<Guid>("ClienteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DataTransacao")
                        .HasColumnType("datetime");

                    b.Property<string>("NomeProduto")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<Guid?>("ProdutoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("Quantidade")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("ValorUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("TransacaoCliente");
                });

            modelBuilder.Entity("XpInc.Core.Messages.Event", b =>
                {
                    b.HasOne("XpInc.Transacao.API.Models.Entities.TransacaoCliente", null)
                        .WithMany("Notificacoes")
                        .HasForeignKey("TransacaoClienteId");
                });

            modelBuilder.Entity("XpInc.Transacao.API.Models.Entities.TransacaoCliente", b =>
                {
                    b.Navigation("Notificacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
