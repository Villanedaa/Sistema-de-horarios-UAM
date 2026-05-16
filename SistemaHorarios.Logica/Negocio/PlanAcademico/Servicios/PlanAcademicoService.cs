using SistemaHorarios.Datos.Interfaces;
using SistemaHorarios.Logica.Negocio.PlanAcademico.Interfaces;
using SistemaHorarios.Modelos.DTOs.PlanAcademico;
using MateriaPlanEntidad = SistemaHorarios.Modelos.Entidades.MateriaPlan;
using PlanAcademicoEntidad = SistemaHorarios.Modelos.Entidades.PlanAcademico;
using SemestrePlanEntidad = SistemaHorarios.Modelos.Entidades.SemestrePlan;

namespace SistemaHorarios.Logica.Negocio.PlanAcademico.Servicios;

public class PlanAcademicoService : IPlanAcademicoService
{
    private readonly IPlanAcademicoRepository _repository;

    public PlanAcademicoService(
        IPlanAcademicoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PlanAcademicoResponseDto>> ObtenerTodosAsync()
    {
        var planes =
            await _repository.ObtenerTodosAsync();

        return planes
            .Select(plan => MapearPlan(plan))
            .ToList();
    }

    public async Task<PlanAcademicoResponseDto?> ObtenerPorIdAsync(
        int idPlanAcademico)
    {
        var plan =
            await _repository.ObtenerPorIdAsync(idPlanAcademico);

        if (plan == null)
        {
            return null;
        }

        return MapearPlan(plan);
    }

    public async Task<PlanAcademicoResponseDto> CrearAsync(
        CrearPlanAcademicoDto dto)
    {
        var plan = new PlanAcademicoEntidad
        {
            Nombre = dto.Nombre,
            Programa = dto.Programa,
            Anio = dto.Anio,
            Estado = dto.Estado
        };

        await _repository.CrearAsync(plan);

        return MapearPlan(plan);
    }

    public async Task<bool> ActualizarAsync(
        int idPlanAcademico,
        ActualizarPlanAcademicoDto dto)
    {
        var plan =
            await _repository.ObtenerPorIdAsync(idPlanAcademico);

        if (plan == null)
        {
            return false;
        }

        plan.Nombre = dto.Nombre;
        plan.Programa = dto.Programa;
        plan.Anio = dto.Anio;
        plan.Estado = dto.Estado;

        return await _repository.ActualizarAsync(plan);
    }

    public async Task<bool> EliminarAsync(
        int idPlanAcademico)
    {
        var plan =
            await _repository.ObtenerPorIdAsync(idPlanAcademico);

        if (plan == null)
        {
            return false;
        }

        return await _repository.EliminarAsync(plan);
    }

    public async Task<SemestrePlanResponseDto> AgregarSemestreAsync(
        int idPlanAcademico,
        CrearSemestrePlanDto dto)
    {
        var plan =
            await _repository.ObtenerPorIdAsync(idPlanAcademico);

        if (plan == null)
        {
            throw new Exception("El plan académico no existe.");
        }

        var semestreExistente =
            await _repository.ObtenerSemestrePorNumeroAsync(
                idPlanAcademico,
                dto.NumeroSemestre);

        if (semestreExistente != null)
        {
            throw new Exception("El semestre ya existe en este plan académico.");
        }

        var semestre = new SemestrePlanEntidad
        {
            IdPlanAcademico = idPlanAcademico,
            NumeroSemestre = dto.NumeroSemestre
        };

        await _repository.CrearSemestreAsync(semestre);

        return MapearSemestre(semestre);
    }

    public async Task<MateriaPlanResponseDto> AgregarMateriaASemestreAsync(
        int idSemestrePlan,
        AgregarMateriaPlanDto dto)
    {
        var semestre =
            await _repository.ObtenerSemestrePorIdAsync(idSemestrePlan);

        if (semestre == null)
        {
            throw new Exception("El semestre no existe.");
        }

        var materia =
            await _repository.ObtenerMateriaPorIdAsync(dto.IdMateria);

        if (materia == null)
        {
            throw new Exception("La materia no existe.");
        }

        bool yaExiste =
            await _repository.ExisteMateriaEnSemestreAsync(
                idSemestrePlan,
                dto.IdMateria);

        if (yaExiste)
        {
            throw new Exception("La materia ya está agregada a este semestre.");
        }

        var materiaPlan = new MateriaPlanEntidad
        {
            IdSemestrePlan = idSemestrePlan,
            IdMateria = dto.IdMateria,
            Materia = materia
        };

        await _repository.CrearMateriaPlanAsync(materiaPlan);

        return MapearMateriaPlan(materiaPlan);
    }

    public async Task<bool> EliminarMateriaDelPlanAsync(
        int idMateriaPlan)
    {
        var materiaPlan =
            await _repository.ObtenerMateriaPlanPorIdAsync(idMateriaPlan);

        if (materiaPlan == null)
        {
            return false;
        }

        return await _repository.EliminarMateriaPlanAsync(materiaPlan);
    }

    private PlanAcademicoResponseDto MapearPlan(
        PlanAcademicoEntidad plan)
    {
        return new PlanAcademicoResponseDto
        {
            IdPlanAcademico = plan.IdPlanAcademico,
            Nombre = plan.Nombre,
            Programa = plan.Programa,
            Anio = plan.Anio,
            Estado = plan.Estado,
            Semestres = plan.Semestres
                .OrderBy(semestre => semestre.NumeroSemestre)
                .Select(semestre => MapearSemestre(semestre))
                .ToList()
        };
    }

    private SemestrePlanResponseDto MapearSemestre(
        SemestrePlanEntidad semestre)
    {
        return new SemestrePlanResponseDto
        {
            IdSemestrePlan = semestre.IdSemestrePlan,
            NumeroSemestre = semestre.NumeroSemestre,
            Materias = semestre.MateriasPlan
                .Select(materiaPlan => MapearMateriaPlan(materiaPlan))
                .ToList()
        };
    }

    private MateriaPlanResponseDto MapearMateriaPlan(
        MateriaPlanEntidad materiaPlan)
    {
        return new MateriaPlanResponseDto
        {
            IdMateriaPlan = materiaPlan.IdMateriaPlan,
            IdMateria = materiaPlan.IdMateria,
            Codigo = materiaPlan.Materia?.Codigo ?? string.Empty,
            Nombre = materiaPlan.Materia?.Nombre ?? string.Empty,
            Creditos = materiaPlan.Materia?.Creditos ?? 0,
            IntensidadHorariaSemanal =
                materiaPlan.Materia?.IntensidadHorariaSemanal ?? 0
        };
    }
}