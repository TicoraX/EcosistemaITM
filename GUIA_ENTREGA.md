# Guía de Entrega: Taller Final Integrador "El Ecosistema ITM Nivel 5" 🚀

¡Felicidades por llegar hasta aquí! El código ya cumple con todos los requisitos técnicos para una nota de **5.0**. Ahora es fundamental que la presentación y entrega sean igual de profesionales. 

Sigue esta guía paso a paso para armar tu documento PDF y grabar tu video demostrativo.

---

## 📄 Parte 1: Preparar el Documento PDF

El profesor especificó claramente: **NADA DE ARCHIVOS .ZIP**. Debes subir a la plataforma únicamente un documento PDF. Te sugiero esta estructura para el documento:

1. **Portada:** Tu nombre, nombre de tu compañero (si aplica), asignatura y título del proyecto.
2. **Enlace al Repositorio de GitHub:** 
   * Asegúrate de que el repositorio sea público o de invitar al profesor si es privado.
   * *Nota:* Asegúrate de haber hecho un `git push` con todos los cambios que hicimos (Fase A, GitHub Actions, Docker).
3. **Evidencia de CI/CD (Puntos Extra ✅):**
   * Ve a la pestaña **"Actions"** en tu repositorio de GitHub.
   * Toma un pantallazo (screenshot) donde se vea claramente el chulito verde de éxito del workflow `CI Pipeline` que creamos.
   * Pega esa imagen en el PDF.
4. **Colección de Postman:**
   * Entra a Postman, haz clic derecho en tu colección de pruebas y selecciona "Export" o "Share -> Via API/Link".
   * Pega el enlace público o indica que adjuntas el archivo `.json` de la colección junto con la entrega (si la plataforma permite múltiples archivos, aunque el PDF con un enlace público es mejor).
5. **Enlace del Video Demostrativo:**
   * Pega el enlace de tu video subido a YouTube (configurado como "Oculto" / "Unlisted").

---

## 🎥 Parte 2: El Video Demostrativo (Máximo 3 Minutos)

El video debe ir directo al grano. Tienes que demostrar los 4 puntos exactos que pidió el profesor.

**💡 Guion Sugerido para el Video:**

* **[0:00 - 0:10] Presentación:** 
  > *"Hola profesor, presento mi Taller Final Integrador nivel 5. A continuación la demostración técnica."*

* **[0:10 - 0:40] Requisito 1: Infraestructura y Docker:** 
  * Abre una terminal limpia y escribe el comando `docker-compose up -d`.
  * Muestra cómo en pocos segundos dicen `Started` los contenedores `itm-database` e `itm-api`.
  * > *"Como puede ver, la arquitectura está dockerizada de forma multietapa. Levantamos la Base de Datos y la API simultáneamente con docker-compose."*

* **[0:40 - 1:10] Requisito 2: App Móvil y Autenticación:** 
  * Muestra el emulador de Android (o tu celular) con la App MAUI abierta en la pantalla de Login.
  * Ingresa los datos y presiona entrar. 
  * > *"Aquí tenemos el frontend en MAUI. Realizamos el Login y el token JWT se guarda automáticamente usando SecureStorage. Además, tenemos un Interceptor configurado para todas las siguientes peticiones."*

* **[1:10 - 1:40] Requisito 3: Catálogo y Paginación:** 
  * Muestra la pantalla del catálogo de cursos.
  * Haz scroll lentamente hacia abajo para que se vea cómo cargan nuevos cursos automáticamente.
  * > *"Implementamos el catálogo de cursos con CollectionView. Aplicamos el patrón PagedResult en el backend y el Scroll Infinito en el frontend para no saturar la base de datos."*

* **[1:40 - 2:30] Requisito 4: Reglas de Negocio y Resiliencia (UX):** 
  * Selecciona un curso que **no tenga cupos** e intenta matricularte.
  * Muestra cómo sale la alerta emergente (pop-up) amigable en la pantalla.
  * > *"Finalmente, probamos la resiliencia y la separación de capas. Al intentar tomar un curso sin cupos, el servicio del backend lanza una excepción controlada (Regla de negocio). La App no colapsa, sino que captura el error 400 y lo muestra en este DisplayAlert amigable para el usuario."*

* **[2:30 - 2:40] Despedida:** 
  > *"Con esto cumplo todos los requerimientos técnicos y de arquitectura limpia. Muchas gracias."*

---

## 🛠️ Últimos preparativos antes de grabar

1. **Verifica la Base de Datos:** Asegúrate de tener al menos un par de cursos creados en la base de datos (puedes crearlos con Swagger `http://localhost:8080/swagger`), y asegúrate de que **al menos uno tenga 0 `CuposDisponibles`** para poder probar el error en el video.
2. **Ensaya:** Haz un recorrido de prueba sin grabar para asegurarte de que todo carga rápido y que el emulador del celular responde bien. 
3. **Limpia la terminal:** Antes de grabar, ejecuta `docker-compose down` para apagar los contenedores, así cuando grabes el `docker-compose up` se verá cómo se levantan desde cero.

¡Mucho éxito con tu presentación y a por ese 5.0! 🎉
