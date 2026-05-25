namespace SistemaHorarios.Modelos.DTOs.HistorialCambios;

/// <summary>
/// DTO para mostrar un registro del historial de cambios.
/// </summary>
public class HistorialCambioResponse
{
    public int IdCambio { get; set; }

    public int IdHistorialCambio { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime FechaHora { get; set; }

    public string Fecha { get; set; } = string.Empty;

    public string Hora { get; set; } = string.Empty;

    public string Usuario { get; set; } = string.Empty;

    public string NombreUsuario { get; set; } = string.Empty;

    public string RolUsuario { get; set; } = string.Empty;

    public string Modulo { get; set; } = string.Empty;

    public string Accion { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string EntidadAfectada { get; set; } = string.Empty;

    public int? IdEntidadAfectada { get; set; }

    public string ValorAnterior { get; set; } = string.Empty;

    public string ValorNuevo { get; set; } = string.Empty;
}
