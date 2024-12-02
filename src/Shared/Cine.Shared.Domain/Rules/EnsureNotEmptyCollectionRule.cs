namespace Cine.Shared.Domain.Rules;

public sealed class EnsureNotEmptyCollectionRule<T>(List<T> values, string propertyName) : IBusinessRule
{
    public string Message => $"'{propertyName}' cannot be empty collection.";

    public bool IsBroken() => values.Count is 0;
}