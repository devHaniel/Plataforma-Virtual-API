# LMS API - Plataforma de Aula Virtual

API REST desarrollada con ASP.NET Core siguiendo Clean Architecture para la gestión de cursos, módulos, lecciones, tareas y entregas.

El proyecto está diseñado para funcionar como backend principal de una plataforma educativa tipo LMS, permitiendo en el futuro integrar clientes frontend como Angular, aplicaciones móviles u otros consumidores externos.

---

# Características principales

- Autenticación JWT
- Roles y autorización
- Gestión de cursos
- Gestión de módulos y lecciones
- Sistema de tareas
- Entregas de estudiantes
- Subida y descarga de archivos
- Solicitudes para convertirse en profesor
- Arquitectura escalable basada en Clean Architecture

---

# Tecnologías utilizadas

- ASP.NET Core
- Entity Framework Core
- SQL Server
- Swagger
- JWT Authentication
- Angular *(integración futura)*

---

# Arquitectura

El proyecto utiliza Clean Architecture separando responsabilidades en distintas capas:

```txt
WebApi
 ↓
Application
 ↓
Domain
```

Infrastructure contiene:

- Entity Framework
- Repositories
- JWT
- Manejo de archivos
- Servicios técnicos

---

# Roles del sistema

## Admin

- Gestionar usuarios
- Aprobar profesores
- Gestionar cualquier curso

---

## Profesor

- Crear cursos
- Gestionar módulos y lecciones
- Crear tareas
- Calificar entregas

---

## Estudiante

- Inscribirse a cursos
- Enviar tareas
- Descargar archivos

---

# Funcionalidades implementadas

## Auth

- Register
- Login
- JWT Authentication

---

## Cursos

- CRUD de cursos
- Relación profesor-curso

---

## Módulos

- CRUD de módulos
- Organización por curso

---

## Lecciones

- CRUD de lecciones
- Contenido y videos

---

## Tareas

- Creación de tareas
- Fechas límite
- Puntajes

---

## Entregas

- Envío de archivos
- Descarga de archivos
- Calificaciones
- Retroalimentación

---

# Sistema de archivos

Los archivos enviados por estudiantes se almacenan físicamente en:

```txt
uploads/entregas
```

La base de datos únicamente almacena la ruta del archivo.

---

# Autenticación

La API utiliza JWT Bearer Authentication.

Ejemplo:

```http
Authorization: Bearer TU_TOKEN
```

---

# Cómo ejecutar el proyecto

## 1. Clonar repositorio

```bash
git clone REPOSITORIO
```

---

## 2. Restaurar paquetes

```bash
dotnet restore
```

---

## 3. Configurar appsettings.json

```json
"ConnectionStrings": {
  "DefaultConnection": "..."
}
```

```json
"Jwt": {
  "Key": "CLAVE_SUPER_SECRETA",
  "Issuer": "LMS.Api",
  "Audience": "LMS.Users"
}
```

---

## 4. Ejecutar migraciones

```bash
dotnet ef database update -p Infrastructure -s WebApi
```

---

## 5. Ejecutar API

```bash
dotnet run
```

---

# Swagger

La documentación Swagger estará disponible en:

```txt
/swagger
```

---

# Objetivo del proyecto

Este proyecto tiene como objetivo practicar y demostrar conocimientos en:

- ASP.NET Core
- Clean Architecture
- JWT Authentication
- Entity Framework Core
- APIs REST
- Manejo de archivos
- Arquitectura backend escalable
- Seguridad y autorización por roles
