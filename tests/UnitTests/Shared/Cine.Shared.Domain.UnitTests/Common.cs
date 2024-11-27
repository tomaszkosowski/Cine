using Cine.Shared.Domain.Events;
using Cine.Shared.Domain.Exceptions;
using Cine.Shared.Domain.Rules;
using FluentAssertions;
using Xunit;

namespace Cine.Shared.Domain.UnitTests
{
    public static class Common
    {
        public static TDomainEvent? GetDomainEvent<TDomainEvent>(this Entity entity) where TDomainEvent : IDomainEvent
            => entity.DomainEvents.OfType<TDomainEvent>().FirstOrDefault();

        public static void AssertBrokenRule<TRule>(this Func<object> action) where TRule : IBusinessRule
        {
            var exception = Assert.Throws<BusinessRuleValidationException>(action);
            exception.BrokenRule.Should().BeOfType<TRule>();
        }
    }
}
