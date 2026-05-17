namespace SistemaHorarios.Modelos.DTOs.Horarios;

// Representa la carga horaria asignada a un docente.
public class CargaDocenteResponse
{
    public int IdDocente { get; set; }

    public string NombreDocente { get; set; } = string.Empty;

    public string IdentificacionDocente { get; set; } = string.Empty;

    public int CantidadClasesAsignadas { get; set; }

    public int HorasAsignadas { get; set; }
}