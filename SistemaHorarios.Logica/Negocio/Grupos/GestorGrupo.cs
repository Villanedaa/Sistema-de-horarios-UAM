using SistemaHorarios.Datos.Interfaces;
using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Grupos;

// Gestiona las reglas de negocio relacionadas con grupos académicos.
public class GestorGrupo
{
    private readonly GrupoRepository grupoRepository;
    private readonly IPlanAcademicoRepository planAcademicoRepository;
    private readonly ValidadorGrupo validadorGrupo;

    // Recibe los repositorios necesarios para trabajar con grupos.
    public GestorGrupo(
        GrupoRepository grupoRepository,
        IPlanAcademicoRepository planAcademicoRepository)
    {
        this.grupoRepository = grupoRepository;
        this.planAcademicoRepository = planAcademicoRepository;
        validadorGrupo = new ValidadorGrupo();
    }

    // Crea un nuevo grupo después de validar sus datos.
    public async Task<List<string>> CrearGrupoAsync(Grupo grupoNuevo)
    {
        List<string> errores = await ValidarCreacionGrupoAsync(grupoNuevo);

        if (errores.Count > 0)
        {
            return errores;
        }

        PrepararGrupoNuevo(grupoNuevo);

        await grupoRepository.CrearGrupoAsync(grupoNuevo);

        return errores;
    }

    // Modifica la información de un grupo existente.
    public async Task<List<string>> ModificarGrupoAsync(int idGrupo, Grupo grupoModificado)
    {
        List<string> errores = await ValidarModificacionGrupoAsync(idGrupo, grupoModificado);

        if (errores.Count > 0)
        {
            return errores;
        }

        PrepararGrupoModificado(idGrupo, grupoModificado);

        await grupoRepository.ActualizarGrupoAsync(grupoModificado);

        return errores;
    }

    // Desactiva un grupo sin eliminarlo físicamente.
    public async Task<List<string>> DesactivarGrupoAsync(int idGrupo)
    {
        List<string> errores = await ValidarDesactivacionGrupoAsync(idGrupo);

        if (errores.Count > 0)
        {
            return errores;
        }

        await grupoRepository.DesactivarGrupoAsync(idGrupo);

        return errores;
    }

    // Consulta un grupo por su identificador.
    public async Task<Grupo?> ConsultarGrupoPorIdAsync(int idGrupo)
    {
        if (idGrupo <= 0)
        {
            return null;
        }

        return await grupoRepository.ObtenerGrupoPorIdAsync(idGrupo);
    }

    // Lista todos los grupos registrados.
    public async Task<List<Grupo>> ListarGruposAsync()
    {
        return await grupoRepository.ListarGruposAsync();
    }

    // Lista únicamente los grupos activos.
    public async Task<List<Grupo>> ListarGruposActivosAsync()
    {
        return await grupoRepository.ListarGruposActivosAsync();
    }

    // Lista grupos por plan académico.
    public async Task<List<Grupo>> ListarGruposPorPlanAcademicoAsync(int idPlanAcademico)
    {
        if (idPlanAcademico <= 0)
        {
            return new List<Grupo>();
        }

        return await grupoRepository.ListarGruposPorPlanAcademicoAsync(idPlanAcademico);
    }

    // Lista grupos por semestre.
    public async Task<List<Grupo>> ListarGruposPorSemestreAsync(int numeroSemestre)
    {
        if (numeroSemestre <= 0)
        {
            return new List<Grupo>();
        }

        return await grupoRepository.ListarGruposPorSemestreAsync(numeroSemestre);
    }

    // Valida las reglas necesarias para crear un grupo.
    private async Task<List<string>> ValidarCreacionGrupoAsync(Grupo grupoNuevo)
    {
        List<string> errores = validadorGrupo.Validar(grupoNuevo);

        if (grupoNuevo == null)
        {
            return errores;
        }

        await ValidarReglasGeneralesAsync(grupoNuevo, errores, 0);

        return errores;
    }

    // Valida las reglas necesarias para modificar un grupo.
    private async Task<List<string>> ValidarModificacionGrupoAsync(
        int idGrupo,
        Grupo grupoModificado)
    {
        List<string> errores = validadorGrupo.Validar(grupoModificado);

        AgregarErrorSi(
            idGrupo <= 0,
            errores,
            "El identificador del grupo no es válido."
        );

        if (grupoModificado == null)
        {
            return errores;
        }

        bool existeGrupo = await grupoRepository.ExisteGrupoPorIdAsync(idGrupo);

        AgregarErrorSi(
            !existeGrupo,
            errores,
            "El grupo que desea modificar no existe."
        );

        await ValidarReglasGeneralesAsync(grupoModificado, errores, idGrupo);

        return errores;
    }

