using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/materias")]
public class MateriasController : ControllerBase
{
    [HttpGet]
    public IActionResult ObtenerMaterias()
    {
        var materias = new[]
        {
            new
            {
                Id = 1,
                Codigo = "MAT-001",
                Nombre = "Programación I",
                Creditos = 3,
                IntensidadHorariaSemanal = 4,
                SemestreSugerido = 1,
                Estado = "Activa"
            },
            new
            {
                Id = 2,
                Codigo = "MAT-002",
                Nombre = "Estructuras de Datos",
                Creditos = 3,
                IntensidadHorariaSemanal = 4,
                SemestreSugerido = 3,
                Estado = "Activa"
            },
            new
            {
                Id = 3,
                Codigo = "MAT-003",
                Nombre = "Bases de Datos",
                Creditos = 3,
                IntensidadHorariaSemanal = 4,
                SemestreSugerido = 4,
                Estado = "Activa"
            }
        };

        return Ok(materias);
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerMateriaPorId(int id)
    {
        var materia = new
        {
            Id = id,
            Codigo = "MAT-002",
            Nombre = "Estructuras de Datos",
            Creditos = 3,
            IntensidadHorariaSemanal = 4,
            SemestreSugerido = 3,
            Estado = "Activa",
            Prerrequisitos = new[]
            {
                new
                {
                    Id = 1,
                    Codigo = "MAT-001",
                    Nombre = "Programación I"
                }
            }
        };

        return Ok(materia);
    }

    [HttpPost]
    public IActionResult CrearMateria([FromBody] CrearMateriaRequest request)
    {
        var materiaCreada = new
        {
            Id = 4,
            Codigo = request.Codigo,
            Nombre = request.Nombre,
            Creditos = request.Creditos,
            IntensidadHorariaSemanal = request.IntensidadHorariaSemanal,
            SemestreSugerido = request.SemestreSugerido,
            Estado = "Activa"
        };

        return Ok(materiaCreada);
    }

    [HttpPut("{id}")]
    public IActionResult ActualizarMateria(int id, [FromBody] ActualizarMateriaRequest request)
    {
        var materiaActualizada = new
        {
            Id = id,
            Codigo = request.Codigo,
            Nombre = request.Nombre,
            Creditos = request.Creditos,
            IntensidadHorariaSemanal = request.IntensidadHorariaSemanal,
            SemestreSugerido = request.SemestreSugerido,
            Estado = request.Estado
        };

        return Ok(materiaActualizada);
    }

    [HttpDelete("{id}")]
    public IActionResult EliminarMateria(int id)
    {
        return Ok(new
        {
            Mensaje = "Materia eliminada correctamente.",
            Id = id
        });
    }

    [HttpGet("{id}/prerrequisitos")]
    public IActionResult ObtenerPrerrequisitosDeMateria(int id)
    {
        var prerrequisitos = new[]
        {
            new
            {
                Id = 1,
                Codigo = "MAT-001",
                Nombre = "Programación I",
                Creditos = 3
            }
        };

        return Ok(new
        {
            IdMateria = id,
            Prerrequisitos = prerrequisitos
        });
    }
}

public class CrearMateriaRequest
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public int IntensidadHorariaSemanal { get; set; }
    public int SemestreSugerido { get; set; }
}

public class ActualizarMateriaRequest
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public int IntensidadHorariaSemanal { get; set; }
    public int SemestreSugerido { get; set; }
    public string Estado { get; set; } = string.Empty;
}