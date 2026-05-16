namespace SistemaHorarios.Modelos.DTOs.PlanAcademico
{
    public class MateriaPlanResponseDto
    {
        public int IdMateriaPlan { get; set; }

        public int IdMateria { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public int Creditos { get; set; }

        public int IntensidadHorariaSemanal { get; set; }
    }
}