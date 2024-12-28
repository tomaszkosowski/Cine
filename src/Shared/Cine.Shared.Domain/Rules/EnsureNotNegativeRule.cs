namespace Cine.Shared.Domain.Rules;

public sealed class EnsureNotNegativeRule(TimeSpan value, string propertyName) : IBusinessRule
{
    public string Message => $"'{propertyName}' cannot be negative.";

    public bool IsBroken() => value < TimeSpan.Zero;
}