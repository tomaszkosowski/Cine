namespace Cine.Shared.Domain.Rules
{
    public sealed class EnsureNotZeroRule(TimeSpan Value, string PropertyName) : IBusinessRule
    {
        public string Message => $"'{PropertyName}' cannot be zero.";

        public bool IsBroken() => Value == TimeSpan.Zero;
    }
}
