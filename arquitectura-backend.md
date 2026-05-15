# Arquitectura Backend

## Arquitectura General

El proyecto utiliza arquitectura por capas para separar responsabilidades, mejorar mantenibilidad y facilitar el crecimiento del sistema.

La solución está dividida en diferentes proyectos especializados.

## Flujo General

Frontend
↓
API
↓
Lógica de negocio
↓
Persistencia de datos
↓
MySQL

## Proyectos

### SistemaHorarios.API

Responsable de:

- Endpoints HTTP
- JWT
- Swagger
- Policies
- Middlewares
- Configuración general
- Controladores

### SistemaHorarios.Logica

Responsable de:

- Reglas de negocio
- Validaciones de negocio
- Servicios
- Gestores
- Procesos académicos

### SistemaHorarios.Datos

Responsable de:

- Entity Framework Core
- DbContext
- Repositorios
- Migraciones
- SeedData
- Persistencia MySQL

### SistemaHorarios.Modelos

Responsable de:

- Entidades
- DTOs
- Responses
- Objetos compartidos

### SistemaHorarios.Pruebas

Responsable de:

- pruebas unitarias
- validaciones
- testing


- .NET 8
- ASP.NET Core
- Entity Framework Core
- Pomelo MySQL
- MySQL
- Swagger
- JWT