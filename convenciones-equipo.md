# Convenciones Equipo

## Idioma

Todo el proyecto utiliza nombres en español.

## Branches

Formato:

feature/nombre-feature

Ejemplos:

- feature/auth-jwt
- feature/crud-usuarios
- feature/crud-docentes

## Commits

Formato:

feat:
fix:
refactor:
docs:

Ejemplos:

feat(auth): implementar login JWT

refactor(api): reorganizar arquitectura base

docs: agregar documentación backend

## Arquitectura

Se utiliza arquitectura por capas.

Cada proyecto tiene responsabilidades específicas.

## Validaciones

Las validaciones HTTP deben implementarse mediante DTOs y DataAnnotations.

## Responses

Todas las respuestas deben utilizar ApiResponse<T>.

## Seguridad

Toda funcionalidad protegida debe utilizar:

- JWT
- Policies
- Authorize