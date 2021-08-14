using Microsoft.EntityFrameworkCore.Migrations;

namespace LaboratorioLopez.Migrations
{
    public partial class ActualizarTablaExamenParametro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "ExamenParametros");

            migrationBuilder.DropColumn(
                name: "Seleccionado",
                table: "ExamenParametros");

            migrationBuilder.AddColumn<int>(
                name: "ParametroId",
                table: "ExamenParametros",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParametroId",
                table: "ExamenParametros");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "ExamenParametros",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Seleccionado",
                table: "ExamenParametros",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
