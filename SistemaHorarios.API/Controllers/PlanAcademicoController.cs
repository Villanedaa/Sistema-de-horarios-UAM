using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.PlanAcademico.Interfaces;
using SistemaHorarios.Modelos.DTOs.PlanAcademico;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador,Coordinador")]
public class PlanAcademicoController : ControllerBase
{
    private readonly IPlanAcademicoService _planAcademicoService;

    public PlanAcademicoController(
        IPlanAcademicoService planAcademicoService)
    {
        _planAcademicoService = planAcademicoService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PlanAcademicoResponseDto>>>> ObtenerTodos()
    {
        var planes =
            await _planAcademicoService.ObtenerTodosAsync();

        return Ok(OkResponse("Planes académicos obtenidos correctamente.", planes));
    }

    [HttpGet("{idPlanAcademico}")]
    public async Task<ActionResult<ApiResponse<PlanAcademicoResponseDto>>> ObtenerPorId(
        int idPlanAcademico)
    {
        var plan =
            await _planAcademicoService.ObtenerPorIdAsync(idPlanAcademico);

        if (plan == null)
        {
            return NotFound(ErrorResponse<PlanAcademicoResponseDto>("Plan académico no encontrado."));
        }

        return Ok(OkResponse("Plan académico obtenido correctamente.", plan));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PlanAcademicoResponseDto>>> Crear(
        CrearPlanAcademicoDto dto)
    {
        var planCreado =
            await _planAcademicoService.CrearAsync(dto);

        return Ok(OkResponse("Plan académico creado correctamente.", planCreado));
    }

    [HttpPut("{idPlanAcademico}")]
    public async Task<ActionResult<ApiResponse<object>>> Actualizar(
        int idPlanAcademico,
        ActualizarPlanAcademicoDto dto)
    {
        bool actualizado =
            await _planAcademicoService.ActualizarAsync(
                idPlanAcademico,
                dto);

        if (!actualizado)
        {
            return NotFound(ErrorResponse<object>("Plan académico no encontrado."));
        }

        return Ok(OkResponse<object>("Plan académico actualizado correctamente.", null));
    }

    [HttpDelete("{idPlanAcademico}")]
    public async Task<ActionResult<ApiResponse<object>>> Eliminar(
        int idPlanAcademico)
    {
        bool eliminado =
            await _planAcademicoService.EliminarAsync(idPlanAcademico);

        if (!eliminado)
        {
            return NotFound(ErrorResponse<object>("Plan académico no encontrado."));
        }

        return Ok(OkResponse<object>("Plan académico eliminado correctamente.", null));
    }

    [HttpPost("{idPlanAcademico}/semestres")]
    public async Task<ActionResult<ApiResponse<SemestrePlanResponseDto>>> AgregarSemestre(
        int idPlanAcademico,
        CrearSemestrePlanDto dto)
    {
        try
        {
            var semestreCreado =
                await _planAcademicoService.AgregarSemestreAsync(
                    idPlanAcademico,
                    dto);

            return Ok(OkResponse("Semestre agregado correctamente.", semestreCreado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ErrorResponse<SemestrePlanResponseDto>(ex.Message));
        }
    }

    [HttpPost("semestres/{idSemestrePlan}/materias")]
    public async Task<ActionResult<ApiResponse<MateriaPlanResponseDto>>> AgregarMateriaASemestre(
        int idSemestrePlan,
        AgregarMateriaPlanDto dto)
    {
        try
        {
            var materiaAgregada =
                await _planAcademicoService.AgregarMateriaASemestreAsync(
                    idSemestrePlan,
                    dto);

            return Ok(OkResponse("Materia agregada al semestre correctamente.", materiaAgregada));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ErrorResponse<MateriaPlanResponseDto>(ex.Message));
        }
    }

    [HttpDelete("materias/{idMateriaPlan}")]
    public async Task<ActionResult<ApiResponse<object>>> EliminarMateriaDelPlan(
        int idMateriaPlan)
    {
        bool eliminada =
            await _planAcademicoService.EliminarMateriaDelPlanAsync(
                idMateriaPlan);

        if (!eliminada)
        {
            return NotFound(ErrorResponse<object>("Materia del plan no encontrada."));
        }

        return Ok(OkResponse<object>("Materia eliminada del plan correctamente.", null));
    }

    private static ApiResponse<T> OkResponse<T>(string message, T? data)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    private static ApiResponse<T> ErrorResponse<T>(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message
        };
    }
}
