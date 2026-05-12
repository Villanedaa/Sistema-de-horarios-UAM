using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    [HttpGet("resumen")]
    public IActionResult ObtenerResumen()
    {
        var resumen = new
        {
            TotalMaterias = 50,
            TotalDocentes = 50,
            TotalGrupos = 19,
            TotalHorarios = 19,
            UltimosHorarios = new[]
            {
                new
                {
                    Nombre = "Horario_2026-01",
                    Fecha = "2026-04-15",
                    Jornada = "Nocturna",
                    Grupos = 12,
                    Semestre = 1,
                    Estado = "Aprobado"
                },
                new
                {
                    Nombre = "Horario_2026-02",
                    Fecha = "2026-04-16",
                    Jornada = "Diurna",
                    Grupos = 40,
                    Semestre = 2,
                    Estado = "Aprobado"
                },
                new
                {
                    Nombre = "Horario_2026-03",
                    Fecha = "2026-04-18",
                    Jornada = "Diurna",
                    Grupos = 64,
                    Semestre = 2,
                    Estado = "Aprobado"
                }
            }
        };

        return Ok(resumen);
    }
}