using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Logica.Negocio.Dashboard.Interfaces;
using SistemaHorarios.Modelos.DTOs.Dashboard;

namespace SistemaHorarios.Logica.Negocio.Dashboard.Servicios;

public class DashboardService : IDashboardService
{
    private readonly SistemaHorariosDbContext _context;

    public DashboardService(SistemaHorariosDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardResumenDto> ObtenerResumenAsync()
    {
        var resumen = new DashboardResumenDto
        {
            TotalUsuarios =
                await _context.Usuarios.CountAsync(),

            TotalRoles =
                await _context.Roles.CountAsync(),

            TotalMaterias =
                await _context.Materias.CountAsync(),

            TotalPrerrequisitos =
                await _context.Prerrequisitos.CountAsync(),

            TotalFranjasHorarias =
                await _context.FranjasHorarias.CountAsync(),

            TotalPlanesAcademicos =
                await _context.PlanesAcademicos.CountAsync(),

            TotalSemestresPlan =
                await _context.SemestresPlan.CountAsync(),

            TotalMateriasPlan =
                await _context.MateriasPlan.CountAsync(),

            UsuariosPorRol =
                await ObtenerUsuariosPorRolAsync(),

            MateriasPorSemestre =
                await ObtenerMateriasPorSemestreAsync(),

            FranjasPorDia =
                await ObtenerFranjasPorDiaAsync(),

            PlanesAcademicos =
                await ObtenerPlanesAcademicosAsync()
        };

        return resumen;
    }

    private async Task<List<DashboardUsuariosPorRolDto>> ObtenerUsuariosPorRolAsync()
    {
        var usuarios =
            await _context.Usuarios
                .Include(usuario => usuario.Rol)
                .ToListAsync();

        var resultado =
            usuarios
                .GroupBy(usuario =>
                    usuario.Rol != null
                        ? usuario.Rol.Nombre
                        : "Sin rol")
                .Select(grupo => new DashboardUsuariosPorRolDto
                {
                    Rol = grupo.Key,
                    TotalUsuarios = grupo.Count()
                })
                .ToList();

        return resultado;
    }

    private async Task<List<DashboardMateriasPorSemestreDto>> ObtenerMateriasPorSemestreAsync()
    {
        var materias =
            await _context.Materias
                .ToListAsync();

        var resultado =
            materias
                .GroupBy(materia => materia.Semestre)
                .OrderBy(grupo => grupo.Key)
                .Select(grupo => new DashboardMateriasPorSemestreDto
                {
                    Semestre = grupo.Key,
                    TotalMaterias = grupo.Count()
                })
                .ToList();

        return resultado;
    }

    private async Task<List<DashboardFranjasPorDiaDto>> ObtenerFranjasPorDiaAsync()
    {
        var franjas =
            await _context.FranjasHorarias
                .ToListAsync();

        var resultado =
            franjas
                .GroupBy(franja => franja.DiaSemana)
                .Select(grupo => new DashboardFranjasPorDiaDto
                {
                    Dia = grupo.Key,
                    TotalFranjas = grupo.Count()
                })
                .ToList();

        return resultado;
    }

    private async Task<List<DashboardPlanAcademicoDto>> ObtenerPlanesAcademicosAsync()
    {
        var planes =
            await _context.PlanesAcademicos
                .Include(plan => plan.Semestres)
                    .ThenInclude(semestre => semestre.MateriasPlan)
                .ToListAsync();

        var resultado =
            planes
                .Select(plan => new DashboardPlanAcademicoDto
                {
                    IdPlanAcademico = plan.IdPlanAcademico,
                    Nombre = plan.Nombre,
                    Programa = plan.Programa,
                    Anio = plan.Anio,
                    TotalSemestres = plan.Semestres.Count,
                    TotalMaterias = plan.Semestres
                        .Sum(semestre => semestre.MateriasPlan.Count)
                })
                .ToList();

        return resultado;
    }
}