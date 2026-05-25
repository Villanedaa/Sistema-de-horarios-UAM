using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaHorarios.Datos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEstadoHorario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Horarios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MotivoRechazo",
                table: "Horarios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "MotivoRechazo",
                table: "Horarios");
        }
    }
}
