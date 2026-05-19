using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Dtos;
using GestionITM.Infrastructure.Services;
using AutoMapper;
using GestionITM.Domain.Entities;

namespace GestionITM.Tests
{
    public class ProfesorServiceTests
    {
        // Prueba 1: El camino triste (validar que falle cuando debe fallar)
        [Fact] // Este atributo le dice a Visual Studio que este método es una prueba unitaria
        public async Task RegistrarProfesor_ConEspecialidadVacia_DebeRetornarFalse()
        {
            // 1. Arrange (Preparar el escenario)

            // Creamos los dobles de acción (Mocks) usando nuestras interfaces
            // Esta es la razón por la que creamos IProfesorRepository: para poder simular su comportamiento sin depender de la base de datos real
            var mockRepository = new Mock<IProfesorRepository>();
            var mockMapper = new Mock<IMapper>();

            // Configuramos el mapper para devolver un Profesor cualquiera cuando reciba un ProfesorCreateDto
            mockMapper
                .Setup(m => m.Map<Profesor>(It.IsAny<ProfesorCreateDto>()))
                .Returns(new Profesor());

            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ProfesorService>>();

            // Instanciamos el servicio REAL, pero le inyectamos los mocks en lugar de las implementaciones reales
            var profesorService = new ProfesorService(mockRepository.Object, mockMapper.Object, mockLogger.Object);

            // Preparamos unos datos errados a propósito para probar la validación

            var dtomalo = new ProfesorCreateDto
            {
                Nombre = "Juan Perez",
                Email = "juan@itm.edu.co",
                Especialidad = "" // Esta es la especialidad vacía que queremos probar

                };

            // 2. Act & Assert (Ejecutar la acción y exgir que lance excepción)

            // Exigimos (Assert) que al ejecutar (Act) el método, el sistema DEBE lanzar una excepción con mensaje "Especialidad vacía"
            var excepcion = await Assert.ThrowsAsync<Exception>(() => profesorService.RegistrarProfesorAsync(dtomalo));
            Assert.Equal("Especialidad vacía", excepcion.Message);
        }

        // Prueba 2 : El camino feliz (Validar que funcione cuando debe funcionar)
        [Fact]
        public async Task RegistrarProfesor_DatosCorrectos_DebeLllamarAlRepositorio()
        {
            // 1. Arrange
            var mockRepository = new Mock<IProfesorRepository>();
            var mockMapper = new Mock<IMapper>();

            mockMapper
                .Setup(m => m.Map<Profesor>(It.IsAny<ProfesorCreateDto>()))
                .Returns(new Profesor());

            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ProfesorService>>();
            var profesorService = new ProfesorService(mockRepository.Object, mockMapper.Object, mockLogger.Object);
            var dtobien = new ProfesorCreateDto
            {
                Nombre = "Ana",
                Email = "ana@itm.edu.co",
                Especialidad = "Arquitectura"
            };

            // 2. Act

            await profesorService.RegistrarProfesorAsync(dtobien);

            // 3. Assert
            // Verificamos que el método AgregarAsync del repositorio se haya llamado exactamente una vez
            // It.IsAny<Profesor>() signfica que no nos importa el objeto Profesor específico que se le pasó, solo queremos asegurarnos de que se llamó con algún objeto de tipo Profesor
            mockRepository.Verify(x => x.AgregarAsync(It.IsAny<Profesor>()), Times.Once);
        }
    }
}
