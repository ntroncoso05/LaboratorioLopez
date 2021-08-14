using Microsoft.EntityFrameworkCore.Migrations;

namespace LaboratorioLopez.Migrations
{
    public partial class AddPagoUpdate1ToPagoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacturaDetalleId",
                table: "Pago");

            migrationBuilder.AddColumn<bool>(
                name: "EstadoPago",
                table: "Pago",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FacturaId",
                table: "Pago",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LineaFactura",
                table: "Pago",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoPago",
                table: "Pago");

            migrationBuilder.DropColumn(
                name: "FacturaId",
                table: "Pago");

            migrationBuilder.DropColumn(
                name: "LineaFactura",
                table: "Pago");

            migrationBuilder.AddColumn<int>(
                name: "FacturaDetalleId",
                table: "Pago",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
