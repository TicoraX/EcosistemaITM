# Guía Personal de Entrega y Sustentación

Este documento está pensado como checklist personal para cerrar el proyecto, preparar la sustentación y organizar lo que sí debe mostrarse al docente.

## 1. Estado real del proyecto

Con lo que se revisó en el repositorio, el proyecto ya cubre el núcleo funcional esperado para la entrega.

### Lo que ya está resuelto

- Backend en capas con `Domain`, `Infrastructure` y `API`.
- Matrícula protegida con JWT y rol `Estudiante`.
- Regla de cupos controlada en el servicio de matrícula.
- Regla de matrícula duplicada controlada con conflicto claro.
- Paginación de cursos con `Skip`, `Take` y `PagedResult<T>`.
- App MAUI con login, `SecureStorage`, `DelegatingHandler`, scroll infinito y mensajes amigables.
- Serilog escribiendo logs en `Logs/api-log-.txt`.
- Dockerfile multietapa y `docker-compose.yml` funcionales.
- Colección de Postman preparada para pruebas.
- Documentación de cierre y material de apoyo ya alineado con sustentación.

### Lo que no hace falta tocar salvo que el profesor lo exija

- Reescrituras grandes de arquitectura.
- Refactors cosméticos en capas que ya funcionan.
- Cambios en la UI móvil que no aporten a la demo.
- Cambios de comportamiento que puedan romper la sustentación.

## 2. Qué debe ir en la entrega

### En el PDF final

Incluye, como mínimo, estas partes:

1. Portada.
2. Enlace al repositorio.
3. Evidencia de GitHub Actions en verde.
4. Enlace o captura de la colección de Postman.
5. Resumen técnico de arquitectura limpia.
6. Capturas o descripción del flujo móvil.
7. Guion de sustentación.
8. Conclusión final.

### Lo que no debes exponer públicamente

- Contraseñas locales.
- Secrets de GitHub.
- Tokens de Docker Hub.
- Rutas privadas de instalación con datos sensibles.
- Archivos internos que solo sirven para ti durante el cierre.

## 3. Checklist de verificación técnica

Antes de entregar, valida lo siguiente:

- La API arranca con `dotnet run --project src\GestionITM.API`.
- La app MAUI abre en el emulador Android.
- El login funciona con las credenciales demo.
- El token JWT se guarda en `SecureStorage`.
- El header `Authorization` se adjunta automáticamente.
- El scroll infinito carga más cursos.
- La matrícula con cupo cero muestra error amigable.
- La matrícula duplicada devuelve conflicto controlado.
- Los logs se escriben en `Logs`.
- El `docker-compose up -d --build` levanta API y base de datos.
- GitHub Actions aparece en verde.
- Postman tiene login, cursos y matrícula listos.

## 4. Ruta recomendada para la sustentación

La sustentación debe verse ordenada, breve y demostrativa.

### Orden recomendado

1. Mostrar que el backend está listo.
2. Levantar infraestructura con Docker o demostrar la API local.
3. Abrir la app móvil.
4. Hacer login.
5. Mostrar el catálogo paginado.
6. Forzar una matrícula válida.
7. Forzar una matrícula con cupo cero.
8. Forzar una matrícula duplicada.
9. Mostrar evidencia de Actions y Postman.
10. Cerrar con conclusión técnica.

## 5. Guion corto de sustentación

### Apertura

"Este proyecto implementa el módulo de matrículas del Ecosistema ITM con arquitectura limpia, seguridad JWT, paginación de cursos, app móvil MAUI y despliegue con Docker."

### Infraestructura

"La solución puede ejecutarse con Docker Compose o de forma nativa. La API usa SQL Server y registra eventos con Serilog."

### Autenticación

"El login devuelve un JWT que se guarda de forma segura en el dispositivo. A partir de ese token se protege la matrícula con el rol Estudiante."

### Catálogo

"Los cursos se consultan de forma paginada, lo que permite scroll infinito desde la app móvil."

### Matrícula

"La matrícula valida cupos y evita duplicados. Si no hay cupo, la API responde con error controlado; si ya existe matrícula, devuelve conflicto controlado."

### Cierre

"La solución cumple la arquitectura solicitada, las reglas de negocio y la experiencia móvil esperada para la entrega."

## 6. Qué probar en Postman

### Flujo mínimo

1. Login.
2. Cursos paginados.
3. Matrícula con cupo.
4. Matrícula sin cupo.
5. Matrícula duplicada.

### Qué revisar en cada request

- Que la URL use la base correcta.
- Que el token quede almacenado.
- Que el body esté con el formato actual.
- Que los códigos de respuesta sean los esperados.

## 7. Cómo usar la app móvil en la demo

### Antes de abrir la app

- Verifica que la API esté arriba.
- Verifica que el puerto coincida con el modo de ejecución.
- Verifica que el emulador Android esté seleccionado.

### Durante la demo

