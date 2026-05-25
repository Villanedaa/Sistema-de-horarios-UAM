using SistemaHorarios.Modelos.DTOs.Reportes;

namespace SistemaHorarios.Logica.Negocio.Reportes.Interfaces;

public interface IReporteService
{
    Task<ReporteGeneralDto> ObtenerReporteGeneralAsync();

    Task<List<ReporteUsuariosPorRolDto>> ObtenerUsuariosPorRolAsync();

    Task<List<ReporteMateriasPorSemestreDto>> ObtenerMateriasPorSemestreAsync();

    Task<List<ReporteFranjaHorariaDto>> ObtenerFranjasPorDiaAsync();

    Task<List<ReportePlanAcademicoDto>> ObtenerPlanesAcademicosAsync();

    Task<List<ReporteHorarioGrupoDto>> ObtenerHorarioPorGrupoAsync(int idGrupo);

    Task<List<ReporteHorarioDocenteDto>> ObtenerHorarioPorDocenteAsync(int idDocente);

    Task<List<ReporteCargaDocenteDto>> ObtenerCargaDocenteAsync();

    Task<List<ReporteConflictoHorarioDto>> ObtenerConflictosHorarioAsync();

    Task<List<ReporteHorarioGeneradoDto>> ObtenerHorariosGeneradosAsync();

    Task<(byte[] Archivo, string ContentType, string NombreArchivo)> ExportarReporteAsync(
        string tipo,
        string formato,
        int? idGrupo,
        int? idDocente);
}
