# Guía de Entrega: Taller Final Integrador "El Ecosistema ITM Nivel 5"

Este documento contiene las pautas sugeridas para estructurar la entrega del proyecto final y preparar la sustentación técnica ante el docente.

---

## Parte 1: Preparación del Documento PDF

Según las indicaciones del taller, no deben subirse archivos comprimidos (.zip). Toda la sustentación escrita debe presentarse en un único documento PDF con la siguiente estructura sugerida:

1. **Portada:** Nombre del estudiante (o de la pareja), asignatura, fecha y título del proyecto.
2. **Enlace al Repositorio de GitHub:** 
   * Asegúrese de que el repositorio sea público para facilitar la revisión del código.
   * Verifique haber realizado la sincronización (`git push`) de todos los archivos y configuraciones correspondientes al backend, la aplicación móvil y DevOps.
3. **Evidencia de Integración Continua (CI/CD):**
   * En la pestaña "Actions" del repositorio de GitHub, tome una captura de pantalla del pipeline completado con éxito.
   * Adjunte la imagen en esta sección como constancia del funcionamiento automático de las pruebas.
4. **Colección de Postman:**
   * Exporte la colección de Postman e incluya en el PDF el enlace de acceso público o el texto JSON correspondiente para que el docente pueda replicar las pruebas.
5. **Enlace del Video Demostrativo:**
   * Inserte el enlace directo al video explicativo subido a una plataforma como YouTube (se recomienda configurarlo en modo "Oculto" o "No listado").

---

## Parte 2: Video Demostrativo (Máximo 3 Minutos)

El video debe ser conciso y enfocado en validar los requisitos técnicos fundamentales solicitados. A continuación, se presenta una propuesta de guion técnico:

* **[0:00 - 0:10] Introducción:** 
  > "Presentación del proyecto correspondiente al Módulo de Matrículas del Ecosistema ITM Nivel 5. A continuación, inicia la demostración técnica."

* **[0:10 - 0:40] Requisito 1: Infraestructura y Docker:** 
  * Con una terminal limpia, ejecute el comando `docker-compose up -d`.
  * Muestre el estado activo de los contenedores de la API (`itm-api`) y de la base de datos (`itm-database`).
  * > "La arquitectura se encuentra completamente dockerizada mediante un proceso de construcción multietapa. Levantamos de forma simultánea la base de datos SQL Server y el servicio Web API en un entorno virtualizado y conectado."

* **[0:40 - 1:10] Requisito 2: Aplicación Móvil y Autenticación:** 
  * Muestre el emulador con la pantalla de inicio de sesión de la aplicación .NET MAUI.
  * Ingrese las credenciales de prueba y acceda al sistema.
  * > "El frontend móvil está desarrollado en .NET MAUI. Tras un inicio de sesión exitoso, el token JWT devuelto por el servidor se almacena localmente de forma segura en el dispositivo mediante SecureStorage. Adicionalmente, se cuenta con un interceptor HTTP que adjunta automáticamente este token en las cabeceras de todas las solicitudes subsiguientes."

* **[1:10 - 1:40] Requisito 3: Catálogo y Paginación:** 
  * Navegue por el catálogo de cursos realizando un desplazamiento (scroll) hacia abajo para mostrar la carga dinámica.
  * > "El catálogo de cursos hace uso de CollectionView y el evento RemainingItemsThresholdReached. Se consume un endpoint paginado en el backend bajo el patrón PagedResult, lo que permite implementar scroll infinito en la aplicación para optimizar el rendimiento y el tráfico de red."

* **[1:40 - 2:30] Requisito 4: Reglas de Negocio y Resiliencia en UX:** 
  * Seleccione un curso con cupo cero e intente realizar la matrícula para forzar el error controlado.
  * Muestre en pantalla la alerta emergente que explica la situación.
  * > "Para garantizar la robustez, el servicio en la capa de infraestructura valida la disponibilidad de cupos antes de procesar una matrícula. Si el curso no dispone de capacidad, se lanza una excepción de negocio que la API expone como un error 400. La aplicación móvil intercepta este error y presenta un cuadro de diálogo amigable al usuario en lugar de interrumpir la ejecución de la app."

* **[2:30 - 2:40] Conclusión:** 
  > "Con esta demostración se valida el cumplimiento de las reglas de negocio, la arquitectura por capas y los estándares solicitados para el integrador. Muchas gracias por su atención."

---

## Recomendaciones Técnicas Previas a la Grabación

1. **Preparación de la Base de Datos:** Verifique con anticipación que haya registros de cursos creados en la base de datos, y asegúrese de que al menos uno de ellos tenga la columna de cupos disponibles en 0 para poder recrear la validación del error durante el video.
2. **Prueba de Red en Emuladores:** Si realiza la prueba desde un emulador de Android o un dispositivo físico, asegúrese de que la dirección IP configurada en `MauiProgram.cs` apunte a la IP de red local del equipo donde corre la API, en lugar de `localhost`.
3. **Limpieza del Entorno:** Ejecute `docker-compose down` antes de iniciar la grabación para poder mostrar en tiempo real cómo arranca toda la infraestructura desde cero.
