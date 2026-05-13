namespace SistemaHorarios.API.DTOs.Materias
{
    //Representa toda la información de una materia activa en el sistema.
    public class MateriaResponse
    {
        public int IdMateria { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public int Creditos { get; set; }

        public int IntensidadHorariaSemanal { get; set; }

        public int Semestre { get; set; }

        public bool Activa { get; set; }

        public string EstadoTexto { get; set; } = string.Empty;
    }
}
