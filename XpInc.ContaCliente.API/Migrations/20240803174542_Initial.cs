using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XpInc.ContaCliente.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContaClienteSaldo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeCompleto = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    TelefoneCelular = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    CPF = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    SaldoDisponivel = table.Column<double>(type: "float", nullable: false),
                    TotalInvestido = table.Column<double>(type: "float", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaClienteSaldo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    ContaClienteSaldoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MessageType = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    AggregateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_ContaClienteSaldo_ContaClienteSaldoId",
                        column: x => x.ContaClienteSaldoId,
                        principalTable: "ContaClienteSaldo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_ContaClienteSaldoId",
                table: "Event",
                column: "ContaClienteSaldoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "ContaClienteSaldo");
        }
    }
}
