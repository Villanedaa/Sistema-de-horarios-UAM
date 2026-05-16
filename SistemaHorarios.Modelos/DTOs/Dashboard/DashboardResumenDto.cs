namespace SistemaHorarios.Modelos.DTOs.Dashboard;

public class DashboardResumenDto
{
    public int TotalUsuarios { get; set; }

    public int TotalRoles { get; set; }

    public int TotalMaterias { get; set; }

    public int TotalPrerrequisitos { get; set; }

    public int TotalFranjasHorarias { get; set; }

    public int TotalPlanesAcademicos { get; set; }

    public int TotalSemestresPlan { get; set; }

    public int TotalMateriasPlan { get; set; }

    public List<DashboardUsuariosPorRolDto> UsuariosPorRol { get; set; } =
        new List<DashboardUsuariosPorRolDto>();

    public List<DashboardMateriasPorSemestreDto> MateriasPorSemestre { get; set; } =
        new List<DashboardMateriasPorSemestreDto>();

    public List<DashboardFranjasPorDiaDto> FranjasPorDia { get; set; } =
        new List<DashboardFranjasPorDiaDto>();

    public List<DashboardPlanAcademicoDto> PlanesAcademicos { get; set; } =
        new List<DashboardPlanAcademicoDto>();
}