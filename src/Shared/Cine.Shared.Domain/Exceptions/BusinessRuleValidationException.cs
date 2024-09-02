using Cine.Shared.Domain.Rules;

namespace Cine.Shared.Domain.Exceptions
{
    public sealed class BusinessRuleValidationException(IBusinessRule brokenRule) : Exception(brokenRule.Message)
    {
        #region Properties

        public IBusinessRule BrokenRule { get; } = brokenRule;

        public string Details { get; } = brokenRule.Message;

        #endregion

        #region Constructors

        #endregion

        #region Public methods

        public override string ToString()
            => $"{BrokenRule.GetType().FullName} = {BrokenRule.Message}";

        #endregion
    }
}
