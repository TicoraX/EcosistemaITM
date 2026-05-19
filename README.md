# 🎓 Ecosistema ITM Nivel 5 - Taller Final Integrador

¡Bienvenido al **Ecosistema ITM Nivel 5**! Este es el proyecto final integrador de la asignatura de **Programación de Software**. Aquí se implementa una solución completa **End-to-End** (Backend API + Frontend Móvil) que modela el **Módulo de Matrículas** del ITM siguiendo los estándares de seguridad, arquitectura y robustez más rigurosos de la industria.

---

## 🏛️ Arquitectura de la Solución (Clean Architecture)

El proyecto está diseñado bajo los principios de **Clean Architecture** (Arquitectura Limpia), aislando por completo la lógica de negocio central de los detalles tecnológicos (bases de datos, controladores HTTP, frameworks).

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

### 📂 Estructura de Directorios

- **`src/`** (Código de producción):
  - **`GestionITM.Domain/`**: El núcleo de la aplicación. Contiene las entidades (`Matricula`, `Curso`, `Estudiante`), las interfaces de servicios y repositorios (`IMatriculaService`, `IMatriculaRepository`), y los DTOs (`MatriculaCreateDto`, `MatriculaDto`). **100% libre de dependencias externas.**
  - **`GestionITM.Infrastructure/`**: Contiene la implementación de la persistencia de datos con **EF Core / SQL Server** (`ApplicationDbContext`), los repositorios concretos y los servicios que implementan las reglas de negocio (`MatriculaService`).
  - **`GestionITM.API/`**: Capa de presentación HTTP. Contiene los controladores expuestos (`MatriculaController` con seguridad JWT, `CursoController` con paginación) y middlewares globales de excepción.
  - **`GestionITM.AppMovil/`**: Frontend multiplataforma en **.NET MAUI** estructurado en MVVM, con interceptor HTTP seguro para adjuntar el JWT, soporte para carga paginada infinita y alertas elegantes ante errores 400.
- **`tests/`** (Pruebas unitarias):
  - **`GestionITM.Tests/`**: Pruebas automatizadas en xUnit para verificar las reglas de negocio de matrículas.
- **`postman/`** (Colección de pruebas):
  - Contiene la colección lista para importar en Postman y verificar la API en segundos.
- **`.github/workflows/`** (CI/CD):
  - Contiene el pipeline de **GitHub Actions** (`ci.yml`) que compila la solución y corre todas las pruebas unitarias automáticamente en cada Push.

---

## 🚀 Tecnologías y Características Implementadas

- **Backend:** ASP.NET Core Web API / .NET 8.
- **Persistencia:** Entity Framework Core con SQL Server.
- **Seguridad:** Autenticación por Tokens **JWT** con seguridad por Roles (Rol "Estudiante" requerido para matricular).
- **Paginación:** Patrón `PagedResult<T>` en backend y **Scroll Infinito** (`RemainingItemsThresholdReached`) en la App Móvil.
- **Regla de Negocio Sólida:** Excepción controlada si se intenta matricular un curso sin `CuposDisponibles` (Controlado en `MatriculaService`, nunca en los controladores).
- **Dockerización Completa:** Dockerfile multi-etapa y orquestación con Docker Compose.
- **Observabilidad:** Serilog configurado para volcar trazas y errores en archivos diarios dentro del directorio local `/Logs`.

---

## 🛠️ Cómo Ejecutar la Solución

### Opción A: Orquestación Local Completa (Docker Compose)

Puedes levantar todo el ecosistema (Base de Datos SQL Server + API Web) con una sola línea de comandos desde la raíz del repositorio:

```bash
docker-compose up -d --build
```

Esto levantará los siguientes servicios en paralelo en una red privada virtual de Docker:
- **Base de Datos (`itm-database`):** SQL Server escuchando en el puerto `1433`.
- **API Backend (`itm-api`):** Web API en .NET 8 disponible en `http://localhost:8080` (con Swagger activo en `http://localhost:8080/swagger`).

Para apagar la flota:
```bash
docker-compose down
```

### Opción B: Ejecución en Visual Studio o VS Code

1. Abre la solución unificada **`EcosistemaITM.slnx`** en Visual Studio.
2. Asegúrate de configurar las migraciones y aplicar la base de datos si corres de forma nativa:
   ```bash
   dotnet ef database update --project src/GestionITM.Infrastructure --startup-project src/GestionITM.API
   ```
3. Ejecuta el proyecto `GestionITM.API` para levantar el backend y abre el proyecto de la App Móvil `GestionITM.AppMovil` en el emulador de Android o dispositivo iOS de tu preferencia.

---

## 📬 Pruebas con Postman

En la carpeta [postman/](file:///c:/Users/santi/OneDrive/Documents/Programacion/EcosistemaITM/postman/) encontrarás la colección lista para importar en Postman.

La colección contiene:
1. **Iniciar Sesión:** Envía credenciales de un estudiante de prueba, extrae el token JWT devuelto y lo configura automáticamente en las variables del entorno de Postman.
2. **Cursos Paginados:** Obtiene el listado de cursos paginados listo para simular el scroll infinito.
3. **Matricular Curso (Con Cupos):** Petición POST autenticada con JWT para matricular de forma exitosa.
4. **Matricular Curso (Sin Cupos - Error 400):** Verifica que al matricular un curso sin cupos disponibles, la API devuelva un código `400 BadRequest` con un mensaje amigable estructurado por la regla de negocio.

---

## 📄 Guía para la Sustentación Final

Encuentra las pautas completas, estructura recomendada para tu reporte PDF y el guion sugerido para tu video de sustentación de 3 minutos en el archivo interactivo: **`GUIA_ENTREGA.md`** en la raíz de este proyecto.

¡Mucho éxito con tu sustentación final! 🚀
