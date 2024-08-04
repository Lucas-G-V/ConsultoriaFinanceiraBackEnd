using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XpInc.RendaFixa.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoHistoricoDeValores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAdministrador",
                table: "RendaFixaProduto",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RendaFixaHistoricoId",
                table: "Event",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RendaFixaHistorico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    ValorMinimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RendaFixaHistorico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RendaFixaHistorico_RendaFixaProduto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "RendaFixaProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_RendaFixaHistoricoId",
                table: "Event",
                column: "RendaFixaHistoricoId");

            migrationBuilder.CreateIndex(
                name: "IX_RendaFixaHistorico_ProdutoId",
                table: "RendaFixaHistorico",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_RendaFixaHistorico_RendaFixaHistoricoId",
                table: "Event",
                column: "RendaFixaHistoricoId",
                principalTable: "RendaFixaHistorico",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_RendaFixaHistorico_RendaFixaHistoricoId",
                table: "Event");

            migrationBuilder.DropTable(
                name: "RendaFixaHistorico");

            migrationBuilder.DropIndex(
                name: "IX_Event_RendaFixaHistoricoId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EmailAdministrador",
                table: "RendaFixaProduto");

            migrationBuilder.DropColumn(
                name: "RendaFixaHistoricoId",
                table: "Event");
        }
    }
}
