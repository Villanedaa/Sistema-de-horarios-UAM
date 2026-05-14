namespace SistemaHorarios.API.DTOs.Materias
{
    //Representa la información necesaria para actualizar una materia existente en el sistema.
    public class ActualizarMateriaRequest
    {
        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public int Creditos { get; set; }

        public int IntensidadHorariaSemanal { get; set; }

        public int Semestre { get; set; }

        public bool Activa { get; set; }
    }
}
