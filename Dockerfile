# ==========================================
# ETAPA 1: BASE (El motor mínimo para correr)
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ==========================================
# ETAPA 2: BUILD (La Fábrica de Compilación)
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiamos TODOS los archivos de proyecto primero (Para aprovechar la caché de Docker)
COPY ["src/GestionITM.API/GestionITM.API.csproj", "src/GestionITM.API/"]
COPY ["src/GestionITM.Domain/GestionITM.Domain.csproj", "src/GestionITM.Domain/"]
COPY ["src/GestionITM.Infrastructure/GestionITM.Infrastructure.csproj", "src/GestionITM.Infrastructure/"]

# 2. Restauramos las dependencias (NuGets)
RUN dotnet restore "src/GestionITM.API/GestionITM.API.csproj"

# 3. Copiamos el resto del código fuente real
COPY . .
WORKDIR "/src/src/GestionITM.API"

# 4. Compilamos el proyecto en modo Release (Optimizado)
RUN dotnet build "GestionITM.API.csproj" -c Release -o /app/build

# ==========================================
# ETAPA 3: PUBLISH (Empacar el producto final)
# ==========================================
FROM build AS publish
RUN dotnet publish "GestionITM.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==========================================
# ETAPA 4: PRODUCCIÓN (El contenedor final)
# ==========================================
FROM base AS final
WORKDIR /app
# Solo copiamos el resultado de la etapa 'publish', dejando la "fábrica" atrás
COPY --from=publish /app/publish .

# Comando de arranque cuando el contenedor encienda
ENTRYPOINT ["dotnet", "GestionITM.API.dll"]
