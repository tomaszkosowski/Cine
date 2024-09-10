using OneOf.Types;

namespace Cine.Shared.Application.Optional
{
    public static class OneOfFactory
    {
        public static Error<ApplicationException> CreateApplicationError(Exception exception)
            => new(new ApplicationException(exception.Message, exception));
    }
}
