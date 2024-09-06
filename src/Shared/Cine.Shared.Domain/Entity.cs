using Cine.Shared.Domain.Events;
using Cine.Shared.Domain.Exceptions;
using Cine.Shared.Domain.Rules;

namespace Cine.Shared.Domain
{
    public abstract class Entity
    {
        #region Fields

        private List<IDomainEvent> _domainEvents = null!;

        #endregion

        #region Properties

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly()!;

        #endregion

        #region Public methods

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        #endregion

        #region Protected methods

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= [];
            _domainEvents.Add(domainEvent);
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }

        #endregion
    }
}
