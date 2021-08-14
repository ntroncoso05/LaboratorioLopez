using Microsoft.EntityFrameworkCore.Migrations;

namespace LaboratorioLopez.Migrations
{
    public partial class AddPagoUpdate3ToPagoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ExamenPorcentaje",
                table: "Pago",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ExamenPrecio",
                table: "Pago",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamenPorcentaje",
                table: "Pago");

            migrationBuilder.DropColumn(
                name: "ExamenPrecio",
                table: "Pago");
        }
    }
}
