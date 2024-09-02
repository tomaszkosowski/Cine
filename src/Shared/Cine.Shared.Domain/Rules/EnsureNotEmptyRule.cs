namespace Cine.Shared.Domain.Rules
{
    public sealed class EnsureNotEmptyRule(string Value, string PropertyName) : IBusinessRule
    {
        public string Message => $"'{PropertyName}' cannot be empty.";

        public bool IsBroken() => string.IsNullOrWhiteSpace(Value);
    }
}
