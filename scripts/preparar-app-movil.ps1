# Limpia e intenta compilar/instalar la app MAUI en el emulador.
# Ejecutar desde la raíz del repo. Requiere Visual Studio 2022 con workload MAUI + Android.

$ErrorActionPreference = "Stop"
$project = Join-Path $PSScriptRoot "src\GestionITM.AppMovil\GestionITM.AppMovil.csproj"

Write-Host "Limpiando obj/bin (OneDrive suele bloquear estas carpetas)..." -ForegroundColor Yellow
Remove-Item -Recurse -Force (Join-Path $PSScriptRoot "src\GestionITM.AppMovil\obj") -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force (Join-Path $PSScriptRoot "src\GestionITM.AppMovil\bin") -ErrorAction SilentlyContinue

dotnet clean $project

Write-Host "Compilando Android..." -ForegroundColor Cyan
dotnet build $project -f net9.0-android -c Debug

Write-Host ""
Write-Host "Compilacion OK. Ahora en Visual Studio:" -ForegroundColor Green
Write-Host "  1. GestionITM.AppMovil = proyecto de inicio"
Write-Host "  2. Dispositivo = Android Emulator (Pixel 7)"
Write-Host "  3. F5"
Write-Host ""
Write-Host "La app se abrira sola (login Ecosistema ITM). No hace falta buscarla en Chrome." -ForegroundColor Green
