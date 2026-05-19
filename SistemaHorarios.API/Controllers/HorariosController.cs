using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Horarios;
using SistemaHorarios.Modelos.DTOs.Horarios;
using HorarioEntidad = SistemaHorarios.Modelos.Entidades.Horario;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/horarios")]
public class HorariosController : ControllerBase
{
    private readonly GestorHorario gestorHorario;

    // Recibe el gestor de horarios para aplicar reglas de negocio.
    public HorariosController(GestorHorario gestorHorario)
    {
        this.gestorHorario = gestorHorario;
    }

    // Lista todos los horarios registrados.
    [HttpGet]
    public async Task<ActionResult<List<HorarioResponse>>> ObtenerHorarios()
    {
        List<HorarioEntidad> horarios =
            await gestorHorario.ListarHorariosAsync();

        List<HorarioResponse> respuesta = horarios
            .Select(MapearHorarioResponse)
            .ToList();

        return Ok(respuesta);
    }

    // Consulta un horario por su identificador.
    [HttpGet("{id}")]
    public async Task<ActionResult<HorarioResponse>> ObtenerHorarioPorId(int id)
    {
        HorarioEntidad? horario =
            await gestorHorario.ConsultarHorarioPorIdAsync(id);

        if (horario == null)
        {
            return NotFound("El horario no existe.");
        }

        return Ok(MapearHorarioResponse(horario));
    }

    // Crea una nueva asignación de horario.
    [HttpPost]
    public async Task<IActionResult> CrearHorario(
        [FromBody] GenerarHorarioRequest request)
    {
        HorarioEntidad horario = new HorarioEntidad
        {
            IdGrupo = request.IdGrupo,
            IdMateria = request.IdMateria,
            IdDocente = request.IdDocente,
            IdFranjaHoraria = request.IdFranjaHoraria,
            Observacion = request.Observacion
        };

        List<string> errores =
            await gestorHorario.CrearHorarioAsync(horario);

        if (errores.Count > 0)
        {
            return BadRequest(errores);
        }

        HorarioEntidad? horarioCreado =
            await gestorHorario.ConsultarHorarioPorIdAsync(horario.IdHorario);

        return Ok(new
        {
            Mensaje = "Horario creado correctamente.",
            Horario = horarioCreado == null
                ? MapearHorarioResponse(horario)
                : MapearHorarioResponse(horarioCreado)
        });
    }

    // Actualiza una asignación de horario existente.
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarHorario(
        int id,
        [FromBody] ActualizarHorarioRequest request)
    {
        HorarioEntidad horario = new HorarioEntidad
        {
            IdGrupo = request.IdGrupo,
            IdMateria = request.IdMateria,
            IdDocente = request.IdDocente,
            IdFranjaHoraria = request.IdFranjaHoraria,
            Observacion = request.Observacion,
            Activo = request.Activo
        };

        List<string> errores =
            await gestorHorario.ModificarHorarioAsync(id, horario);

        if (errores.Count > 0)
        {
            return BadRequest(errores);
        }

        HorarioEntidad? horarioActualizado =
            await gestorHorario.ConsultarHorarioPorIdAsync(id);

        if (horarioActualizado == null)
        {
            return NotFound("El horario no existe.");
        }

        return Ok(new
        {
            Mensaje = "Horario actualizado correctamente.",
            Horario = MapearHorarioResponse(horarioActualizado)
        });
    }

    // Actualiza solo la materia, docente y franja de un horario.
    [HttpPut("{id}/asignatura")]
    public async Task<IActionResult> ActualizarAsignaturaHorario(
        int id,
        [FromBody] ActualizarAsignaturaHorarioRequest request)
    {
        HorarioEntidad horario = new HorarioEntidad
        {
            IdMateria = request.IdMateria,
            IdDocente = request.IdDocente,
            IdFranjaHoraria = request.IdFranjaHoraria,
            Observacion = request.Observacion
        };

        HorarioEntidad? horarioActual =
            await gestorHorario.ConsultarHorarioPorIdAsync(id);

        if (horarioActual == null)
        {
            return NotFound("El horario no existe.");
        }

        horario.IdGrupo = horarioActual.IdGrupo;
        horario.Activo = horarioActual.Activo;

        List<string> errores =
            await gestorHorario.ModificarAsignaturaHorarioAsync(id, horario);

        if (errores.Count > 0)
        {
            return BadRequest(errores);
        }

        HorarioEntidad? horarioActualizado =
            await gestorHorario.ConsultarHorarioPorIdAsync(id);

        return Ok(new
        {
            Mensaje = "Asignatura del horario actualizada correctamente.",
            Horario = horarioActualizado == null
                ? null
                : MapearHorarioResponse(horarioActualizado)
        });
    }

    // Inactiva un horario sin eliminarlo físicamente.
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarHorario(int id)
    {
        List<string> errores =
            await gestorHorario.DesactivarHorarioAsync(id);

        if (errores.Count > 0)
        {
            return BadRequest(errores);
        }

        return Ok(new
        {
            IdHorario = id,
            Mensaje = "Horario inactivado correctamente."
        });
    }

    // Genera automáticamente los bloques de horario para un grupo.
    [HttpPost("generar/{idGrupo}")]
    public async Task<IActionResult> GenerarHorario(int idGrupo)
    {
        var (generados, errores) = await gestorHorario.GenerarHorariosAsync(idGrupo);

        if (errores.Count > 0 && generados == 0)
            return BadRequest(new { Errores = errores });

        return Ok(new { Generados = generados, Advertencias = errores });
    }

