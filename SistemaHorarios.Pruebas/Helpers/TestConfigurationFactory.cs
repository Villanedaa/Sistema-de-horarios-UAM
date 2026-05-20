using Microsoft.Extensions.Configuration;
using SistemaHorarios.Logica.Negocio.Auth;

namespace SistemaHorarios.Pruebas.Helpers;

public static class TestConfigurationFactory
{
    public static IConfiguration CrearConfiguracionJwt()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "clave-super-secreta-de-prueba-minimo-32-chars",
                ["Jwt:Issuer"] = "SistemaHorarios-Test",
                ["Jwt:Audience"] = "SistemaHorarios-Test",
                ["Jwt:ExpiresInMinutes"] = "60"
            })
            .Build();
    }

    public static JwtService CrearJwtService()
    {
        return new JwtService(CrearConfiguracionJwt());
    }
}
