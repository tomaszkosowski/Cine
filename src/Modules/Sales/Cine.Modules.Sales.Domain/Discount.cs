using Cine.Shared.Domain;

namespace Cine.Modules.Sales.Domain;

public record DiscountId : TypedId<DiscountId>;

public class Discount
{
    #region Properties

    public DiscountId DiscountId { get; }

    public Type DiscountSpecyfication { get; }

    public bool IsActive { get; private set; }

    #endregion

    #region Constructors

    private Discount()
    {
        // Blank for ORM.
    }

    private Discount(bool isActive)
    {
        DiscountId = DiscountId.Create();
    }

    #endregion

    #region Public methods

    public static Discount Create() => new(true);

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    #endregion
}