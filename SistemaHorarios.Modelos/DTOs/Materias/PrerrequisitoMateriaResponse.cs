namespace SistemaHorarios.API.DTOs.Materias
{
    // Representa un prerrequisito asociado a una materia.
    public class PrerrequisitoMateriaResponse
    {
        public int IdPrerrequisito { get; set; }

        public int IdMateria { get; set; }

        public int IdMateriaPrerrequisito { get; set; }

        public string CodigoMateriaPrerrequisito { get; set; } = string.Empty;

        public string NombreMateriaPrerrequisito { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }
}