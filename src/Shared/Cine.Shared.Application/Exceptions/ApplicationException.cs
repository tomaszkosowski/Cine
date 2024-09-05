namespace Cine.Shared.Application.Exceptions
{
    public class ApplicationException(string Message, Exception InnerException) : Exception(Message, InnerException);
}
