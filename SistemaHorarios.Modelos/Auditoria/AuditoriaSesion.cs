using System.Threading;

namespace SistemaHorarios.Modelos.Auditoria;

/// <summary>
/// Mantiene los datos del usuario autenticado durante una petición HTTP.
/// </summary>
public static class AuditoriaSesion
{
    private static readonly AsyncLocal<DatosAuditoria?> _datosActuales = new();

    public static DatosAuditoria? Actual
    {
        get => _datosActuales.Value;
        set => _datosActuales.Value = value;
    }

    public static void Limpiar()
    {
        _datosActuales.Value = null;
    }
}

public class DatosAuditoria
{
    public int? IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = "Sistema";

    public string RolUsuario { get; set; } = "Sistema";
}
