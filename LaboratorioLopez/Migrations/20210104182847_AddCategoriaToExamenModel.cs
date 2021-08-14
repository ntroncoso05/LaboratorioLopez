using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaboratorioLopez.Migrations
{
    public partial class AddCategoriaToExamenModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Precio",
                table: "Examen",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Examen",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Examen");


            migrationBuilder.AlterColumn<float>(
                name: "Precio",
                table: "Examen",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
