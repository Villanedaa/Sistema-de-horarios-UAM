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

        return Ok(new ApiResponse<List<PlanAcademicoResponseDto>>
        {
            Success = true,
            Message = "Planes académicos obtenidos correctamente.",
            Data = planes
        });
    }

    [HttpGet("{idPlanAcademico}")]
    public async Task<ActionResult<ApiResponse<PlanAcademicoResponseDto>>> ObtenerPorId(
        int idPlanAcademico)
    {
        var plan =
            await _planAcademicoService.ObtenerPorIdAsync(idPlanAcademico);

        if (plan == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Plan académico no encontrado."
            });
        }

        return Ok(new ApiResponse<PlanAcademicoResponseDto>
        {
            Success = true,
            Message = "Plan académico obtenido correctamente.",
            Data = plan
        });
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PlanAcademicoResponseDto>>> Crear(
        CrearPlanAcademicoDto dto)
    {
        var planCreado =
            await _planAcademicoService.CrearAsync(dto);

        return Ok(new ApiResponse<PlanAcademicoResponseDto>
        {
            Success = true,
            Message = "Plan académico creado correctamente.",
            Data = planCreado
        });
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
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Plan académico no encontrado."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Plan académico actualizado correctamente."
        });
    }

    [HttpDelete("{idPlanAcademico}")]
    public async Task<ActionResult<ApiResponse<int>>> Eliminar(
    int idPlanAcademico)
    {
        bool eliminado =
            await _planAcademicoService.EliminarAsync(idPlanAcademico);

        if (!eliminado)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Plan académico no encontrado."
            });
        }

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Plan académico inactivado correctamente.",
            Data = idPlanAcademico
        });
    }

    [HttpPatch("{idPlanAcademico}/activar")]
    public async Task<ActionResult<ApiResponse<int>>> Activar(
    int idPlanAcademico)
    {
        bool activado =
            await _planAcademicoService.ActivarAsync(idPlanAcademico);

        if (!activado)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Plan académico no encontrado."
            });
        }

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Plan académico activado correctamente.",
            Data = idPlanAcademico
        });
    }

    [HttpPost("{idPlanAcademico}/semestres")]
    public async Task<ActionResult<ApiResponse<SemestrePlanResponseDto>>> AgregarSemestre(
        int idPlanAcademico,
        CrearSemestrePlanDto dto)
    {
        var semestreCreado =
            await _planAcademicoService.AgregarSemestreAsync(
                idPlanAcademico,
                dto);

        return Ok(new ApiResponse<SemestrePlanResponseDto>
        {
            Success = true,
            Message = "Semestre agregado correctamente.",
            Data = semestreCreado
        });
    }

    [HttpPost("semestres/{idSemestrePlan}/materias")]
    public async Task<ActionResult<ApiResponse<MateriaPlanResponseDto>>> AgregarMateriaASemestre(
        int idSemestrePlan,
        AgregarMateriaPlanDto dto)
    {
        var materiaAgregada =
            await _planAcademicoService.AgregarMateriaASemestreAsync(
                idSemestrePlan,
                dto);

        return Ok(new ApiResponse<MateriaPlanResponseDto>
        {
            Success = true,
            Message = "Materia agregada al semestre correctamente.",
            Data = materiaAgregada
        });
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
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Materia del plan no encontrada."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Materia eliminada del plan correctamente."
        });
    }
}