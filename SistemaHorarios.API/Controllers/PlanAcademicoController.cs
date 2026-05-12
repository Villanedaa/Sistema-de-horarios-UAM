using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/plan-academico")]
public class PlanAcademicoController : ControllerBase
{
    [HttpGet]
    public IActionResult ObtenerPlanesAcademicos()
    {
        return Ok(new[]
        {
            new
            {
                IdPlan = 1,
                Nombre = "Plan Académico Diurno",
                Jornada = "Diurna",
                TotalSemestres = 10,
                TotalMaterias = 57,
                Estado = "Activo"
            },
            new
            {
                IdPlan = 2,
                Nombre = "Plan Académico Nocturno",
                Jornada = "Nocturna",
                TotalSemestres = 12,
                TotalMaterias = 57,
                Estado = "Activo"
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerPlanPorId(int id)
    {
        return Ok(new
        {
            IdPlan = id,
            Nombre = "Plan Académico Diurno",
            Jornada = "Diurna",
            TotalSemestres = 10,
            TotalMaterias = 57,
            Estado = "Activo"
        });
    }

    [HttpGet("jornada/{jornada}")]
    public IActionResult ObtenerPlanPorJornada(string jornada)
    {
        return Ok(new
        {
            IdPlan = 1,
            Nombre = $"Plan Académico {jornada}",
            Jornada = jornada,
            TotalSemestres = jornada.ToLower() == "nocturna" ? 12 : 10,
            TotalMaterias = 57,
            Estado = "Activo"
        });
    }

    [HttpGet("{id}/semestres")]
    public IActionResult ObtenerSemestresDelPlan(int id)
    {
        return Ok(new[]
        {
            new
            {
                NumeroSemestre = 1,
                CantidadMaterias = 5,
                CreditosTotales = 15
            },
            new
            {
                NumeroSemestre = 2,
                CantidadMaterias = 5,
                CreditosTotales = 16
            },
            new
            {
                NumeroSemestre = 3,
                CantidadMaterias = 6,
                CreditosTotales = 18
            }
        });
    }

    [HttpGet("{id}/semestres/{numeroSemestre}")]
    public IActionResult ObtenerDetalleSemestre(int id, int numeroSemestre)
    {
        return Ok(new
        {
            IdPlan = id,
            Jornada = "Diurna",
            NumeroSemestre = numeroSemestre,
            Materias = new[]
            {
                new
                {
                    IdMateria = 1,
                    Codigo = "MAT101",
                    Nombre = "Cálculo Diferencial",
                    Creditos = 4,
                    IntensidadHorariaSemanal = 64
                },
                new
                {
                    IdMateria = 2,
                    Codigo = "PRO101",
                    Nombre = "Programación I",
                    Creditos = 3,
                    IntensidadHorariaSemanal = 64
                }
            }
        });
    }

    [HttpPost]
    public IActionResult CrearPlanAcademico([FromBody] CrearPlanAcademicoRequest request)
    {
        return Ok(new
        {
            IdPlan = 3,
            request.Nombre,
            request.Jornada,
            request.TotalSemestres,
            request.Estado,
            Mensaje = "Plan académico creado correctamente."
        });
    }

    [HttpPut("{id}")]
    public IActionResult ActualizarPlanAcademico(int id, [FromBody] ActualizarPlanAcademicoRequest request)
    {
        return Ok(new
        {
            IdPlan = id,
            request.Nombre,
            request.Jornada,
            request.TotalSemestres,
            request.Estado,
            Mensaje = "Plan académico actualizado correctamente."
        });
    }

    [HttpPut("{id}/semestres/{numeroSemestre}")]
    public IActionResult ActualizarSemestreDelPlan(
        int id,
        int numeroSemestre,
        [FromBody] ActualizarSemestrePlanRequest request)
    {
        return Ok(new
        {
            IdPlan = id,
            NumeroSemestre = numeroSemestre,
            Materias = request.IdsMaterias,
            Mensaje = "Semestre actualizado correctamente."
        });
    }
}

public class CrearPlanAcademicoRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string Jornada { get; set; } = string.Empty;
    public int TotalSemestres { get; set; }
    public string Estado { get; set; } = string.Empty;
}

public class ActualizarPlanAcademicoRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string Jornada { get; set; } = string.Empty;
    public int TotalSemestres { get; set; }
    public string Estado { get; set; } = string.Empty;
}

public class ActualizarSemestrePlanRequest
{
    public List<int> IdsMaterias { get; set; } = new();
}