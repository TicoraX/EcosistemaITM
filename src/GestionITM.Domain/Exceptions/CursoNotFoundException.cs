namespace GestionITM.Domain.Exceptions;

public class CursoNotFoundException : Exception
{
    public CursoNotFoundException(int cursoId)
        : base($"El curso con ID {cursoId} no existe.")
    {
    }
}
