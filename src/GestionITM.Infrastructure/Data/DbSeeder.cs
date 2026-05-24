using GestionITM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionITM.Infrastructure.Data;

public static class DbSeeder
{
    public const string DemoEmail = "estudiante.demo@correo.itm.edu.co";
    public const string DemoPassword = "ItmDemo2026!";

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedProfesoresAsync(context);
        await SeedCursosAsync(context);
        await SeedEstudiantesAsync(context);
        await context.SaveChangesAsync();
        await SeedMatriculasAsync(context);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProfesoresAsync(ApplicationDbContext context)
    {
        var profesores = new[]
        {
            ("Ana", "Torres", "Sistemas", "ana.torres@correo.itm.edu.co"),
            ("Luis", "Mendoza", "Bases de Datos", "luis.mendoza@correo.itm.edu.co"),
            ("Claudia", "Ríos", "Redes", "claudia.rios@correo.itm.edu.co"),
            ("Jorge", "Pineda", "Inteligencia Artificial", "jorge.pineda@correo.itm.edu.co"),
            ("Sofía", "Vargas", "Programación", "sofia.vargas@correo.itm.edu.co"),
            ("Diego", "Castro", "Ciberseguridad", "diego.castro@correo.itm.edu.co"),
            ("Carolina", "Hernández", "Matemáticas", "carolina.hernandez@correo.itm.edu.co"),
            ("Felipe", "Ramírez", "Arquitectura de Software", "felipe.ramirez@correo.itm.edu.co"),
            ("Paola", "Gil", "Desarrollo Web", "paola.gil@correo.itm.edu.co"),
            ("Nicolás", "Ortega", "Bases de Datos", "nicolas.ortega@correo.itm.edu.co")
        };

        foreach (var (nombre, apellido, especialidad, email) in profesores)
        {
            if (await context.Profesores.AnyAsync(p => p.Email == email))
                continue;

            context.Profesores.Add(new Profesor
            {
                Nombre = $"{nombre} {apellido}",
                Especialidad = especialidad,
                Email = email,
                FechaContratacion = DateTime.UtcNow.AddYears(-2)
            });
        }
    }

    private static async Task SeedCursosAsync(ApplicationDbContext context)
    {
        var cursos = new[]
        {
            ("MAT101", "Matemáticas Básicas", 3, 30, 25),
            ("PROG201", "Programación Avanzada", 4, 20, 5),
            ("RED301", "Redes de Computadores", 3, 15, 0),
            ("BD210", "Bases de Datos Relacionales", 3, 25, 18),
            ("ALG220", "Algoritmos y Estructuras de Datos", 4, 28, 12),
            ("EST310", "Estadística Aplicada", 3, 22, 20),
            ("IA350", "Introducción a Inteligencia Artificial", 4, 18, 7),
            ("CALC102", "Cálculo Diferencial", 4, 35, 30),
            ("ELEC210", "Electrónica Digital", 3, 20, 15),
            ("CYB303", "Fundamentos de Ciberseguridad", 3, 16, 4),
            ("WEB240", "Desarrollo Web con .NET", 4, 24, 10),
            ("MOV330", "Aplicaciones Móviles con MAUI", 4, 20, 8)
        };

        foreach (var (codigo, nombre, creditos, totales, disponibles) in cursos)
        {
            if (await context.Cursos.AnyAsync(c => c.Codigo == codigo))
                continue;

            context.Cursos.Add(new Curso
            {
                Codigo = codigo,
                Nombre = nombre,
                Creditos = creditos,
                CuposTotales = totales,
                CuposDisponibles = disponibles
            });
        }
    }

    private static async Task SeedEstudiantesAsync(ApplicationDbContext context)
    {
        var estudiantes = new[]
        {
            ("Estudiante Demo ITM", DemoEmail, "3001110001"),
            ("María García", "maria.garcia@correo.itm.edu.co", "3002220002"),
            ("Carlos López", "carlos.lopez@correo.itm.edu.co", "3003330003"),
            ("Laura Jiménez", "laura.jimenez@correo.itm.edu.co", "3004440004"),
            ("Andrés Muñoz", "andres.munoz@correo.itm.edu.co", "3005550005")
        };

        foreach (var (nombre, correo, telefono) in estudiantes)
        {
            if (await context.Estudiantes.AnyAsync(e => e.Correo == correo))
                continue;

            context.Estudiantes.Add(new Estudiante
            {
                Nombre = nombre,
                Correo = correo,
                Telefono = telefono,
                FechaInscripcion = DateTime.UtcNow.AddMonths(-3),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(DemoPassword)
            });
        }
    }

    private static async Task SeedMatriculasAsync(ApplicationDbContext context)
    {
        var demo = await context.Estudiantes.FirstOrDefaultAsync(e => e.Correo == DemoEmail);
        var maria = await context.Estudiantes.FirstOrDefaultAsync(e => e.Correo == "maria.garcia@correo.itm.edu.co");
        if (demo == null)
            return;

        var matriculas = new List<(int EstudianteId, string CodigoCurso, string Periodo, string Estado)>
        {
            (demo.Id, "MAT101", "2026-1", "Activa"),
            (demo.Id, "PROG201", "2026-1", "Activa")
        };

        if (maria != null)
        {
            matriculas.Add((maria.Id, "BD210", "2026-1", "Activa"));
            matriculas.Add((maria.Id, "WEB240", "2026-1", "Activa"));
            matriculas.Add((maria.Id, "CALC102", "2025-2", "Finalizada"));
        }

        foreach (var (estudianteId, codigoCurso, periodo, estado) in matriculas)
        {
            var curso = await context.Cursos.FirstOrDefaultAsync(c => c.Codigo == codigoCurso);
            if (curso == null)
                continue;

            var existe = await context.Matriculas.AnyAsync(m =>
                m.EstudianteId == estudianteId &&
                m.CursoId == curso.Id &&
                m.Periodo == periodo);

            if (existe)
                continue;

            context.Matriculas.Add(new Matricula
            {
                EstudianteId = estudianteId,
                CursoId = curso.Id,
                Periodo = periodo,
                Estado = estado
            });
        }
    }
}