- Entra con la cuenta demo.
- Verifica que el login no muestre errores.
- Baja en el catálogo para mostrar carga dinámica.
- Intenta matricular un curso con cupo disponible.
- Intenta matricular uno sin cupo.
- Si aplica, intenta el duplicado.

## 8. Qué archivos sí conviene enseñar

### Backend

- `src/GestionITM.API/Program.cs`
- `src/GestionITM.API/Controllers/MatriculaController.cs`
- `src/GestionITM.API/Middleware/ExceptionMiddleware.cs`
- `src/GestionITM.Infrastructure/Services/MatriculaService.cs`
- `src/GestionITM.Infrastructure/Repositories/CursoRepository.cs`
- `src/GestionITM.Infrastructure/Repositories/MatriculaRepository.cs`
- `src/GestionITM.Domain/Dtos/MatriculaCreateDto.cs`

### Móvil

- `src/GestionITM.AppMovil/Services/ApiService.cs`
- `src/GestionITM.AppMovil/Services/AuthHandler.cs`
- `src/GestionITM.AppMovil/ViewModels/LoginViewModel.cs`
- `src/GestionITM.AppMovil/ViewModels/CursosViewModel.cs`

### Documentación y evidencia

- `README.md`
- `postman/EcosistemaITM.postman_collection.json`
- Captura de GitHub Actions

## 9. Qué no conviene mencionar como foco principal

- Refactors internos que no cambian el comportamiento.
- Detalles de implementación que el docente no te vaya a preguntar.
- Dependencias internas de prueba o cambios históricos que ya no importan.
- Guías privadas de instalación o contraseñas.

## 10. Problemas comunes y cómo responder

### La API no abre

Revisa que `JWT_KEY` esté definido y que la cadena de conexión sea válida.

### La app no inicia sesión

Revisa que la API esté en el puerto correcto y que el emulador apunte a `10.0.2.2`.

### La matrícula falla

Revisa que el curso tenga cupos y que el token corresponda al rol `Estudiante`.

### Postman no guarda el token

Revisa que el request de login se ejecute antes y que la variable `jwt_token` exista.

### GitHub Actions falla

Revisa el log del workflow y confirma que no haya rompimiento de compilación o de prueba.

## 11. Checklist final antes de entregar

- [ ] Repositorio público actualizado.
- [ ] Rama principal sin cambios pendientes.
- [ ] GitHub Actions en verde.
- [ ] Postman listo y probado.
- [ ] PDF final armado.
- [ ] Sustentación ensayada.
- [ ] Capturas verificadas.
- [ ] Demo móvil probada de punta a punta.
- [ ] Mensajes de error claros y controlados.
- [ ] No hay documentos privados expuestos en el repo.

## 12. Conclusión práctica

Si el profesor evalúa la entrega por funcionalidad, arquitectura y demo, el proyecto ya está en un punto fuerte. Lo que queda es cerrar bien la presentación, cuidar qué material se expone públicamente y mostrar el flujo completo sin improvisar.

## 13. Comandos rápidos (PowerShell / Windows)

Guarda esta lista en un lugar accesible. Son los comandos que más vas a necesitar para probar, depurar y preparar la entrega.

- Arrancar API local (usa `JWT_KEY` en la misma sesión):

```powershell
$env:JWT_KEY='ClaveJwtMinimo32CaracteresParaHMAC256!!'
dotnet run --project src\GestionITM.API
```

- Aplicar migraciones (desde la raíz del repo):

```powershell
dotnet ef database update --project src\GestionITM.Infrastructure --startup-project src\GestionITM.API
```

- Levantar infraestructura con Docker Compose (build + run):

```bash
docker-compose up -d --build
```

- Detener y limpiar contenedores:

```bash
docker-compose down
```

- Construir imagen Docker manualmente (ejemplo para la API):

```bash
docker build -f src/GestionITM.API/Dockerfile -t tuusuario/itm-api:latest .
```

- Ejecutar pruebas unitarias:

```powershell
dotnet test tests\GestionITM.Tests\GestionITM.Tests.csproj -c Release
```

- Comandos git útiles (rama de entrega y push):

```bash
git checkout -b release/entrega-publica
git add -A
git commit -m "chore: preparar entrega publica"
git push -u origin release/entrega-publica
```

- Hacer merge a `main` (si ya verificaste y quieres integrar):

```bash
git checkout main
git pull origin main
git merge --no-ff release/entrega-publica -m "Merge release/entrega-publica"
git push origin main
```

- Exportar/Importar colección Postman: abrir Postman → Import → elegir `postman/EcosistemaITM.postman_collection.json` y ejecutar el request de Login primero.

- Ver logs locales (Serilog files):

```powershell
Get-ChildItem -Path Logs -Filter "api-log-*.txt" | Select-Object -Last 3
Get-Content -Path Logs\api-log-2026-05-24.txt -Tail 200 -Wait
```

---

Guía corta: para la demo yo suelo ejecutar en orden: 1) `dotnet ef database update`, 2) `dotnet run --project src\GestionITM.API`, 3) abrir emulador y ejecutar la app con F5, 4) en Postman ejecutar Login y pruebas.
