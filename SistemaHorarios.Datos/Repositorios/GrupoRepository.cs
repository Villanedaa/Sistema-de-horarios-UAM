using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Repositorios;

// Gestiona las consultas y operaciones de grupos en la base de datos.
public class GrupoRepository
{
    private readonly SistemaHorariosDbContext contexto;

    // Recibe el contexto de base de datos.
    public GrupoRepository(SistemaHorariosDbContext contexto)
    {
        this.contexto = contexto;
    }

    // Lista todos los grupos registrados.
    public async Task<List<Grupo>> ListarGruposAsync()
    {
        return await contexto.Grupos.ToListAsync();
    }

    // Lista únicamente los grupos activos.
    public async Task<List<Grupo>> ListarGruposActivosAsync()
    {
        return await contexto.Grupos
            .Where(grupo => grupo.Activo)
            .ToListAsync();
    }

    // Busca un grupo por su identificador.
    public async Task<Grupo?> ObtenerGrupoPorIdAsync(int idGrupo)
    {
        return await contexto.Grupos
            .FirstOrDefaultAsync(grupo => grupo.IdGrupo == idGrupo);
    }

    // Lista grupos por plan académico.
    public async Task<List<Grupo>> ListarGruposPorPlanAcademicoAsync(int idPlanAcademico)
    {
        return await contexto.Grupos
            .Where(grupo => grupo.IdPlanAcademico == idPlanAcademico)
            .ToListAsync();
    }

    // Lista grupos por número de semestre.
    public async Task<List<Grupo>> ListarGruposPorSemestreAsync(int numeroSemestre)
    {
        return await contexto.Grupos
            .Where(grupo => grupo.NumeroSemestre == numeroSemestre)
            .ToListAsync();
    }

    // Verifica si existe un grupo con el identificador recibido.
    public async Task<bool> ExisteGrupoPorIdAsync(int idGrupo)
    {
        return await contexto.Grupos
            .AnyAsync(grupo => grupo.IdGrupo == idGrupo);
    }

    // Verifica si ya existe un grupo con el mismo código.
    public async Task<bool> ExisteCodigoGrupoAsync(string codigo, int idGrupoExcluir)
    {
        string codigoLimpio = codigo.Trim().ToLower();

        return await contexto.Grupos
            .AnyAsync(grupo =>
                grupo.IdGrupo != idGrupoExcluir &&
                grupo.Codigo.ToLower() == codigoLimpio
            );
    }


    // Guarda un nuevo grupo.
    public async Task CrearGrupoAsync(Grupo grupo)
    {
        await contexto.Grupos.AddAsync(grupo);
        await contexto.SaveChangesAsync();
    }

    // Actualiza un grupo existente.
    public async Task<bool> ActualizarGrupoAsync(Grupo grupoModificado)
    {
        Grupo? grupoActual = await ObtenerGrupoPorIdAsync(grupoModificado.IdGrupo);

        if (grupoActual == null)
        {
            return false;
        }

        grupoActual.Codigo = grupoModificado.Codigo;
	grupoActual.Nombre = grupoModificado.Nombre;
	grupoActual.Jornada = grupoModificado.Jornada;
	grupoActual.TipoGrupo = grupoModificado.TipoGrupo;
	grupoActual.NumeroSemestre = grupoModificado.NumeroSemestre;
	grupoActual.CantidadEstudiantes = grupoModificado.CantidadEstudiantes;
	grupoActual.IdPlanAcademico = grupoModificado.IdPlanAcademico;
	grupoActual.Activo = grupoModificado.Activo;

        await contexto.SaveChangesAsync();

        return true;
    }

    // Desactiva un grupo sin eliminarlo físicamente.
    public async Task<bool> DesactivarGrupoAsync(int idGrupo)
    {
        Grupo? grupo = await ObtenerGrupoPorIdAsync(idGrupo);

        if (grupo == null)
        {
            return false;
        }

        grupo.Activo = false;

        await contexto.SaveChangesAsync();

        return true;
    }

// Activa nuevamente un grupo académico.
public async Task<bool> ActivarGrupoAsync(int idGrupo)
{
    Grupo? grupo = await ObtenerGrupoPorIdAsync(idGrupo);

    if (grupo == null)
    {
        return false;
    }

    grupo.Activo = true;

    await contexto.SaveChangesAsync();

    return true;
}
}