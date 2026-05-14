# Configuración Desarrollo

## Requisitos

- .NET 8 SDK
- MySQL
- Visual Studio 2022
- Docker Desktop
- Git

## Restaurar paquetes

dotnet restore

## Ejecutar backend

dotnet run --project SistemaHorarios.API

## Crear migración

dotnet ef migrations add NombreMigracion --project SistemaHorarios.Datos --startup-project SistemaHorarios.API

## Aplicar migraciones

dotnet ef database update --project SistemaHorarios.Datos --startup-project SistemaHorarios.API

## Base de datos

El proyecto utiliza:

- MySQL
- Entity Framework Core
- Pomelo.EntityFrameworkCore.MySql

## Swagger

Swagger se encuentra disponible en:

/swagger

## ZeroTier

ZeroTier se utiliza para pruebas remotas frontend-backend durante desarrollo.

## Arquitectura API

La API utiliza:

- JWT
- Middleware global excepciones
- Policies
- DTO Validation
- ApiResponse estándar