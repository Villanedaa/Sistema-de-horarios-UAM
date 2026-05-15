# Arquitectura Backend

## Arquitectura General

El proyecto utiliza una arquitectura por capas con separación de responsabilidades para garantizar:

- mantenibilidad,
- escalabilidad,
- modularidad,
- reutilización de código,
- facilidad de pruebas,
- crecimiento organizado del sistema.

La solución se encuentra dividida en proyectos especializados, donde cada capa tiene una responsabilidad específica dentro de la aplicación.

---

# Flujo General del Sistema

```txt
Frontend
↓
API
↓
Lógica de Negocio
↓
Persistencia de Datos
↓
MySQL
```

---

# Estructura Actual de la Solución

```txt
SistemaHorarios
│
├── SistemaHorarios.API
├── SistemaHorarios.Logica
├── SistemaHorarios.Datos
├── SistemaHorarios.Modelos
├── SistemaHorarios.Pruebas
└── docs
```

---

# Proyectos de la Solución

## SistemaHorarios.API

Capa encargada de exponer la API REST del sistema.

### Responsabilidades

- endpoints HTTP,
- autenticación JWT,
- autorización,
- Swagger/OpenAPI,
- middleware,
- configuración del pipeline HTTP,
- políticas de acceso,
- controladores.

### Componentes actuales

#### Configuraciones

- ConfiguracionSwagger
- ConfiguracionJWT
- ConfiguracionComportamientoAPI

#### Middleware

- ExceptionMiddleware

#### Politicas

- PoliticasAutorizacion

#### Extensions

- ExtensionesServicios
- ExtensionesAplicacion

#### Controllers

- AuthController

---

## SistemaHorarios.Logica

Capa encargada de la lógica de negocio del sistema.

### Responsabilidades

- reglas académicas,
- validaciones de negocio,
- autenticación,
- gestores,
- procesos académicos,
- lógica del dominio.

### Módulos actuales

#### Auth

- IAuthService
- AuthService
- JwtService
- PasswordService

#### Materias

- GestorMateria
- GestorPrerrequisito

---

## SistemaHorarios.Datos

Capa encargada de persistencia y acceso a datos.

### Responsabilidades

- Entity Framework Core,
- DbContext,
- repositorios,
- conexión MySQL,
- migraciones,
- persistencia de datos.

### Componentes actuales

#### Contexto

- SistemaHorariosDbContext

#### Repositorios

- MateriaRepository
- PrerrequisitoRepository

#### Migraciones

Migraciones de Entity Framework Core para control de versiones de base de datos.

---

## SistemaHorarios.Modelos

Proyecto compartido encargado de centralizar modelos y objetos comunes del sistema.

### Responsabilidades

- entidades,
- DTOs,
- responses,
- objetos compartidos.

### Entidades actuales

- Usuario
- Rol
- Materia
- Prerrequisito

### DTOs actuales

#### Auth

- LoginRequestDto
- LoginResponseDto
- RegistroUsuarioDto
- PerfilUsuarioDto

### Responses

- ApiResponse<T>

---

## SistemaHorarios.Pruebas

Proyecto destinado a pruebas del sistema.

### Responsabilidades

- pruebas unitarias,
- validaciones,
- testing,
- pruebas de lógica de negocio.

Actualmente se encuentra en preparación para futuras implementaciones de testing automatizado.

---

# Arquitectura Actual del Backend

Actualmente el backend ya cuenta con:

- arquitectura modular,
- separación por capas,
- autenticación JWT,
- autorización basada en roles,
- middleware global de excepciones,
- Swagger integrado,
- Entity Framework Core,
- conexión MySQL,
- repositorios,
- gestores de negocio,
- DTOs,
- respuestas estandarizadas,
- configuración modular mediante extensiones,
- pipeline HTTP desacoplado,
- dependency injection.

---

# Tecnologías Utilizadas

## Backend

- .NET 8
- ASP.NET Core
- Entity Framework Core
- Pomelo EntityFrameworkCore MySQL
- Swagger / OpenAPI
- JWT Authentication
- BCrypt

## Base de Datos

- MySQL

## Arquitectura y Herramientas

- Dependency Injection
- Middleware Pipeline
- Extension Methods
- Arquitectura por capas
- REST API

---

# Estado Actual del Proyecto

Actualmente el proyecto ya tiene implementado:

✅ Arquitectura backend sólida  
✅ API REST funcional  
✅ Autenticación JWT  
✅ Middleware global de excepciones  
✅ Swagger con autenticación JWT  
✅ Entity Framework Core  
✅ Conexión MySQL  
✅ Repositorios  
✅ Gestores de lógica de negocio  
✅ DTOs y Responses  
✅ Separación de responsabilidades  
✅ Program.cs modularizado  
✅ Pipeline HTTP desacoplado  
✅ Configuración centralizada mediante extensiones  