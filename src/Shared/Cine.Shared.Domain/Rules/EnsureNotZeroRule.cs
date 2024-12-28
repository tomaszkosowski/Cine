namespace Cine.Shared.Domain.Rules;

public sealed class EnsureNotZeroRule(TimeSpan value, string propertyName) : IBusinessRule
{
    public string Message => $"'{propertyName}' cannot be zero.";

    public bool IsBroken() => value == TimeSpan.Zero;
}