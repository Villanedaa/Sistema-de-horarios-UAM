# SistemaHorarios Backend API

Backend del sistema de gestión y generación de horarios académicos de la UAM.  
Este proyecto expone una API REST desarrollada en **.NET 8**, organizada por capas, con conexión a **MySQL** y documentación mediante **Swagger**.

## 1. Objetivo del backend

El backend permite administrar la información académica necesaria para generar horarios, incluyendo:

- Autenticación de usuarios.
- Gestión de roles.
- Gestión de coordinadores.
- Gestión de docentes.
- Gestión de materias.
- Gestión de grupos académicos.
- Gestión de planes académicos.
- Generación y administración de horarios.
- Consulta de reportes académicos.

La lógica principal del sistema está enfocada en generar horarios académicos evitando conflictos entre docentes, grupos, materias y franjas horarias.

## 2. Tecnologías utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- MySQL
- Swagger / OpenAPI
- Arquitectura por capas

## 3. Estructura del proyecto

```text
Sistema-de-horarios-UAM
│
├── SistemaHorarios.API
│   ├── Controllers
│   ├── DTOs
│   ├── Program.cs
│   └── appsettings.json
│
├── SistemaHorarios.Logica
│   └── Negocio
│       ├── Seguridad
│       ├── Docentes
│       ├── Materias
│       ├── Grupos
│       ├── PlanAcademico
│       ├── Horarios
│       └── Reportes
│
├── SistemaHorarios.Datos
│   ├── Contexto
│   ├── Repositorios
│   └── Migrations
│
└── SistemaHorarios.Modelos
    ├── Entidades
    └── DTOs
```

## 4. Arquitectura

El proyecto mantiene separación por capas:

```text
API → Lógica → Datos → Base de datos
```

### SistemaHorarios.API

Contiene los controladores REST, configuración de Swagger, autenticación, autorización y punto de entrada de la aplicación.

### SistemaHorarios.Logica

Contiene la lógica de negocio del sistema. Aquí se validan reglas académicas, disponibilidad docente, generación de horarios, reportes y operaciones principales.

### SistemaHorarios.Datos

Contiene el `DbContext`, repositorios, configuración de Entity Framework Core y acceso a MySQL.

### SistemaHorarios.Modelos

Contiene las entidades principales y DTOs compartidos entre capas.

## 5. Requisitos previos

Antes de ejecutar el backend se debe tener instalado:

- .NET SDK 8
- MySQL Server, Laragon o XAMPP
- Visual Studio, Visual Studio Code o Rider
- Git, opcional

Para verificar .NET:

```powershell
dotnet --version
```

## 6. Configuración de base de datos

El backend usa MySQL. La cadena de conexión se encuentra en:

```text
SistemaHorarios.API/appsettings.json
```

Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=sistema_horarios;user=root;password=;"
  }
}
```

Si MySQL tiene contraseña:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=sistema_horarios;user=root;password=Root123456;"
  }
}
```

## 7. Restaurar paquetes

Desde la raíz del backend:

```powershell
dotnet restore
```

## 8. Compilar el backend

```powershell
dotnet build
```

Si la compilación termina correctamente, se mostrará un mensaje similar a:

```text
Compilación realizada correctamente.
```

## 9. Ejecutar migraciones

Si la base de datos aún no existe o faltan tablas:

```powershell
dotnet ef database update --project SistemaHorarios.Datos --startup-project SistemaHorarios.API
```

Para crear una nueva migración:

```powershell
dotnet ef migrations add NombreMigracion --project SistemaHorarios.Datos --startup-project SistemaHorarios.API
```

## 10. Ejecutar la API

```powershell
dotnet run --project SistemaHorarios.API/SistemaHorarios.API.csproj
```

La API quedará disponible en el puerto mostrado por la consola.

Ejemplo:

```text
https://localhost:7208
```

Swagger se puede abrir en:

```text
https://localhost:7208/swagger
```

## 11. Módulos principales

### Autenticación

Permite iniciar sesión, obtener información del usuario autenticado y controlar el acceso según rol.

Endpoints principales:

```http
POST /api/auth/login
POST /api/auth/logout
GET /api/auth/perfil
```

### Coordinadores / Usuarios

Permite crear, editar, activar e inactivar coordinadores.  
Si un coordinador está inactivo, no puede iniciar sesión.

Endpoints principales:

```http
GET /api/usuarios
GET /api/usuarios/coordinadores
POST /api/usuarios
PUT /api/usuarios/{id}
PUT /api/usuarios/{id}/estado
DELETE /api/usuarios/{id}
```

### Docentes

Permite gestionar docentes, sus estados, disponibilidad y materias que pueden dictar.

Endpoints principales:

```http
GET /api/docentes
GET /api/docentes/{id}
POST /api/docentes
PUT /api/docentes/{id}
DELETE /api/docentes/{id}
GET /api/docentes/{id}/disponibilidad
PUT /api/docentes/{id}/disponibilidad
```

### Materias

