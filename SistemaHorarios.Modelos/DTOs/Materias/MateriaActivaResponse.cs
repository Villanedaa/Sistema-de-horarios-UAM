namespace SistemaHorarios.API.DTOs.Materias
{

    // Representa una materia activa para listas de selección.
    public class MateriaActivaResponse
    {
        public int IdMateria { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;
    }
}