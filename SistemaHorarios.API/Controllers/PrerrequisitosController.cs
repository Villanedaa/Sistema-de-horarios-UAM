using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/prerrequisitos")]
public class PrerrequisitosController : ControllerBase
{
    [HttpGet]
    public IActionResult ObtenerPrerrequisitos()
    {
        return Ok(new[]
        {
            new
            {
                IdPrerrequisito = 1,
                IdMateria = 2,
                Materia = "Ecuaciones Diferenciales",
                IdMateriaPrerrequisito = 1,
                MateriaPrerrequisito = "Cálculo Diferencial",
                Estado = "Activo"
            },
            new
            {
                IdPrerrequisito = 2,
                IdMateria = 2,
                Materia = "Ecuaciones Diferenciales",
                IdMateriaPrerrequisito = 3,
                MateriaPrerrequisito = "Cálculo Integral",
                Estado = "Activo"
            }
        });
    }

    [HttpGet("materia/{idMateria}")]
    public IActionResult ObtenerPrerrequisitosPorMateria(int idMateria)
    {
        return Ok(new
        {
            IdMateria = idMateria,
            Materia = "Ecuaciones Diferenciales",
            Prerrequisitos = new[]
            {
                new
                {
                    IdMateria = 1,
                    Codigo = "MAT101",
                    Nombre = "Cálculo Diferencial"
                },
                new
                {
                    IdMateria = 3,
                    Codigo = "MAT301",
                    Nombre = "Cálculo Integral"
                }
            }
        });
    }

    [HttpPost]
    public IActionResult CrearPrerrequisito([FromBody] CrearPrerrequisitoRequest request)
    {
        return Ok(new
        {
            IdPrerrequisito = 3,
            request.IdMateria,
            request.IdMateriaPrerrequisito,
            Estado = "Activo",
            Mensaje = "Prerrequisito asignado correctamente."
        });
    }

    [HttpDelete("{id}")]
    public IActionResult EliminarPrerrequisito(int id)
    {
        return Ok(new
        {
            IdPrerrequisito = id,
            Mensaje = "Prerrequisito eliminado correctamente."
        });
    }
}

public class CrearPrerrequisitoRequest
{
    public int IdMateria { get; set; }
    public int IdMateriaPrerrequisito { get; set; }
}