namespace Cine.Shared.Domain.Rules;

public sealed class EnsureNotEmptyRule(string value, string propertyName) : IBusinessRule
{
    public string Message => $"'{propertyName}' cannot be empty.";

    public bool IsBroken() => string.IsNullOrWhiteSpace(value);
}