using AutoMapper;
using GestionITM.API.Mappings;
using GestionITM.Domain.Interfaces;
using GestionITM.Infrastructure;
using GestionITM.API.Middleware;
using GestionITM.Infrastructure.Data;
using GestionITM.Infrastructure.Repositories;
using GestionITM.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;// Para la configuración de swagger 
using System.Text; // Para Encoding.UTF8.GetBytes
using System.Reflection; // Necesaqrio para Assembly.GetExecutingAssembly() en SwaggerGen
using System.IO; // Necesario para Path.Combine en SwaggerGen
using Serilog; // Para la configuración de logging con Serilog

var builder = WebApplication.CreateBuilder(args);

// 1. Logger temporal (bootstrap logger) para capturar logs durante el arranque de la aplicación, antes de que el host esté completamente configurado.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Arrancando el servidor GestionITM API...");

    // 2. Le decimos a ASPNET que use Serilog y lea toda la configuración del JSON (appsettings.json) para configurar Serilog, incluyendo los sinks, niveles de log, etc.
    builder.Host.UseSerilog((context, services, configuration) => configuration
                     .ReadFrom.Configuration(context.Configuration)
                     .ReadFrom.Services(services)
                     .Enrich.FromLogContext());
builder.Logging.AddFilter("Microsoft.AspNetCore.DataProtection", LogLevel.Error);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestionITM API",
        Version = "v1"
    });

    //INSTRUCCIÓN NUEVA
    // Localice el archivo xml generado en la carpeta de binario (bin) después de compilar el proyecto. El nombre del archivo suele ser el mismo que el nombre del proyecto, seguido de .xml (por ejemplo, GestionITM.API.xml).
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Pegue el token obtenido en POST /api/Auth/login. Ejemplo: eyJhbGciOiJIUzI1NiIs...",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
}
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configure ConnectionStrings__DefaultConnection (variable de entorno o User Secrets).");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? Environment.GetEnvironmentVariable("JWT_KEY");
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("Configure Jwt:Key o la variable de entorno JWT_KEY.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });


// 2. Registrar el Repositorio (Inyección de Dependencias)
// AddScoped significa: "Crea una instancia por cada petición HTTP"
builder.Services.AddScoped<IEstudianteRepository, EstudianteRepository>();
builder.Services.AddScoped<IEstudianteService, EstudianteService>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();

builder.Services.AddScoped<IProfesorRepository, ProfesorRepository>();
builder.Services.AddScoped<IProfesorService, ProfesorService>();

builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
builder.Services.AddScoped<IMatriculaService, MatriculaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Nivel Dios: Aplicar migraciones pendientes automáticamente al arrancar
await using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        await DbSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al aplicar la migración de la base de datos.");
    }
}

// 3. Mildleware mágico Nivel 5:  Registra los códigos HTTP 200, 400, 404, 500 aotomaticamente 

app.UseSerilogRequestLogging(); // Middleware de Serilog para registrar las solicitudes HTTP y sus respuestas, incluyendo los códigos de estado. Esto es útil para monitorear el tráfico y detectar errores.

// Configure the HTTP request pipeline. ESCUDO DE EXCEPCIONES GLOBAL
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    // Bloque de activación de autenticación y autorización
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fallo catastrófico al iniciar la API.");
}
finally
{
    Log.Information("Apagando la API de forma segura.");
    Log.CloseAndFlush(); // Asegura que el archivo de texto no quede bloqueado
}