# Autenticación JWT

## Objetivo

El sistema utiliza JWT para autenticación y autorización de usuarios.

## Flujo autenticación

Usuario
↓
POST /api/auth/login
↓
AuthService
↓
Validación credenciales
↓
Generación JWT
↓
Respuesta token

## Endpoints

### POST /api/auth/login

Permite iniciar sesión.

### POST /api/auth/registrar

Permite registrar usuarios.

### GET /api/auth/perfil

Obtiene información del usuario autenticado.

## Claims JWT

El token almacena:

- IdUsuario
- CorreoInstitucional
- NombreCompleto
- Rol

## Policies

Actualmente existen:

- SoloAdministradores
- GestionMaterias
- VerHorarios

## Seguridad

El sistema utiliza:

- JWT Bearer Authentication
- Policies de autorización
- Middleware global de excepciones
- Validaciones automáticas DTO