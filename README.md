# Ecosistema ITM Nivel 5 - Taller Final Integrador

Este repositorio contiene el proyecto final integrador para la asignatura de Programación de Software. La solución implementa un sistema completo (Backend API y Frontend Móvil) para el Módulo de Matrículas del ITM, aplicando principios de arquitectura limpia, seguridad y buenas prácticas de desarrollo.

---

## Arquitectura de la Solución

El proyecto está estructurado bajo los lineamientos de Arquitectura Limpia (Clean Architecture), lo que permite separar la lógica de negocio central de los detalles de infraestructura (bases de datos, frameworks y controladores HTTP).

```text
               ┌───────────────────────────────────────────────┐
               │              GestionITM.AppMovil              │ (Frontend MAUI)
               └───────────────────────┬───────────────────────┘
                                       │ (Peticiones HTTP con JWT)
                                       ▼
 ┌───────────────────────────────────────────────────────────────────────────┐
 │                            GestionITM.API                                 │ (Presentación API)
 └─────────────────────────────────────┬─────────────────────────────────────┘
                                       │ 
                                       ▼
 ┌───────────────────────────────────────────────────────────────────────────┐
 │                        GestionITM.Infrastructure                          │ (Acceso a Datos/Servicios)
 └─────────────────────────────────────┬─────────────────────────────────────┘
                                       │ (Implementa interfaces)
                                       ▼
 ┌───────────────────────────────────────────────────────────────────────────┐
 │                          GestionITM.Domain                                │ (Lógica Core / Entidades)
 └───────────────────────────────────────────────────────────────────────────┘
```

### Estructura de Directorios

La solución se divide en las siguientes carpetas:

- **`src/`** (Código fuente de producción):
  - **`GestionITM.Domain/`**: Núcleo de la aplicación. Define las entidades de negocio (`Matricula`, `Curso`, `Estudiante`), las interfaces de servicios y repositorios (`IMatriculaService`, `IMatriculaRepository`), y los DTOs (`MatriculaCreateDto`, `MatriculaDto`). No tiene dependencias externas.
  - **`GestionITM.Infrastructure/`**: Implementa la persistencia de datos mediante Entity Framework Core con SQL Server, la configuración del contexto de base de datos (`ApplicationDbContext`), los repositorios concretos y el servicio de matrículas (`MatriculaService`).
  - **`GestionITM.API/`**: Capa de presentación HTTP. Contiene los controladores (`MatriculaController` protegido con JWT, `CursoController` con soporte para paginación) y el middleware para el manejo global de excepciones.
  - **`GestionITM.AppMovil/`**: Aplicación móvil multiplataforma desarrollada en .NET MAUI. Sigue el patrón MVVM e incluye un interceptor de red para adjuntar el token JWT, soporte para scroll infinito y un manejo de errores robusto.
- **`tests/`** (Pruebas del sistema):
  - **`GestionITM.Tests/`**: Pruebas unitarias escritas en xUnit para validar las reglas de negocio de la capa de servicio.
- **`postman/`** (Pruebas de endpoints):
  - Colección de solicitudes HTTP en formato JSON lista para importar en Postman.
- **`.github/workflows/`** (Integración Continua):
  - Pipeline de GitHub Actions (`ci.yml`) que compila la solución y ejecuta las pruebas de forma automatizada.

---

## Tecnologías y Características Implementadas

- **Backend:** ASP.NET Core Web API con .NET 8.
- **Base de Datos:** SQL Server gestionado a través de Entity Framework Core.
- **Seguridad:** Autenticación y autorización mediante Tokens JWT con validación de roles (se requiere el rol de Estudiante para realizar una matrícula).
- **Paginación:** Implementación del patrón `PagedResult<T>` en el backend y consumo mediante scroll infinito en la aplicación móvil.
- **Reglas de Negocio:** Validación controlada a nivel de servicio para verificar la disponibilidad de cupos en un curso antes de confirmar la matrícula.
- **DevOps:** Dockerización multietapa para la API y orquestación del entorno completo mediante Docker Compose.
- **Observabilidad:** Registro de eventos del sistema y errores en archivos físicos diarios utilizando Serilog, guardados en el directorio `/Logs`.

