using SistemaHorarios.Modelos.DTOs.Dashboard;

namespace SistemaHorarios.Logica.Negocio.Dashboard.Interfaces;

public interface IDashboardService
{
    Task<DashboardResumenDto> ObtenerResumenAsync();
}
