using AutoMapper;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionITM.Infrastructure.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IMapper _mapper;

        public MatriculaService(IMatriculaRepository matriculaRepository, ICursoRepository cursoRepository, IMapper mapper)
        {
            _matriculaRepository = matriculaRepository;
            _cursoRepository = cursoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MatriculaDto>> GetAllAsync()
        {
            var matriculas = await _matriculaRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MatriculaDto>>(matriculas);
        }

        public async Task<MatriculaDto?> GetByIdAsync(int id)
        {
            var matricula = await _matriculaRepository.GetByIdAsync(id);
            if (matricula == null) return null;
            
            return _mapper.Map<MatriculaDto>(matricula);
        }

        public async Task<MatriculaDto> CreateAsync(MatriculaCreateDto matriculaDto)
        {
            // Regla de Negocio (El Chef)
            var curso = await _cursoRepository.ObtenerPorIdAsync(matriculaDto.CursoId);
            if (curso == null)
            {
                throw new Exception("El curso especificado no existe.");
            }

            if (curso.CuposDisponibles <= 0)
            {
                throw new Exception("El curso ya no tiene cupos disponibles.");
            }

            // Disminuir cupos disponibles
            curso.CuposDisponibles--;
            await _cursoRepository.ActualizarAsync(curso);

            // Crear matrícula
            var matricula = _mapper.Map<Matricula>(matriculaDto);
            matricula.Estado = "Activa";

            var nuevaMatricula = await _matriculaRepository.AddAsync(matricula);
            return _mapper.Map<MatriculaDto>(nuevaMatricula);
        }
    }
}
