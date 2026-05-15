# Modelo Relacional Académico

## Objetivo

Definir la estructura relacional del sistema académico para garantizar:

- integridad de datos,
- escalabilidad,
- mantenibilidad,
- correcta generación de horarios,
- correcta asignación académica.

---

# Flujo General Académico

```txt
Usuarios
↓
Docentes / Estudiantes
↓
Materias
↓
Plan Académico
↓
Grupos
↓
Disponibilidad Docente
↓
Horarios
```

---

# Entidades Principales

## Usuario

Representa la identidad principal dentro del sistema.

### Responsabilidades

- autenticación,
- autorización,
- JWT,
- roles,
- acceso al sistema.

### Relaciones

```txt
Usuario
    1:1
Docente

Usuario
    1:1
Estudiante
```

---

## Rol

Controla permisos y acceso dentro del sistema.

### Relaciones

```txt
Rol
    1:N
Usuario
```

---

## Docente

Representa docentes del sistema académico.

### Responsabilidades

- asignación académica,
- disponibilidad,
- carga horaria,
- asignación materias.

### Relaciones

```txt
Docente
    N:M
Materia

Docente
    1:N
DisponibilidadDocente

Docente
    1:N
Horario
```

---

## Estudiante

Representa estudiantes del sistema.

### Relaciones

```txt
Estudiante
    N:1
PlanAcademico
```

---

## Materia

Representa materias académicas del programa.

### Relaciones

```txt
Materia
    N:M
Docente

Materia
    N:M
Materia
    (Prerrequisitos)

Materia
    1:N
Grupo

Materia
    N:M
PlanAcademico
```

---

## Prerrequisitos

Representa relaciones entre materias.

Ejemplo:

```txt
Programación II
→ requiere
Programación I
```

### Modelo recomendado

```txt
Materia
    N:M
Materia
```

Utilizando tabla puente:

```txt
Prerrequisito
```

---

## PlanAcademico

Representa estructura curricular del programa.

### Relaciones

```txt
PlanAcademico
    1:N
PlanMateria
```

---

## PlanMateria

Tabla puente entre:

- materias,
- semestres,
- planes académicos.

### Relaciones

```txt
PlanMateria
    N:1
Materia

PlanMateria
    N:1
PlanAcademico
```

---

## Grupo

Representa grupos académicos.

### Relaciones

```txt
Grupo
    N:1
Materia

Grupo
    1:N
Horario
```

---

## DisponibilidadDocente

Representa horarios disponibles de docentes.

### Relaciones

```txt
DisponibilidadDocente
    N:1
Docente
```

---

## Horario

Representa asignaciones horarias del sistema.

### Relaciones

```txt
Horario
    N:1
Grupo

Horario
    N:1
Docente

Horario
    N:1
FranjaHoraria
```

---

## FranjaHoraria

Representa bloques horarios institucionales.

Ejemplo:

```txt
Lunes 6:00am - 8:00am
Martes 2:00pm - 4:00pm
```

### Relaciones

```txt
FranjaHoraria
    1:N
Horario
```

---

# Relaciones Críticas del Sistema

## Docente ↔ Materia

```txt
N:M
```

Un docente puede dictar muchas materias.

Una materia puede ser dictada por varios docentes.

---

## Materia ↔ Materia

```txt
N:M
```

Para manejo de prerrequisitos.

---

## Grupo ↔ Materia

```txt
N:1
```

Un grupo pertenece a una materia.

---

## Horario ↔ Docente

```txt
N:1
```

Un horario tiene asignado un docente.

---

# Objetivo Final del Modelo

Este modelo permitirá posteriormente:

- generación automática de horarios,
- control de cruces,
- validación de prerrequisitos,
- asignación docente,
- gestión académica,
- control curricular,
- gestión de disponibilidad,
- manejo de TAPSI y homologaciones.