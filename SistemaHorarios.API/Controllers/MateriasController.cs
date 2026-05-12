using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/materias")]
public class MateriasController : ControllerBase
{
    [HttpGet]
    public IActionResult ObtenerMaterias(
        [FromQuery] string? busqueda,
        [FromQuery] int? semestre,
        [FromQuery] string? estado)
    {
        return Ok(new[]
        {
            new
            {
                IdMateria = 1,
                Codigo = "MAT101",
                Nombre = "Cálculo Diferencial",
                Creditos = 4,
                IntensidadHorariaSemanal = 64,
                Semestre = 1,
                CantidadGrupos = 2,
                Estado = "Activa"
            },
            new
            {
                IdMateria = 2,
                Codigo = "MAT201",
                Nombre = "Ecuaciones Diferenciales",
                Creditos = 3,
                IntensidadHorariaSemanal = 64,
                Semestre = 5,
                CantidadGrupos = 6,
                Estado = "Activa"
            },
            new
            {
                IdMateria = 3,
                Codigo = "MAT301",
                Nombre = "Cálculo Integral",
                Creditos = 4,
                IntensidadHorariaSemanal = 64,
                Semestre = 2,
                CantidadGrupos = 3,
                Estado = "Activa"
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerMateriaPorId(int id)
    {
        return Ok(new
        {
            IdMateria = id,
            Codigo = "MAT201",
            Nombre = "Ecuaciones Diferenciales",
            Creditos = 3,
            IntensidadHorariaSemanal = 64,
            Semestre = 5,
            CantidadGrupos = 6,
            Estado = "Activa",
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
    public IActionResult CrearMateria([FromBody] CrearMateriaRequest request)
    {
        return Ok(new
        {
            IdMateria = 4,
            request.Codigo,
            request.Nombre,
            request.Creditos,
            request.IntensidadHorariaSemanal,
            request.Semestre,
            request.IdsPrerrequisitos,
            Estado = "Activa",
            Mensaje = "Materia creada correctamente."
        });
    }

    [HttpPut("{id}")]
    public IActionResult ActualizarMateria(int id, [FromBody] ActualizarMateriaRequest request)
    {
        return Ok(new
        {
            IdMateria = id,
            request.Codigo,
            request.Nombre,
            request.Creditos,
            request.IntensidadHorariaSemanal,
            request.Semestre,
            request.IdsPrerrequisitos,
            request.Estado,
            Mensaje = "Materia actualizada correctamente."
        });
    }

    [HttpDelete("{id}")]
    public IActionResult EliminarMateria(int id)
    {
        return Ok(new
        {
            IdMateria = id,
            Mensaje = "Materia inactivada correctamente."
        });
    }

    [HttpGet("activas")]
    public IActionResult ObtenerMateriasActivas()
    {
        return Ok(new[]
        {
            new
            {
                IdMateria = 1,
                Codigo = "MAT101",
                Nombre = "Cálculo Diferencial"
            },
            new
            {
                IdMateria = 2,
                Codigo = "MAT201",
                Nombre = "Ecuaciones Diferenciales"
            },
            new
            {
                IdMateria = 3,
                Codigo = "MAT301",
                Nombre = "Cálculo Integral"
            }
        });
    }

    [HttpGet("{id}/prerrequisitos")]
    public IActionResult ObtenerPrerrequisitosDeMateria(int id)
    {
        return Ok(new
        {
            IdMateria = id,
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
}

public class CrearMateriaRequest
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public int IntensidadHorariaSemanal { get; set; }
    public int Semestre { get; set; }
    public List<int> IdsPrerrequisitos { get; set; } = new();
}

public class ActualizarMateriaRequest
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public int IntensidadHorariaSemanal { get; set; }
    public int Semestre { get; set; }
    public List<int> IdsPrerrequisitos { get; set; } = new();
    public string Estado { get; set; } = string.Empty;
}