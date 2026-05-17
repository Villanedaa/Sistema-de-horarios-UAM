using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaHorarios.Datos.Migrations
{
    /// <inheritdoc />
    public partial class CrearModuloPlanAcademico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPrerrequisito",
                table: "Materias");

            migrationBuilder.CreateTable(
                name: "PlanesAcademicos",
                columns: table => new
                {
                    IdPlanAcademico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Programa = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesAcademicos", x => x.IdPlanAcademico);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SemestresPlan",
                columns: table => new
                {
                    IdSemestrePlan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroSemestre = table.Column<int>(type: "int", nullable: false),
                    IdPlanAcademico = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemestresPlan", x => x.IdSemestrePlan);
                    table.ForeignKey(
                        name: "FK_SemestresPlan_PlanesAcademicos_IdPlanAcademico",
                        column: x => x.IdPlanAcademico,
                        principalTable: "PlanesAcademicos",
                        principalColumn: "IdPlanAcademico",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MateriasPlan",
                columns: table => new
                {
                    IdMateriaPlan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdSemestrePlan = table.Column<int>(type: "int", nullable: false),
                    IdMateria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriasPlan", x => x.IdMateriaPlan);
                    table.ForeignKey(
                        name: "FK_MateriasPlan_Materias_IdMateria",
                        column: x => x.IdMateria,
                        principalTable: "Materias",
                        principalColumn: "IdMateria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MateriasPlan_SemestresPlan_IdSemestrePlan",
                        column: x => x.IdSemestrePlan,
                        principalTable: "SemestresPlan",
                        principalColumn: "IdSemestrePlan",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "IdRol", "Activo", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "", "Administrador" },
                    { 2, true, "", "Coordinador" },
                    { 3, true, "", "Docente" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MateriasPlan_IdMateria",
                table: "MateriasPlan",
                column: "IdMateria");

            migrationBuilder.CreateIndex(
                name: "IX_MateriasPlan_IdSemestrePlan_IdMateria",
                table: "MateriasPlan",
                columns: new[] { "IdSemestrePlan", "IdMateria" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemestresPlan_IdPlanAcademico_NumeroSemestre",
                table: "SemestresPlan",
                columns: new[] { "IdPlanAcademico", "NumeroSemestre" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MateriasPlan");

            migrationBuilder.DropTable(
                name: "SemestresPlan");

            migrationBuilder.DropTable(
                name: "PlanesAcademicos");

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

            migrationBuilder.AddColumn<int>(
                name: "IdPrerrequisito",
                table: "Materias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
