namespace SistemaHorarios.Logica.Negocio.Auth;

/// <summary>
/// Servicio encargado de gestionar
/// hash y validación de contraseñas
/// utilizando BCrypt.
/// </summary>
public class PasswordService
{
    /// <summary>
    /// Genera el hash seguro de una contraseña.
    /// </summary>
    /// <param name="password">
    /// Contraseña texto plano.
    /// </param>
    /// <returns>
    /// Contraseña hasheada.
    /// </returns>
    public string HashPassword(
        string password)
    {
        return BCrypt.Net.BCrypt
            .HashPassword(password);
    }

    /// <summary>
    /// Verifica si una contraseña coincide
    /// con el hash almacenado.
    /// </summary>
    /// <param name="password">
    /// Contraseña texto plano.
    /// </param>
    /// <param name="passwordHash">
    /// Hash almacenado.
    /// </param>
    /// <returns>
    /// True si la contraseña es válida.
    /// </returns>
    public bool VerifyPassword(
        string password,
        string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(
            password,
            passwordHash
        );
    }
}