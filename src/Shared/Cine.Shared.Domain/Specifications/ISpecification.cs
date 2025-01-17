namespace Cine.Shared.Domain.Specifications;

public interface ISpecification<in T>
{
    bool IsSatisfiedBy(T other);
}