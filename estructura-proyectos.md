# Estructura Proyectos

## SistemaHorarios.API

### Configuraciones

- ConfiguracionJwt
- ConfiguracionSwagger
- ConfiguracionComportamientoApi

### Politicas

- PoliticasAutorizacion

### Middlewares

- ExceptionMiddleware

### Controllers

- AuthController

## SistemaHorarios.Logica

### Negocio

#### Auth

- AuthService
- JwtService
- PasswordService

#### Materias

- GestorMateria
- GestorPrerrequisito

## SistemaHorarios.Datos

### Contexto

- SistemaHorariosDbContext

### Repositorios

- MateriaRepository
- PrerrequisitoRepository

### Migraciones

Migraciones Entity Framework Core.

## SistemaHorarios.Modelos

### Entidades

- Usuario
- Rol
- Materia

### DTOs

#### Auth

- LoginRequestDto
- LoginResponseDto
- RegistroUsuarioDto
- PerfilUsuarioDto

### Responses

- ApiResponse<T>