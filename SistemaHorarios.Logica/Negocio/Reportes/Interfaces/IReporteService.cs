using SistemaHorarios.Modelos.DTOs.Reportes;

namespace SistemaHorarios.Logica.Negocio.Reportes.Interfaces;

public interface IReporteService
{
    Task<ReporteGeneralDto> ObtenerReporteGeneralAsync();

    Task<List<ReporteUsuariosPorRolDto>> ObtenerUsuariosPorRolAsync();

    Task<List<ReporteMateriasPorSemestreDto>> ObtenerMateriasPorSemestreAsync();

    Task<List<ReporteFranjaHorariaDto>> ObtenerFranjasPorDiaAsync();

    Task<List<ReportePlanAcademicoDto>> ObtenerPlanesAcademicosAsync();
}
