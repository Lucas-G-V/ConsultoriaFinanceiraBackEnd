using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XpInc.RendaFixa.API.Migrations
{
    /// <inheritdoc />
    public partial class CotasDisponiveis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeCotasDisponivel",
                table: "RendaFixaProduto",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeCotasInicial",
                table: "RendaFixaProduto",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeCotasDisponivel",
                table: "RendaFixaProduto");

            migrationBuilder.DropColumn(
                name: "QuantidadeCotasInicial",
                table: "RendaFixaProduto");
        }
    }
}
