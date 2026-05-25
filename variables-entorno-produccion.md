# Variables de entorno para publicar la API

Configura estas variables en el servicio donde publiques la API.

```text
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=server=HOST;port=PUERTO;database=NOMBRE_BD;user=USUARIO;password=CLAVE;SslMode=Required
Jwt__Key=CAMBIA_ESTA_CLAVE_POR_UNA_MUY_SEGURA
Jwt__Issuer=SistemaHorariosAPI
Jwt__Audience=SistemaHorariosFrontend
Jwt__ExpiresInMinutes=120
Swagger__Enabled=true
```

Notas:

- No uses `root` en producción.
- No subas contraseñas reales a GitHub.
- Para probar localmente se seguirá usando `appsettings.Development.json`.
