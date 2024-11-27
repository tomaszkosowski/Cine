namespace Cine.Shared.Domain.Rules
{
    public sealed class EnsureNotNegativeRule(TimeSpan Value, string PropertyName) : IBusinessRule
    {
        public string Message => $"'{PropertyName}' cannot be negative.";

        public bool IsBroken() => Value < TimeSpan.Zero;
    }
}
