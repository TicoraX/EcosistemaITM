using BCrypt.Net;
using GestionITM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionITM.Infrastructure.Data;

public static class DbSeeder
{
    public const string DemoEmail = "estudiante.demo@correo.itm.edu.co";
    public const string DemoPassword = "ItmDemo2026!";

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!await context.Estudiantes.AnyAsync(e => e.Correo == DemoEmail))
        {
            context.Estudiantes.Add(new Estudiante
            {
                Nombre = "Estudiante Demo ITM",
                Correo = DemoEmail,
                Telefono = "3000000000",
                FechaInscripcion = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(DemoPassword)
            });
        }

        if (!await context.Cursos.AnyAsync())
        {
            context.Cursos.AddRange(
                new Curso
                {
                    Codigo = "MAT101",
                    Nombre = "Matemáticas Básicas",
                    Creditos = 3,
                    CuposTotales = 30,
                    CuposDisponibles = 25
                },
                new Curso
                {
                    Codigo = "PROG201",
                    Nombre = "Programación Avanzada",
                    Creditos = 4,
                    CuposTotales = 20,
                    CuposDisponibles = 5
                },
                new Curso
                {
                    Codigo = "RED301",
                    Nombre = "Redes de Computadores",
                    Creditos = 3,
                    CuposTotales = 15,
                    CuposDisponibles = 0
                });
        }

        await context.SaveChangesAsync();
    }
}
