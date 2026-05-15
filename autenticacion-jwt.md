# Autenticación JWT

## Objetivo

El sistema utiliza JWT (JSON Web Token) para gestionar la autenticación y autorización de usuarios dentro de la API.

La implementación permite:

- autenticación segura,
- protección de endpoints,
- autorización basada en roles,
- identificación del usuario autenticado,
- control de acceso mediante policies.

---

# Flujo de Autenticación

```txt
Usuario
↓
POST /api/auth/login
↓
AuthController
↓
AuthService
↓
Validación credenciales
↓
PasswordService
↓
Generación JWT
↓
Respuesta token
```

---

# Componentes Relacionados

## Controllers

- AuthController

## Servicios

- IAuthService
- AuthService
- JwtService
- PasswordService

## Configuraciones

- ConfiguracionJWT

## Middleware

- ExceptionMiddleware

## DTOs

- LoginRequestDto
- LoginResponseDto
- RegistroUsuarioDto
- PerfilUsuarioDto

---

# Endpoints de Autenticación

## POST /api/auth/login

Permite iniciar sesión en el sistema.

### Proceso

- valida credenciales,
- verifica contraseña utilizando BCrypt,
- genera token JWT,
- devuelve información autenticada.

### Request

```json
{
  "correoInstitucional": "usuario@uam.edu.co",
  "contrasena": "123456"
}
```

### Response

```json
{
  "success": true,
  "message": "Login exitoso",
  "data": {
    "token": "jwt-token"
  }
}
```

---

## POST /api/auth/registrar

Permite registrar usuarios dentro del sistema.

### Proceso

- valida datos,
- hashea contraseña,
- almacena usuario en base de datos.

### Request

```json
{
  "nombreCompleto": "Juan Perez",
  "correoInstitucional": "juan@uam.edu.co",
  "contrasena": "123456"
}
```

---

## GET /api/auth/perfil

Obtiene información del usuario autenticado.

### Requiere

- token JWT válido,
- autenticación Bearer.

### Header

```txt
Authorization: Bearer {token}
```

---

# Claims JWT

El token JWT almacena información básica del usuario autenticado.

## Claims actuales

- IdUsuario
- CorreoInstitucional
- NombreCompleto
- Rol

Estos claims permiten:

- identificar usuarios,
- validar permisos,
- proteger endpoints,
- aplicar autorización basada en roles.

---

# Policies de Autorización

Actualmente el sistema cuenta con las siguientes policies:

- SoloAdministradores
- GestionMaterias
- VerHorarios

Las policies son utilizadas mediante:

```csharp
[Authorize(Policy = "SoloAdministradores")]
```

---

# Seguridad Implementada

El sistema actualmente utiliza:

- JWT Bearer Authentication,
- autorización basada en policies,
- middleware global de excepciones,
- validaciones automáticas DTO,
- BCrypt para contraseñas,
- autenticación mediante claims,
- protección de endpoints con `[Authorize]`.

---

# BCrypt

Las contraseñas NO son almacenadas en texto plano.

El sistema utiliza BCrypt para:

- hash de contraseñas,
- validación segura,
- protección de credenciales.

### Flujo

```txt
Contraseña usuario
↓
PasswordService
↓
BCrypt Hash
↓
Base de datos
```

---

# Swagger + JWT

Swagger se encuentra configurado para soportar autenticación JWT.

Esto permite:

- probar endpoints protegidos,
- autenticarse desde Swagger UI,
- enviar tokens Bearer automáticamente.

---

# Middleware de Excepciones

El sistema utiliza:

- ExceptionMiddleware

para capturar errores globales y devolver respuestas JSON estandarizadas.

Ejemplo:

```json
{
  "success": false,
  "message": "Ocurrió un error interno"
}
```

---

# Estado Actual

Actualmente la autenticación ya cuenta con:

✅ Login funcional  
✅ Registro funcional  
✅ JWT funcional  
✅ Claims configurados  
✅ BCrypt implementado  
✅ Policies configuradas  
✅ Swagger integrado con JWT  
✅ Middleware global de excepciones  
✅ DTOs validados automáticamente  
✅ Endpoints protegidos con Authorize  