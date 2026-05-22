using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.PlanAcademico.Interfaces;
using SistemaHorarios.Modelos.DTOs.PlanAcademico;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlanAcademicoController : ControllerBase
{
    private readonly IPlanAcademicoService _planAcademicoService;

    public PlanAcademicoController(
        IPlanAcademicoService planAcademicoService)
    {
        _planAcademicoService = planAcademicoService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Coordinador")]
    public async Task<ActionResult<List<PlanAcademicoResponseDto>>> ObtenerTodos()
    {
        var planes =
            await _planAcademicoService.ObtenerTodosAsync();

        return Ok(planes);
    }

    [HttpGet("{idPlanAcademico}")]
    [Authorize(Roles = "Administrador,Coordinador")]
    public async Task<ActionResult<PlanAcademicoResponseDto>> ObtenerPorId(
        int idPlanAcademico)
    {
        var plan =
            await _planAcademicoService.ObtenerPorIdAsync(idPlanAcademico);

        if (plan == null)
        {
            return NotFound("Plan académico no encontrado.");
        }

        return Ok(plan);
    }

    [HttpPost]
    public async Task<ActionResult<PlanAcademicoResponseDto>> Crear(
        CrearPlanAcademicoDto dto)
    {
        var planCreado =
            await _planAcademicoService.CrearAsync(dto);

        return Ok(planCreado);
    }

    [HttpPut("{idPlanAcademico}")]
    public async Task<ActionResult> Actualizar(
        int idPlanAcademico,
        ActualizarPlanAcademicoDto dto)
    {
        bool actualizado =
            await _planAcademicoService.ActualizarAsync(
                idPlanAcademico,
                dto);

        if (!actualizado)
        {
            return NotFound("Plan académico no encontrado.");
        }

        return Ok("Plan académico actualizado correctamente.");
    }

    [HttpDelete("{idPlanAcademico}")]
    public async Task<ActionResult> Eliminar(
        int idPlanAcademico)
    {
        bool eliminado =
            await _planAcademicoService.EliminarAsync(idPlanAcademico);

        if (!eliminado)
        {
            return NotFound("Plan académico no encontrado.");
        }

        return Ok("Plan académico eliminado correctamente.");
    }

    [HttpPost("{idPlanAcademico}/semestres")]
    public async Task<ActionResult<SemestrePlanResponseDto>> AgregarSemestre(
        int idPlanAcademico,
        CrearSemestrePlanDto dto)
    {
        var semestreCreado =
            await _planAcademicoService.AgregarSemestreAsync(
                idPlanAcademico,
                dto);

        return Ok(semestreCreado);
    }

    [HttpPost("semestres/{idSemestrePlan}/materias")]
    public async Task<ActionResult<MateriaPlanResponseDto>> AgregarMateriaASemestre(
        int idSemestrePlan,
        AgregarMateriaPlanDto dto)
    {
        var materiaAgregada =
            await _planAcademicoService.AgregarMateriaASemestreAsync(
                idSemestrePlan,
                dto);

        return Ok(materiaAgregada);
    }

    [HttpDelete("materias/{idMateriaPlan}")]
    public async Task<ActionResult> EliminarMateriaDelPlan(
        int idMateriaPlan)
    {
        bool eliminada =
            await _planAcademicoService.EliminarMateriaDelPlanAsync(
                idMateriaPlan);

        if (!eliminada)
        {
            return NotFound("Materia del plan no encontrada.");
        }

        return Ok("Materia eliminada del plan correctamente.");
    }
}