Permite administrar las materias del plan académico, incluyendo código, nombre, créditos, intensidad horaria y estado.

Endpoints principales:

```http
GET /api/materias
GET /api/materias/{id}
POST /api/materias
PUT /api/materias/{id}
DELETE /api/materias/{id}
```

### Grupos académicos

Permite administrar los grupos de estudiantes según plan, semestre, jornada y tipo.

Endpoints principales:

```http
GET /api/grupos
GET /api/grupos/{id}
POST /api/grupos
PUT /api/grupos/{id}
DELETE /api/grupos/{id}
```

### Plan académico

Permite consultar planes, semestres, mallas curriculares y materias asociadas.

Endpoints principales:

```http
GET /api/plan-academico
GET /api/plan-academico/{id}
GET /api/plan-academico/{id}/semestres
GET /api/plan-academico/{id}/semestres/{numeroSemestre}
POST /api/plan-academico
PUT /api/plan-academico/{id}
```

### Horarios

El módulo de horarios genera y administra horarios académicos por grupo.

Reglas principales:

- El horario se genera por grupo académico.
- El horario del grupo representa el horario del estudiante.
- El horario del docente se consulta a partir de los horarios generados.
- Las clases se asignan en bloques de 2 horas.
- Materia con intensidad de 2 horas genera 1 bloque.
- Materia con intensidad de 4 horas genera 2 bloques.
- Una misma materia no debe quedar el mismo día para el mismo grupo.
- Una misma materia no debe quedar en días consecutivos.
- No se permiten cruces de grupo.
- No se permiten cruces de docente.
- El docente debe tener disponibilidad en la franja asignada.
- El docente debe poder dictar la materia.
- No se permiten clases entre 12:00 y 14:00.
- No se permiten clases entre 18:00 y 18:30.
- Se permite aprobar o rechazar un horario.
- Se permite editar un bloque si cumple las validaciones.

Endpoints principales:

```http
GET /api/horarios
GET /api/horarios/grupo/{idGrupo}
GET /api/horarios/docente/{idDocente}
POST /api/horarios/generar/{idGrupo}
PUT /api/horarios/{id}
PUT /api/horarios/{id}/aceptar
PUT /api/horarios/{id}/rechazar
DELETE /api/horarios/{id}
```

### Reportes

Permite consultar y exportar información académica.

Reportes disponibles:

- Horarios generados.
- Horario por grupo.
- Horario por docente.
- Carga docente.
- Conflictos de horarios.

Endpoints principales:

```http
GET /api/reportes
GET /api/reportes/tipos
GET /api/reportes/vista-previa
GET /api/reportes/descargar
```

## 12. Estados importantes

### Usuarios y docentes

```text
Activo
Inactivo
```

### Horarios

```text
Pendiente
Aprobado
Rechazado
```

## 13. Validaciones del sistema

El backend valida que:

- No se creen usuarios con datos duplicados.
- Un usuario inactivo no pueda iniciar sesión.
- Un docente inactivo no sea asignado a nuevos horarios.
- Un docente solo dicte materias asociadas a él.
- No existan cruces de horarios por grupo.
- No existan cruces de horarios por docente.
- La disponibilidad docente sea respetada.
- Las materias se asignen de acuerdo con el grupo y semestre.
- Los bloques de horario respeten la intensidad horaria semanal.

## 14. Ejecutar pruebas manuales desde Swagger

Flujo recomendado:

1. Iniciar sesión.
2. Crear o verificar docentes.
3. Crear o verificar materias.
4. Crear o verificar grupos académicos.
5. Configurar disponibilidad docente.
6. Generar horario por grupo.
7. Ver horario del grupo.
8. Ver horario del docente.
9. Aprobar o rechazar horario.
10. Consultar reportes.

## 15. Publicar backend como ejecutable

Para generar una versión ejecutable para Windows:

```powershell
dotnet publish SistemaHorarios.API/SistemaHorarios.API.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true -o publish/api
```

El ejecutable quedará en:

```text
publish/api
```

Para ejecutarlo:

```powershell
.\SistemaHorarios.API.exe --urls "http://localhost:5000"
```

Swagger quedará disponible en:

```text
http://localhost:5000/swagger
```

## 16. Consideraciones para despliegue en otro computador

Para que el backend funcione en otro computador se necesita:

1. Instalar MySQL.
2. Importar la base de datos.
3. Revisar la cadena de conexión en `appsettings.json`.
4. Ejecutar `SistemaHorarios.API.exe`.
5. Verificar Swagger.

## 17. Problemas comunes

### Error de conexión a base de datos

Revisar:

- Nombre de la base de datos.
- Usuario.
- Contraseña.
- Puerto de MySQL.
- Que MySQL esté iniciado.

### Error 401

Indica que la petición no tiene token válido o el usuario no está autorizado.

### Swagger no abre

Verificar que la API esté ejecutándose y revisar el puerto mostrado en consola.

## 18. Autores

Proyecto académico desarrollado para la gestión y generación de horarios académicos.
