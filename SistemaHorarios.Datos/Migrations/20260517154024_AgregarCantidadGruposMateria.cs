using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaHorarios.Datos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCantidadGruposMateria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadGrupos",
                table: "Materias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadGrupos",
                table: "Materias");
        }
    }
}
