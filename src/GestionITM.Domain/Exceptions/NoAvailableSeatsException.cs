namespace GestionITM.Domain.Exceptions;

public class NoAvailableSeatsException : Exception
{
    public NoAvailableSeatsException()
        : base("El curso ya no tiene cupos disponibles.")
    {
    }

    public NoAvailableSeatsException(string message) : base(message) { }
}
