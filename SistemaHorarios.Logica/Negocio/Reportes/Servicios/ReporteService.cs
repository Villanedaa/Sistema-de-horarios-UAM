using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Logica.Negocio.Reportes.Interfaces;
using SistemaHorarios.Modelos.DTOs.Reportes;

namespace SistemaHorarios.Logica.Negocio.Reportes.Servicios;

public class ReporteService : IReporteService
{
    private readonly SistemaHorariosDbContext _context;

    public ReporteService(SistemaHorariosDbContext context)
    {
        _context = context;
    }

    public async Task<ReporteGeneralDto> ObtenerReporteGeneralAsync()
    {
        var reporte = new ReporteGeneralDto
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
                await _context.MateriasPlan.CountAsync()
        };

        return reporte;
    }

    public async Task<List<ReporteUsuariosPorRolDto>> ObtenerUsuariosPorRolAsync()
    {
        var usuarios =
            await _context.Usuarios
                .Include(usuario => usuario.Rol)
                .ToListAsync();

        var reporte =
            usuarios
                .GroupBy(usuario =>
                    usuario.Rol != null
                        ? usuario.Rol.Nombre
                        : "Sin rol")
                .Select(grupo => new ReporteUsuariosPorRolDto
                {
                    Rol = grupo.Key,
                    TotalUsuarios = grupo.Count()
                })
                .ToList();

        return reporte;
    }

    public async Task<List<ReporteMateriasPorSemestreDto>> ObtenerMateriasPorSemestreAsync()
    {
        var materias =
            await _context.Materias
                .ToListAsync();

        var reporte =
            materias
                .GroupBy(materia => materia.Semestre)
                .OrderBy(grupo => grupo.Key)
                .Select(grupo => new ReporteMateriasPorSemestreDto
                {
                    Semestre = grupo.Key,
                    TotalMaterias = grupo.Count(),
                    Materias = grupo
                        .Select(materia => materia.Nombre)
                        .ToList()
                })
                .ToList();

        return reporte;
    }

    public async Task<List<ReporteFranjaHorariaDto>> ObtenerFranjasPorDiaAsync()
    {
        var franjas =
            await _context.FranjasHorarias
                .ToListAsync();

        var reporte =
            franjas
                .GroupBy(franja => franja.DiaSemana)
                .Select(grupo => new ReporteFranjaHorariaDto
                {
                    Dia = grupo.Key,
                    TotalFranjas = grupo.Count()
                })
                .ToList();

        return reporte;
    }

    public async Task<List<ReportePlanAcademicoDto>> ObtenerPlanesAcademicosAsync()
    {
        var planes =
            await _context.PlanesAcademicos
                .Include(plan => plan.Semestres)
                    .ThenInclude(semestre => semestre.MateriasPlan)
                .ToListAsync();

        var reporte =
            planes
                .Select(plan => new ReportePlanAcademicoDto
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

        return reporte;
    }
}