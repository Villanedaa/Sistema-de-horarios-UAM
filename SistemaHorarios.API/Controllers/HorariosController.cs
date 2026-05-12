using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/horarios")]
public class HorariosController : ControllerBase
{
    [HttpGet]
    public IActionResult ObtenerHorarios()
    {
        return Ok(new[]
        {
            new
            {
                IdHorario = 1,
                IdGrupo = 7,
                HoraInicio = "14:00",
                HoraFinal = "16:00",
                Dia = "Lunes",
                Jornada = "Diurna",
                Estado = "Generado"
            },
            new
            {
                IdHorario = 2,
                IdGrupo = 8,
                HoraInicio = "08:00",
                HoraFinal = "10:00",
                Dia = "Martes",
                Jornada = "Nocturna",
                Estado = "Generado"
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerHorarioPorId(int id)
    {
        return Ok(new
        {
            IdHorario = id,
            IdGrupo = 7,
            Grupo = "Grupo 15-D-01",
            Materia = "Bases de Datos",
            Docente = "Lina Gómez",
            HoraInicio = "14:00",
            HoraFinal = "16:00",
            Dia = "Lunes",
            Jornada = "Diurna",
            Estado = "Generado"
        });
    }

    [HttpPost("generar")]
    public IActionResult GenerarHorario([FromBody] GenerarHorarioRequest request)
    {
        return Ok(new
        {
            IdHorario = 1,
            request.HoraInicio,
            request.HoraFinal,
            request.DuracionBloque,
            request.Dias,
            Estado = "Generado",
            Mensaje = "Horario generado correctamente."
        });
    }

    [HttpPut("{id}")]
    public IActionResult ActualizarHorario(int id, [FromBody] ActualizarHorarioRequest request)
    {
        return Ok(new
        {
            IdHorario = id,
            request.IdGrupo,
            request.IdMateria,
            request.IdDocente,
            request.Dia,
            request.HoraInicio,
            request.HoraFinal,
            request.Jornada,
            request.Estado,
            Mensaje = "Horario actualizado correctamente."
        });
    }

    [HttpPut("{id}/asignatura")]
    public IActionResult ActualizarAsignaturaHorario(
        int id,
        [FromBody] ActualizarAsignaturaHorarioRequest request)
    {
        return Ok(new
        {
            IdHorario = id,
            request.IdMateria,
            request.IdDocente,
            request.FechaInicio,
            request.HoraInicio,
            request.HoraFinal,
            request.Intensidad,
            request.Observacion,
            Mensaje = "Asignatura actualizada correctamente."
        });
    }

    [HttpGet("{id}/vista-previa")]
    public IActionResult ObtenerVistaPreviaHorario(int id)
    {
        return Ok(new
        {
            IdHorario = id,
            Estado = "PendienteAprobacion",
            Bloques = new[]
            {
                new
                {
                    Dia = "Lunes",
                    HoraInicio = "07:00",
                    HoraFinal = "08:00",
                    Asignatura = "Cálculo Diferencial",
                    Docente = "Carlos Pérez",
                    Grupo = "Grupo 1A",
                    Color = "Verde"
                },
                new
                {
                    Dia = "Martes",
                    HoraInicio = "09:00",
                    HoraFinal = "10:00",
                    Asignatura = "Programación I",
                    Docente = "Ana Gómez",
                    Grupo = "Grupo 1B",
                    Color = "Azul"
                }
            }
        });
    }

    [HttpPost("{id}/aprobar")]
    public IActionResult AprobarHorario(int id)
    {
        return Ok(new
        {
            IdHorario = id,
            Estado = "Aprobado",
            Mensaje = "Horario aprobado correctamente."
        });
    }

    [HttpPost("{id}/rechazar")]
    public IActionResult RechazarHorario(int id, [FromBody] RechazarHorarioRequest request)
    {
        return Ok(new
        {
            IdHorario = id,
            Estado = "Rechazado",
            request.Motivo,
            Mensaje = "Horario rechazado correctamente."
        });
    }

    [HttpGet("conflictos")]
    public IActionResult ObtenerConflictos()
    {
        return Ok(new[]
        {
            new
            {
                IdConflicto = 1,
                Tipo = "Docente",
                Descripcion = "El docente tiene dos grupos asignados en el mismo horario.",
                Dia = "Lunes",
                HoraInicio = "08:00",
                HoraFinal = "10:00"
            }
        });
    }

    [HttpGet("visualizacion")]
    public IActionResult VisualizarHorarios([FromQuery] string? busqueda, [FromQuery] string? programa)
    {
        return Ok(new
        {
            Coordinador = "Administrador",
            Codigo = "1234",
            Programa = programa ?? "Ingeniería de Sistemas",
            Horarios = new[]
            {
                new
                {
                    Asignatura = "Inglés LAN",
                    Dia = "Martes",
                    Horario = "10:00 - 12:00",
                    Docente = "Lina Gómez",
                    Estado = "Activa"
                }
            }
        });
    }

    [HttpGet("docentes")]
    public IActionResult ObtenerHorariosDocentes()
    {
        return Ok(new[]
        {
            new
            {
                IdDocente = 1,
                NombreDocente = "Carlos Pérez",
                TotalHorasSemanales = 16,
                Horarios = new[]
                {
                    new
                    {
                        Dia = "Lunes",
                        HoraInicio = "08:00",
                        HoraFinal = "10:00",
                        Materia = "Programación I",
                        Grupo = "Grupo 1A",
                        Aula = "Aula 203"
                    }
                }
            }
        });
    }

    [HttpGet("docentes/{idDocente}")]
    public IActionResult ObtenerHorarioDocentePorId(int idDocente)
    {
        return Ok(new
        {
            IdDocente = idDocente,
            NombreDocente = "Carlos Pérez",
            TotalHorasSemanales = 16,
            Horarios = new[]
            {
                new
                {
                    Dia = "Lunes",
                    HoraInicio = "08:00",
                    HoraFinal = "10:00",
                    Materia = "Programación I",
                    Grupo = "Grupo 1A",
                    Jornada = "Diurna",
                    Aula = "Aula 203"
                },
                new
                {
                    Dia = "Miércoles",
                    HoraInicio = "10:00",
                    HoraFinal = "12:00",
                    Materia = "Bases de Datos",
                    Grupo = "Grupo 2B",
                    Jornada = "Diurna",
                    Aula = "Aula 105"
                }
            }
        });
    }

    [HttpGet("docentes/{idDocente}/disponibilidad")]
    public IActionResult ObtenerDisponibilidadDocente(int idDocente)
    {
        return Ok(new
        {
            IdDocente = idDocente,
            NombreDocente = "Carlos Pérez",
            Disponibilidad = new[]
            {
                new
                {
                    Dia = "Lunes",
                    HoraInicio = "08:00",
                    HoraFinal = "12:00"
                },
                new
                {
                    Dia = "Miércoles",
                    HoraInicio = "14:00",
                    HoraFinal = "18:00"
                }
            }
        });
    }

    [HttpGet("docentes/{idDocente}/conflictos")]
    public IActionResult ObtenerConflictosDocente(int idDocente)
    {
        return Ok(new
        {
            IdDocente = idDocente,
            NombreDocente = "Carlos Pérez",
            Conflictos = new[]
            {
                new
                {
                    Dia = "Lunes",
                    HoraInicio = "08:00",
                    HoraFinal = "10:00",
                    Descripcion = "El docente tiene dos grupos asignados en el mismo horario."
                }
            }
        });
    }

    [HttpGet("docentes/{idDocente}/carga")]
    public IActionResult ObtenerCargaDocente(int idDocente)
    {
        return Ok(new
        {
            IdDocente = idDocente,
            NombreDocente = "Carlos Pérez",
            TotalHorasSemanales = 16,
            MateriasAsignadas = 3,
            GruposAsignados = 4
        });
    }
}

public class GenerarHorarioRequest
{
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFinal { get; set; } = string.Empty;
    public int DuracionBloque { get; set; }
    public List<string> Dias { get; set; } = new();
}

public class ActualizarHorarioRequest
{
    public int IdGrupo { get; set; }
    public int IdMateria { get; set; }
    public int IdDocente { get; set; }
    public string Dia { get; set; } = string.Empty;
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFinal { get; set; } = string.Empty;
    public string Jornada { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class ActualizarAsignaturaHorarioRequest
{
    public int IdMateria { get; set; }
    public int IdDocente { get; set; }
    public string FechaInicio { get; set; } = string.Empty;
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFinal { get; set; } = string.Empty;
    public int Intensidad { get; set; }
    public string Observacion { get; set; } = string.Empty;
}

public class RechazarHorarioRequest
{
    public string Motivo { get; set; } = string.Empty;
}