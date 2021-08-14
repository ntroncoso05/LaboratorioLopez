using Microsoft.EntityFrameworkCore.Migrations;

namespace LaboratorioLopez.Migrations
{
    public partial class UpdateResultadoAnalisiToEntradaExamenModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "resultadoAnalisis",
                table: "EntradaExamen",
                newName: "ResultadoAnalisis");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResultadoAnalisis",
                table: "EntradaExamen",
                newName: "resultadoAnalisis");
        }
    }
}
