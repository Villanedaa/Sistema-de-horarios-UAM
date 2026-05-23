using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Datos.Interfaces;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Repositorios;

public class PlanAcademicoRepository : IPlanAcademicoRepository
{
    private readonly SistemaHorariosDbContext _context;

    public PlanAcademicoRepository(
        SistemaHorariosDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlanAcademico>> ObtenerTodosAsync()
    {
        return await _context.PlanesAcademicos
            .Include(plan => plan.Semestres)
                .ThenInclude(semestre => semestre.MateriasPlan)
                    .ThenInclude(materiaPlan => materiaPlan.Materia)
            .ToListAsync();
    }

    public async Task<PlanAcademico?> ObtenerPorIdAsync(
        int idPlanAcademico)
    {
        return await _context.PlanesAcademicos
            .Include(plan => plan.Semestres)
                .ThenInclude(semestre => semestre.MateriasPlan)
                    .ThenInclude(materiaPlan => materiaPlan.Materia)
            .FirstOrDefaultAsync(plan =>
                plan.IdPlanAcademico == idPlanAcademico);
    }

    public async Task CrearAsync(
        PlanAcademico planAcademico)
    {
        await _context.PlanesAcademicos.AddAsync(planAcademico);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ActualizarAsync(
        PlanAcademico planAcademico)
    {
        _context.PlanesAcademicos.Update(planAcademico);

        int filasAfectadas =
            await _context.SaveChangesAsync();

        return filasAfectadas > 0;
    }

    public async Task<bool> EliminarAsync(
    PlanAcademico planAcademico)
    {
        planAcademico.Estado = "Inactivo";

        _context.PlanesAcademicos.Update(planAcademico);

        int filasAfectadas =
            await _context.SaveChangesAsync();

        return filasAfectadas > 0;
    }

    public async Task<bool> ActivarAsync(
        PlanAcademico planAcademico)
    {
        planAcademico.Estado = "Activo";

        _context.PlanesAcademicos.Update(planAcademico);

        int filasAfectadas =
            await _context.SaveChangesAsync();

        return filasAfectadas > 0;
    }

    public async Task<SemestrePlan?> ObtenerSemestrePorIdAsync(
        int idSemestrePlan)
    {
        return await _context.SemestresPlan
            .Include(semestre => semestre.MateriasPlan)
                .ThenInclude(materiaPlan => materiaPlan.Materia)
            .FirstOrDefaultAsync(semestre =>
                semestre.IdSemestrePlan == idSemestrePlan);
    }

    public async Task<SemestrePlan?> ObtenerSemestrePorNumeroAsync(
        int idPlanAcademico,
        int numeroSemestre)
    {
        return await _context.SemestresPlan
            .FirstOrDefaultAsync(semestre =>
                semestre.IdPlanAcademico == idPlanAcademico &&
                semestre.NumeroSemestre == numeroSemestre);
    }

    public async Task CrearSemestreAsync(
        SemestrePlan semestrePlan)
    {
        await _context.SemestresPlan.AddAsync(semestrePlan);

        await _context.SaveChangesAsync();
    }

    public async Task<Materia?> ObtenerMateriaPorIdAsync(
        int idMateria)
    {
        return await _context.Materias
            .FirstOrDefaultAsync(materia =>
                materia.IdMateria == idMateria);
    }

    public async Task<bool> ExisteMateriaEnSemestreAsync(
        int idSemestrePlan,
        int idMateria)
    {
        return await _context.MateriasPlan
            .AnyAsync(materiaPlan =>
                materiaPlan.IdSemestrePlan == idSemestrePlan &&
                materiaPlan.IdMateria == idMateria);
    }

    public async Task CrearMateriaPlanAsync(
        MateriaPlan materiaPlan)
    {
        await _context.MateriasPlan.AddAsync(materiaPlan);

        await _context.SaveChangesAsync();
    }

    public async Task<MateriaPlan?> ObtenerMateriaPlanPorIdAsync(
        int idMateriaPlan)
    {
        return await _context.MateriasPlan
            .Include(materiaPlan => materiaPlan.Materia)
            .FirstOrDefaultAsync(materiaPlan =>
                materiaPlan.IdMateriaPlan == idMateriaPlan);
    }

    public async Task<bool> EliminarMateriaPlanAsync(
        MateriaPlan materiaPlan)
    {
        _context.MateriasPlan.Remove(materiaPlan);

        int filasAfectadas =
            await _context.SaveChangesAsync();

        return filasAfectadas > 0;
    }
}