---

## Instrucciones para Ejecutar la Solución

### Opción A: Ejecución en contenedores con Docker Compose

Es posible levantar toda la infraestructura (Base de datos SQL Server y la API Web) ejecutando el siguiente comando en la raíz del repositorio:

```bash
docker-compose up -d --build
```

Este comando inicia los siguientes servicios dentro de una red privada de Docker:
- **Base de Datos (`itm-database`):** Instancia de SQL Server escuchando en el puerto `1433`.
- **API Backend (`itm-api`):** API Web disponible en `http://localhost:8080`, con la documentación de Swagger accesible en `http://localhost:8080/swagger`.

Para detener los servicios:
```bash
docker-compose down
```

### Opción B: Ejecución nativa (API + interfaz móvil)

La **API en terminal** solo muestra Swagger (`http://localhost:5016/swagger`). La **interfaz móvil** es el proyecto `GestionITM.AppMovil` y se ejecuta aparte (emulador Android o Visual Studio).

#### 1) Levantar la API (terminal en la raíz del repo)

```powershell
$env:JWT_KEY='ClaveJwtMinimo32CaracteresParaHMAC256!!'
dotnet run --project src\GestionITM.API
```

#### 2) Ejecutar la app móvil (Visual Studio 2022)

1. Abra `EcosistemaITM.slnx`.
2. Instale la carga de trabajo **.NET MAUI** y **Desarrollo para Android**.
3. Clic derecho en `GestionITM.AppMovil` → **Establecer como proyecto de inicio**.
4. En la barra superior elija un **emulador Android** (no "Windows Machine").
5. Pulse **F5** (o el botón ▶ verde).

Verá la pantalla de **Iniciar Sesión** (no Swagger). Credenciales demo:

- Email: `estudiante.demo@correo.itm.edu.co`
- Contraseña: `ItmDemo2026!`

#### 3) URL de la API en el emulador

En `MauiProgram.cs`, método `GetApiBaseUrl()`: use puerto **5016** si la API corre con `dotnet run`, o **8080** si usa Docker. El emulador usa `http://10.0.2.2:{puerto}/`.

#### Migraciones (si la base está vacía)

```powershell
dotnet ef database update --project src\GestionITM.Infrastructure --startup-project src\GestionITM.API
```

### GitHub Actions (CI)

El pipeline está en [`.github/workflows/ci.yml`](.github/workflows/ci.yml). Se ejecuta al hacer `git push` a GitHub (ramas `main`, `master` o `develop`). Revise la pestaña **Actions** del repositorio en GitHub.

---

## Pruebas de Endpoints con Postman

En la carpeta [postman/](file:///c:/Users/santi/OneDrive/Documents/Programacion/EcosistemaITM/postman/) se incluye la colección JSON para realizar pruebas.

La colección contiene los siguientes flujos preparados:
1. **Iniciar Sesión:** Permite enviar las credenciales del estudiante, obtener el token JWT y configurarlo de forma automática como variable de entorno en Postman.
2. **Cursos Paginados:** Consulta el catálogo de cursos paginados para simular el comportamiento del scroll infinito.
3. **Matricular Curso:** Envía una solicitud de matrícula autenticada con token JWT.
4. **Matricular Curso (Sin Cupo):** Valida la regla de negocio al intentar matricular un curso sin cupos disponibles, verificando que la API retorne un código de respuesta `400 BadRequest` con un mensaje descriptivo.

---

## Guía de Sustentación y Entrega

En la raíz del proyecto se encuentra el archivo `GUIA_ENTREGA.md`, el cual contiene indicaciones detalladas sobre cómo estructurar el documento PDF final y un guion sugerido para la grabación del video demostrativo de sustentación.
