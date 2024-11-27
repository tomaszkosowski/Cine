namespace Cine.Shared.Domain.Rules;

public sealed class EnsureNotEmptyCollectionRule<T>(List<T> Values, string PropertyName) : IBusinessRule
{
    public string Message => $"'{PropertyName}' cannot be empty collection.";

    public bool IsBroken() => Values.Count is 0;
}