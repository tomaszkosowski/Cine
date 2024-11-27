namespace Cine.Shared.Domain.Rules
{
    public sealed class EnsureNotPastRule(DateTime Value, string PropertyName) : IBusinessRule
    {
        public string Message => $"'{PropertyName}' cannot be in the past.";

        public bool IsBroken() => Utc.Now > Value;
    }
}
