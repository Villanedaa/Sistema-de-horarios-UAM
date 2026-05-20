using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaHorarios.Datos.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisponibilidadesDocentes",
                columns: table => new
                {
                    IdDisponibilidadDocente = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdDocente = table.Column<int>(type: "INTEGER", nullable: false),
                    Dia = table.Column<string>(type: "TEXT", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Disponible = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisponibilidadesDocentes", x => x.IdDisponibilidadDocente);
                });

            migrationBuilder.CreateTable(
                name: "Docentes",
                columns: table => new
                {
                    IdDocente = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreCompleto = table.Column<string>(type: "TEXT", nullable: false),
                    Identificacion = table.Column<string>(type: "TEXT", nullable: false),
                    CorreoInstitucional = table.Column<string>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docentes", x => x.IdDocente);
                });

            migrationBuilder.CreateTable(
                name: "FranjasHorarias",
                columns: table => new
                {
                    IdFranjaHoraria = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiaSemana = table.Column<string>(type: "TEXT", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Activa = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FranjasHorarias", x => x.IdFranjaHoraria);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    IdMateria = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Creditos = table.Column<int>(type: "INTEGER", nullable: false),
                    IntensidadHorariaSemanal = table.Column<int>(type: "INTEGER", nullable: false),
                    Semestre = table.Column<int>(type: "INTEGER", nullable: false),
                    CantidadGrupos = table.Column<int>(type: "INTEGER", nullable: false),
                    Activa = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.IdMateria);
                });

            migrationBuilder.CreateTable(
                name: "PlanesAcademicos",
                columns: table => new
                {
                    IdPlanAcademico = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Programa = table.Column<string>(type: "TEXT", nullable: false),
                    Anio = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesAcademicos", x => x.IdPlanAcademico);
                });

            migrationBuilder.CreateTable(
                name: "Prerrequisitos",
                columns: table => new
                {
                    IdPrerrequisito = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdMateria = table.Column<int>(type: "INTEGER", nullable: false),
                    IdMateriaPrerrequisito = table.Column<int>(type: "INTEGER", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prerrequisitos", x => x.IdPrerrequisito);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "DocenteMaterias",
                columns: table => new
                {
                    IdDocenteMateria = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdDocente = table.Column<int>(type: "INTEGER", nullable: false),
                    IdMateria = table.Column<int>(type: "INTEGER", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocenteMaterias", x => x.IdDocenteMateria);
                    table.ForeignKey(
                        name: "FK_DocenteMaterias_Docentes_IdDocente",
                        column: x => x.IdDocente,
                        principalTable: "Docentes",
                        principalColumn: "IdDocente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocenteMaterias_Materias_IdMateria",
                        column: x => x.IdMateria,
                        principalTable: "Materias",
                        principalColumn: "IdMateria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    IdGrupo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Jornada = table.Column<string>(type: "TEXT", nullable: false),
                    TipoGrupo = table.Column<string>(type: "TEXT", nullable: false),
                    NumeroSemestre = table.Column<int>(type: "INTEGER", nullable: false),
                    CantidadEstudiantes = table.Column<int>(type: "INTEGER", nullable: false),
                    IdPlanAcademico = table.Column<int>(type: "INTEGER", nullable: false),
                    Materia = table.Column<string>(type: "TEXT", nullable: false),
                    Dias = table.Column<string>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.IdGrupo);
                    table.ForeignKey(
                        name: "FK_Grupos_PlanesAcademicos_IdPlanAcademico",
                        column: x => x.IdPlanAcademico,
                        principalTable: "PlanesAcademicos",
                        principalColumn: "IdPlanAcademico",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SemestresPlan",
                columns: table => new
                {
                    IdSemestrePlan = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroSemestre = table.Column<int>(type: "INTEGER", nullable: false),
                    IdPlanAcademico = table.Column<int>(type: "INTEGER", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreCompleto = table.Column<string>(type: "TEXT", nullable: false),
                    Cedula = table.Column<string>(type: "TEXT", nullable: false),
                    CorreoInstitucional = table.Column<string>(type: "TEXT", nullable: false),
                    ContrasenaHash = table.Column<string>(type: "TEXT", nullable: false),
                    IdRol = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    Celular = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    IdHorario = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdGrupo = table.Column<int>(type: "INTEGER", nullable: false),
                    IdMateria = table.Column<int>(type: "INTEGER", nullable: false),
                    IdDocente = table.Column<int>(type: "INTEGER", nullable: false),
                    IdFranjaHoraria = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacion = table.Column<string>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.IdHorario);
                    table.ForeignKey(
                        name: "FK_Horarios_Docentes_IdDocente",
                        column: x => x.IdDocente,
                        principalTable: "Docentes",
                        principalColumn: "IdDocente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Horarios_FranjasHorarias_IdFranjaHoraria",
                        column: x => x.IdFranjaHoraria,
                        principalTable: "FranjasHorarias",
                        principalColumn: "IdFranjaHoraria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Horarios_Grupos_IdGrupo",
                        column: x => x.IdGrupo,
                        principalTable: "Grupos",
                        principalColumn: "IdGrupo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Horarios_Materias_IdMateria",
                        column: x => x.IdMateria,
                        principalTable: "Materias",
                        principalColumn: "IdMateria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MateriasPlan",
                columns: table => new
                {
                    IdMateriaPlan = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdSemestrePlan = table.Column<int>(type: "INTEGER", nullable: false),
                    IdMateria = table.Column<int>(type: "INTEGER", nullable: false)
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
                });

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
                name: "IX_DocenteMaterias_IdDocente",
                table: "DocenteMaterias",
                column: "IdDocente");

            migrationBuilder.CreateIndex(
                name: "IX_DocenteMaterias_IdMateria",
                table: "DocenteMaterias",
                column: "IdMateria");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_IdPlanAcademico",
                table: "Grupos",
                column: "IdPlanAcademico");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_IdDocente",
                table: "Horarios",
                column: "IdDocente");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_IdFranjaHoraria",
                table: "Horarios",
                column: "IdFranjaHoraria");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_IdGrupo",
                table: "Horarios",
                column: "IdGrupo");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_IdMateria",
                table: "Horarios",
                column: "IdMateria");

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

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisponibilidadesDocentes");

            migrationBuilder.DropTable(
                name: "DocenteMaterias");

            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "MateriasPlan");

            migrationBuilder.DropTable(
                name: "Prerrequisitos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Docentes");

            migrationBuilder.DropTable(
                name: "FranjasHorarias");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "SemestresPlan");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "PlanesAcademicos");
        }
    }
}
