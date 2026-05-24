# Instalar la app en el emulador Android

La app móvil es **GestionITM.AppMovil**, y en el emulador debe verse como **Ecosistema ITM**. No es Chrome, no es Swagger y no viene instalada por defecto: Visual Studio la despliega cuando ejecutas **F5**.

## Qué corre en cada lugar

| Lugar | Qué corre |
|---|---|
| PC | API + Swagger |
| Emulador Android | App MAUI Ecosistema ITM |
| Chrome del emulador | Solo navegador, no la app |

## Puertos según el modo de ejecución

| Modo | API | App MAUI |
|---|---|---|
| `dotnet run` | `http://localhost:5016` | `http://10.0.2.2:5016` |
| Docker | `http://localhost:8080` | `http://10.0.2.2:8080` |

## Flujo recomendado para probar la app

1. Abre una terminal en la raíz del repo.
2. Arranca la API con `JWT_KEY`.
3. Abre `EcosistemaITM.slnx` en Visual Studio.
4. Pon como proyecto de inicio **GestionITM.AppMovil**.
5. Selecciona el emulador Android, por ejemplo **Pixel 7**.
6. Ejecuta **F5**.
7. La app debe abrir directamente la pantalla de login.

## Arrancar la API en modo local

```powershell
$env:JWT_KEY='ClaveJwtMinimo32CaracteresParaHMAC256!!'
dotnet run --project src\GestionITM.API
```

Swagger debe abrirse en el PC en `http://localhost:5016/swagger`.

## Si usas Docker

Si la API corre con Docker, Swagger debe abrirse en `http://localhost:8080/swagger` y la app móvil debe apuntar a `10.0.2.2:8080`.

## Si aparece "Access denied" en `obj` o `bin`

Ese error es común cuando el proyecto está dentro de OneDrive. Haz esto en este orden:

1. Cierra Visual Studio y el emulador.
2. Pausa OneDrive durante 2 horas.
3. En PowerShell, desde la raíz del repo:

```powershell
Remove-Item -Recurse -Force "src\GestionITM.AppMovil\obj" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "src\GestionITM.AppMovil\bin" -ErrorAction SilentlyContinue
dotnet clean src\GestionITM.AppMovil\GestionITM.AppMovil.csproj
```

4. Vuelve a ejecutar **F5**.

## Cómo reconocer que sí instaló bien

- La app se abre sola al depurar.
- La pantalla inicial muestra el título **Ecosistema ITM**.
- Deben verse los campos de correo y contraseña.
- Si abres el cajón de apps después, puedes encontrar **Ecosistema ITM**.

## Credenciales de prueba

- Correo: `estudiante.demo@correo.itm.edu.co`
- Contraseña: `ItmDemo2026!`

## No confundir

| Esto no es la app | Esto sí es la app |
|---|---|
| Chrome con `10.0.2.2/swagger` | Pantalla de login de **Ecosistema ITM** |
| Gmail, YouTube, Chrome | **GestionITM.AppMovil** ejecutada con F5 |

## Si algo no conecta

Revisa `src\GestionITM.AppMovil\MauiProgram.cs`. El método `GetApiBaseUrl()` debe usar el puerto correcto según el modo: `5016` para `dotnet run` o `8080` para Docker.
