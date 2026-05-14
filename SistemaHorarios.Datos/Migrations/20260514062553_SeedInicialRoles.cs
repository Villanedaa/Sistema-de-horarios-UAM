using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaHorarios.Datos.Migrations
{
    /// <inheritdoc />
    public partial class SeedInicialRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "IdRol", "Activo", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Administrador" },
                    { 2, true, "Coordinador" },
                    { 3, true, "Docente" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "IdRol",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "IdRol",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "IdRol",
                keyValue: 3);
        }
    }
}