    // Valida que el grupo exista antes de desactivarlo.
    private async Task<List<string>> ValidarDesactivacionGrupoAsync(int idGrupo)
    {
        List<string> errores = new List<string>();

        AgregarErrorSi(
            idGrupo <= 0,
            errores,
            "El identificador del grupo no es válido."
        );

        if (errores.Count > 0)
        {
            return errores;
        }

        bool existeGrupo = await grupoRepository.ExisteGrupoPorIdAsync(idGrupo);

        AgregarErrorSi(
            !existeGrupo,
            errores,
            "El grupo que desea desactivar no existe."
        );

        return errores;
    }

    // Valida reglas generales del grupo.
    private async Task ValidarReglasGeneralesAsync(
        Grupo grupo,
        List<string> errores,
        int idGrupoExcluir)
    {
        await ValidarCodigoDuplicadoAsync(grupo.Codigo, errores, idGrupoExcluir);
        ValidarJornadaPermitida(grupo.Jornada, errores);
        ValidarTipoGrupoPermitido(grupo.TipoGrupo, errores);
        await ValidarPlanAcademicoExistenteAsync(grupo.IdPlanAcademico, errores);
    }

    // Valida que no exista otro grupo con el mismo código.
    private async Task ValidarCodigoDuplicadoAsync(
        string codigo,
        List<string> errores,
        int idGrupoExcluir)
    {
        bool existeCodigo = await grupoRepository.ExisteCodigoGrupoAsync(
            codigo,
            idGrupoExcluir
        );

        AgregarErrorSi(
            existeCodigo,
            errores,
            "Ya existe un grupo registrado con el mismo código."
        );
    }

    // Valida que la jornada pertenezca a los valores permitidos.
    private void ValidarJornadaPermitida(string jornada, List<string> errores)
    {
        bool jornadaValida =
            jornada.Equals("Diurna", StringComparison.OrdinalIgnoreCase) ||
            jornada.Equals("Nocturna", StringComparison.OrdinalIgnoreCase);

        AgregarErrorSi(
            !jornadaValida,
            errores,
            "La jornada debe ser Diurna o Nocturna."
        );
    }

    // Valida que el tipo de grupo pertenezca a los valores permitidos.
    private void ValidarTipoGrupoPermitido(string tipoGrupo, List<string> errores)
    {
        bool tipoValido =
            tipoGrupo.Equals("Regular", StringComparison.OrdinalIgnoreCase) ||
            tipoGrupo.Equals("TAPSI", StringComparison.OrdinalIgnoreCase) ||
            tipoGrupo.Equals("Mixto", StringComparison.OrdinalIgnoreCase);

        AgregarErrorSi(
            !tipoValido,
            errores,
            "El tipo de grupo debe ser Regular, TAPSI o Mixto."
        );
    }

    // Valida que el plan académico asociado exista.
    private async Task ValidarPlanAcademicoExistenteAsync(
        int idPlanAcademico,
        List<string> errores)
    {
        SistemaHorarios.Modelos.Entidades.PlanAcademico? planAcademico =
            await planAcademicoRepository.ObtenerPorIdAsync(idPlanAcademico);

        AgregarErrorSi(
            planAcademico == null,
            errores,
            "El plan académico asociado no existe."
        );
    }

    // Normaliza los datos iniciales de un grupo nuevo.
    private void PrepararGrupoNuevo(Grupo grupoNuevo)
    {
        grupoNuevo.Codigo = grupoNuevo.Codigo.Trim();
        grupoNuevo.Nombre = grupoNuevo.Nombre.Trim();
        grupoNuevo.Jornada = grupoNuevo.Jornada.Trim();
        grupoNuevo.TipoGrupo = grupoNuevo.TipoGrupo.Trim();
        grupoNuevo.Materia = grupoNuevo.Materia?.Trim() ?? string.Empty;
        grupoNuevo.Dias = grupoNuevo.Dias?.Trim() ?? string.Empty;
        grupoNuevo.Activo = true;
    }

    // Prepara los datos que serán actualizados en un grupo existente.
    private void PrepararGrupoModificado(int idGrupo, Grupo grupoModificado)
    {
        grupoModificado.IdGrupo = idGrupo;
        grupoModificado.Codigo = grupoModificado.Codigo.Trim();
        grupoModificado.Nombre = grupoModificado.Nombre.Trim();
        grupoModificado.Jornada = grupoModificado.Jornada.Trim();
        grupoModificado.TipoGrupo = grupoModificado.TipoGrupo.Trim();
        grupoModificado.Materia = grupoModificado.Materia?.Trim() ?? string.Empty;
        grupoModificado.Dias = grupoModificado.Dias?.Trim() ?? string.Empty;
    }

    // Agrega un error cuando se cumple una condición inválida.
    private void AgregarErrorSi(bool condicion, List<string> errores, string mensaje)
    {
        if (condicion)
        {
            errores.Add(mensaje);
        }
    }
}