namespace SistemaHorarios.API.DTOs.Materias
{
    // Representa la información necesaria para crear una nueva materia en el sistema.
    public class CrearMateriaRequest
    {
        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public int Creditos { get; set; }

        public int IntensidadHorariaSemanal { get; set; }

        public int Semestre { get; set; }
    }
}
