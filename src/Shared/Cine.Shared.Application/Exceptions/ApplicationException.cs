namespace Cine.Shared.Application.Exceptions;

public class ApplicationException(string message, Exception? innerException = null) : Exception(message, innerException);