    // Lista los horarios asociados a un grupo.
    [HttpGet("grupo/{idGrupo}")]
    public async Task<ActionResult<List<HorarioResponse>>> ObtenerHorariosPorGrupo(
        int idGrupo)
    {
        List<HorarioEntidad> horarios =
            await gestorHorario.ListarHorariosPorGrupoAsync(idGrupo);

        List<HorarioResponse> respuesta = horarios
            .Select(MapearHorarioResponse)
            .ToList();

        return Ok(respuesta);
    }

    // Lista los horarios asociados a un docente.
    [HttpGet("docente/{idDocente}")]
    public async Task<ActionResult<List<HorarioDocenteResponse>>> ObtenerHorariosPorDocente(
        int idDocente)
    {
        List<HorarioEntidad> horarios =
            await gestorHorario.ListarHorariosPorDocenteAsync(idDocente);

        List<HorarioDocenteResponse> respuesta = horarios
            .Select(MapearHorarioDocenteResponse)
            .ToList();

        return Ok(respuesta);
    }

    // Lista los horarios asociados a una materia.
    [HttpGet("materia/{idMateria}")]
    public async Task<ActionResult<List<HorarioResponse>>> ObtenerHorariosPorMateria(
        int idMateria)
    {
        List<HorarioEntidad> horarios =
            await gestorHorario.ListarHorariosPorMateriaAsync(idMateria);

        List<HorarioResponse> respuesta = horarios
            .Select(MapearHorarioResponse)
            .ToList();

        return Ok(respuesta);
    }

    // Busca horarios por nombre, identificación o correo del docente.
    [HttpGet("docentes/buscar")]
    public async Task<ActionResult<List<HorarioDocenteResponse>>> BuscarHorariosPorDocente(
        [FromQuery] string busqueda)
    {
        List<HorarioEntidad> horarios =
            await gestorHorario.BuscarHorariosPorDocenteAsync(busqueda);

        List<HorarioDocenteResponse> respuesta = horarios
            .Select(MapearHorarioDocenteResponse)
            .ToList();

        return Ok(respuesta);
    }

    // Convierte una entidad Horario en respuesta completa.
    private HorarioResponse MapearHorarioResponse(HorarioEntidad horario)
    {
        TimeSpan horaInicio = horario.FranjaHoraria?.HoraInicio ?? TimeSpan.Zero;
        TimeSpan horaFin = horario.FranjaHoraria?.HoraFin ?? TimeSpan.Zero;

        return new HorarioResponse
        {
            IdHorario = horario.IdHorario,

            IdGrupo = horario.IdGrupo,
            CodigoGrupo = horario.Grupo?.Codigo ?? string.Empty,
            NombreGrupo = horario.Grupo?.Nombre ?? string.Empty,
            Jornada = horario.Grupo?.Jornada ?? string.Empty,
            TipoGrupo = horario.Grupo?.TipoGrupo ?? string.Empty,

            IdMateria = horario.IdMateria,
            CodigoMateria = horario.Materia?.Codigo ?? string.Empty,
            NombreMateria = horario.Materia?.Nombre ?? string.Empty,

            IdDocente = horario.IdDocente,
            NombreDocente = horario.Docente?.NombreCompleto ?? string.Empty,
            IdentificacionDocente = horario.Docente?.Identificacion ?? string.Empty,

            IdFranjaHoraria = horario.IdFranjaHoraria,
            DiaSemana = horario.FranjaHoraria?.DiaSemana ?? string.Empty,
            HoraInicio = horaInicio,
            HoraFin = horaFin,
            HorarioTexto = ObtenerHorarioTexto(horaInicio, horaFin),

            Observacion = horario.Observacion,
            Activo = horario.Activo,
            EstadoTexto = ObtenerEstadoTexto(horario.Activo)
        };
    }

    // Convierte una entidad Horario en respuesta para horario docente.
    private HorarioDocenteResponse MapearHorarioDocenteResponse(
        HorarioEntidad horario)
    {
        TimeSpan horaInicio = horario.FranjaHoraria?.HoraInicio ?? TimeSpan.Zero;
        TimeSpan horaFin = horario.FranjaHoraria?.HoraFin ?? TimeSpan.Zero;

        return new HorarioDocenteResponse
        {
            IdHorario = horario.IdHorario,

            IdDocente = horario.IdDocente,
            NombreDocente = horario.Docente?.NombreCompleto ?? string.Empty,
            IdentificacionDocente = horario.Docente?.Identificacion ?? string.Empty,
            CorreoInstitucional = horario.Docente?.CorreoInstitucional ?? string.Empty,

            IdGrupo = horario.IdGrupo,
            CodigoGrupo = horario.Grupo?.Codigo ?? string.Empty,
            NombreGrupo = horario.Grupo?.Nombre ?? string.Empty,

            IdMateria = horario.IdMateria,
            CodigoMateria = horario.Materia?.Codigo ?? string.Empty,
            NombreMateria = horario.Materia?.Nombre ?? string.Empty,

            DiaSemana = horario.FranjaHoraria?.DiaSemana ?? string.Empty,
            HoraInicio = horaInicio,
            HoraFin = horaFin,
            HorarioTexto = ObtenerHorarioTexto(horaInicio, horaFin),

            EstadoTexto = ObtenerEstadoTexto(horario.Activo)
        };
    }

    // Retorna el horario en formato legible.
    private string ObtenerHorarioTexto(TimeSpan horaInicio, TimeSpan horaFin)
    {
        return $"{horaInicio:hh\\:mm} - {horaFin:hh\\:mm}";
    }

    // Convierte el estado booleano en texto legible.
    private string ObtenerEstadoTexto(bool activo)
    {
        if (activo)
        {
            return "Activo";
        }

        return "Inactivo";
    }
}