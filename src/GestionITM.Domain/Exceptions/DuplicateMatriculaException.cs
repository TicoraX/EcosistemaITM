namespace GestionITM.Domain.Exceptions;

public class DuplicateMatriculaException : Exception
{
    public DuplicateMatriculaException(int estudianteId, int cursoId)
        : base($"Ya existe una matrícula para el estudiante {estudianteId} en el curso {cursoId}.")
    {
    }
}