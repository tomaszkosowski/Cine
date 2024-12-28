using Cine.Shared.Domain.Exceptions;
using Cine.Shared.Domain.Rules;

namespace Cine.Shared.Domain;

public abstract record ValueObject
{
    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }
}