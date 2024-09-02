namespace Cine.Shared.Domain.Rules
{
    public interface IBusinessRule
    {
        bool IsBroken();

        uint Code { get; }

        string Message { get; }
    }
}
