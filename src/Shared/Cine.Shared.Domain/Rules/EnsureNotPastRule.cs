namespace Cine.Shared.Domain.Rules
{
    public sealed class EnsureNotPastRule(DateTime value, string propertyName) : IBusinessRule
    {
        public string Message => $"'{propertyName}' cannot be in the past.";

        public bool IsBroken() => Utc.Now >= value;
    }
}
