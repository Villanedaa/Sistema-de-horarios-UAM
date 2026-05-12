using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/docentes")]
public class DocentesController : ControllerBase
{
    [HttpGet("resumen")]
    public IActionResult ObtenerResumenDocentes()
    {
        return Ok(new
        {
            TotalDocentes = 50,
            DocentesActivos = 30,
            DocentesInactivos = 20,
            DocentesDisponibles = 38
        });
    }

    [HttpGet]
    public IActionResult ObtenerDocentes([FromQuery] string? busqueda)
    {
        return Ok(new[]
        {
            new
            {
                IdDocente = 1,
                NombreCompleto = "María Luna",
                Identificacion = "123456789",
                CorreoInstitucional = "maria.luna@uam.edu.co",
                Materias = new[] { "Física I", "Cálculo Integral" },
                DisponibilidadGeneral = "7 horas",
                Estado = "Activo"
            },
            new
            {
                IdDocente = 2,
                NombreCompleto = "Carlos Vega",
                Identificacion = "987654321",
                CorreoInstitucional = "carlos.vega@uam.edu.co",
                Materias = new[] { "Programación I", "Bases de Datos" },
                DisponibilidadGeneral = "10 horas",
                Estado = "Activo"
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerDocentePorId(int id)
    {
        return Ok(new
        {
            IdDocente = id,
            NombreCompleto = "María Luna",
            Identificacion = "123456789",
            CorreoInstitucional = "maria.luna@uam.edu.co",
            Materias = new[] { "Física I", "Cálculo Integral" },
            DisponibilidadGeneral = "7 horas",
            Estado = "Activo"
        });
    }

    [HttpPost]
    public IActionResult CrearDocente([FromBody] CrearDocenteRequest request)
    {
        return Ok(new
        {
            IdDocente = 3,
            request.NombreCompleto,
            request.Identificacion,
            request.CorreoInstitucional,
            request.IdsMaterias,
            request.Estado,
            request.DisponibilidadGeneral,
            Mensaje = "Docente creado correctamente."
        });
    }

    [HttpPut("{id}")]
    public IActionResult ActualizarDocente(int id, [FromBody] ActualizarDocenteRequest request)
    {
        return Ok(new
        {
            IdDocente = id,
            request.NombreCompleto,
            request.Identificacion,
            request.CorreoInstitucional,
            request.IdsMaterias,
            request.Estado,
            request.DisponibilidadGeneral,
            Mensaje = "Docente actualizado correctamente."
        });
    }

    [HttpDelete("{id}")]
    public IActionResult EliminarDocente(int id)
    {
        return Ok(new
        {
            IdDocente = id,
            Mensaje = "Docente inactivado correctamente."
        });
    }

    [HttpGet("{id}/disponibilidad")]
    public IActionResult ObtenerDisponibilidadDocente(int id)
    {
        return Ok(new
        {
            IdDocente = id,
            NombreDocente = "María Luna",
            Disponibilidad = new[]
            {
                new
                {
                    Dia = "Lunes",
                    HoraInicio = "07:00",
                    HoraFin = "09:00",
                    Disponible = true
                },
                new
                {
                    Dia = "Miércoles",
                    HoraInicio = "14:00",
                    HoraFin = "16:00",
                    Disponible = true
                }
            }
        });
    }

    [HttpPut("{id}/disponibilidad")]
    public IActionResult ActualizarDisponibilidadDocente(
        int id,
        [FromBody] ActualizarDisponibilidadDocenteRequest request)
    {
        return Ok(new
        {
            IdDocente = id,
            request.Disponibilidad,
            Mensaje = "Disponibilidad docente actualizada correctamente."
        });
    }

    [HttpGet("{id}/materias")]
    public IActionResult ObtenerMateriasDelDocente(int id)
    {
        return Ok(new
        {
            IdDocente = id,
            Materias = new[]
            {
                new
                {
                    IdMateria = 1,
                    Codigo = "FIS101",
                    Nombre = "Física I"
                },
                new
                {
                    IdMateria = 2,
                    Codigo = "MAT201",
                    Nombre = "Cálculo Integral"
                }
            }
        });
    }

    [HttpPut("{id}/materias")]
    public IActionResult ActualizarMateriasDelDocente(
        int id,
        [FromBody] ActualizarMateriasDocenteRequest request)
    {
        return Ok(new
        {
            IdDocente = id,
            request.IdsMaterias,
            Mensaje = "Materias del docente actualizadas correctamente."
        });
    }

    [HttpGet("activos")]
    public IActionResult ObtenerDocentesActivos()
    {
        return Ok(new[]
        {
            new
            {
                IdDocente = 1,
                NombreCompleto = "María Luna"
            },
            new
            {
                IdDocente = 2,
                NombreCompleto = "Carlos Vega"
            }
        });
    }

    [HttpGet("disponibles")]
    public IActionResult ObtenerDocentesDisponibles()
    {
        return Ok(new[]
        {
            new
            {
                IdDocente = 1,
                NombreCompleto = "María Luna",
                DisponibilidadGeneral = "7 horas"
            }
        });
    }
}

public class CrearDocenteRequest
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string CorreoInstitucional { get; set; } = string.Empty;
    public List<int> IdsMaterias { get; set; } = new();
    public string Estado { get; set; } = string.Empty;
    public string DisponibilidadGeneral { get; set; } = string.Empty;
}

public class ActualizarDocenteRequest
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string CorreoInstitucional { get; set; } = string.Empty;
    public List<int> IdsMaterias { get; set; } = new();
    public string Estado { get; set; } = string.Empty;
    public string DisponibilidadGeneral { get; set; } = string.Empty;
}

public class ActualizarDisponibilidadDocenteRequest
{
    public List<DisponibilidadDocenteRequest> Disponibilidad { get; set; } = new();
}

public class DisponibilidadDocenteRequest
{
    public string Dia { get; set; } = string.Empty;
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFin { get; set; } = string.Empty;
    public bool Disponible { get; set; }
}

public class ActualizarMateriasDocenteRequest
{
    public List<int> IdsMaterias { get; set; } = new();
}