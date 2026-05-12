using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/grupos")]
public class GruposController : ControllerBase
{
    [HttpGet]
    public IActionResult ObtenerGrupos([FromQuery] string? busqueda)
    {
        return Ok(new[]
        {
            new
            {
                IdGrupo = 1,
                NombreGrupo = "Grupo 15-D-01",
                Codigo = "1A-2026-2",
                Jornada = "Diurna",
                Materia = "Técnicas",
                CupoMaximo = 45,
                CuposOcupados = 30,
                CuposDisponibles = 15,
                Tipo = "Mixto",
                Estado = "Activo"
            },
            new
            {
                IdGrupo = 2,
                NombreGrupo = "Grupo 10-N-01",
                Codigo = "1B-2026-2",
                Jornada = "Nocturna",
                Materia = "Física II",
                CupoMaximo = 25,
                CuposOcupados = 19,
                CuposDisponibles = 6,
                Tipo = "TAPSI",
                Estado = "Activo"
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerGrupoPorId(int id)
    {
        return Ok(new
        {
            IdGrupo = id,
            NombreGrupo = "Grupo 15-D-01",
            Codigo = "1A-2026-2",
            Jornada = "Diurna",
            IdMateria = 1,
            Materia = "Técnicas",
            CupoMaximo = 45,
            CuposOcupados = 30,
            CuposDisponibles = 15,
            Tipo = "Mixto",
            Estado = "Activo"
        });
    }

    [HttpPost]
    public IActionResult CrearGrupo([FromBody] CrearGrupoRequest request)
    {
        return Ok(new
        {
            IdGrupo = 3,
            request.NombreGrupo,
            request.Codigo,
            request.Jornada,
            request.IdMateria,
            request.CupoMaximo,
            CuposOcupados = 0,
            CuposDisponibles = request.CupoMaximo,
            request.Tipo,
            request.Estado,
            Mensaje = "Grupo creado correctamente."
        });
    }

    [HttpPut("{id}")]
    public IActionResult ActualizarGrupo(int id, [FromBody] ActualizarGrupoRequest request)
    {
        int cuposDisponibles = request.CupoMaximo - request.CuposOcupados;

        return Ok(new
        {
            IdGrupo = id,
            request.NombreGrupo,
            request.Codigo,
            request.Jornada,
            request.IdMateria,
            request.CupoMaximo,
            request.CuposOcupados,
            CuposDisponibles = cuposDisponibles,
            request.Tipo,
            request.Estado,
            Mensaje = "Grupo actualizado correctamente."
        });
    }

    [HttpDelete("{id}")]
    public IActionResult EliminarGrupo(int id)
    {
        return Ok(new
        {
            IdGrupo = id,
            Mensaje = "Grupo inactivado correctamente."
        });
    }

    [HttpGet("{id}/cupos")]
    public IActionResult ObtenerCuposGrupo(int id)
    {
        return Ok(new
        {
            IdGrupo = id,
            CupoMaximo = 45,
            CuposOcupados = 30,
            CuposDisponibles = 15
        });
    }

    [HttpGet("activos")]
    public IActionResult ObtenerGruposActivos()
    {
        return Ok(new[]
        {
            new
            {
                IdGrupo = 1,
                NombreGrupo = "Grupo 15-D-01",
                Codigo = "1A-2026-2"
            },
            new
            {
                IdGrupo = 2,
                NombreGrupo = "Grupo 10-N-01",
                Codigo = "1B-2026-2"
            }
        });
    }
}

public class CrearGrupoRequest
{
    public string NombreGrupo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Jornada { get; set; } = string.Empty;
    public int IdMateria { get; set; }
    public int CupoMaximo { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class ActualizarGrupoRequest
{
    public string NombreGrupo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Jornada { get; set; } = string.Empty;
    public int IdMateria { get; set; }
    public int CupoMaximo { get; set; }
    public int CuposOcupados { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}