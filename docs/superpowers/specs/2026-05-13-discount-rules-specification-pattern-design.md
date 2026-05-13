# Discount Rules — Specification Pattern Refactor

**Date:** 2026-05-13  
**Scope:** `src/Modules/Sales/Cine.Modules.Sales.Domain` — DiscountRules, DiscountSpecifications, DiscountStrategies  
**Trigger:** Code review findings — specification/strategy coupling causing order-dependent discount application and broken `NotSpecification` semantics.

---

## Problem

The existing `DiscountSpecification` abstract class conflates two concerns:

1. **Predicate** — `IsSatisfiedBy(ReservationContext)` — should this discount apply?
2. **Action** — `ApplyTo(ReservationContext)` — apply the discount amount.

This coupling breaks composite specifications:

- `AndSpecification.ApplyTo` calls each child's `ApplyTo` sequentially on a **mutating** context, making discount application order-dependent (e.g. a percentage discount after a fixed discount yields a different result than the reverse).
- `NotSpecification.ApplyTo` delegates to the inner spec's `ApplyTo`, meaning `Not(Monday)` applies a Monday discount on non-Mondays — the opposite of the intended predicate inversion.
- `BreakfastInAmericaDiscountRule` models Tue–Thu eligibility as four `Not(day)` negations, which is fragile and unreadable.

---

## Architecture

Three clean, independent layers:

```
ISpecification<ReservationContext>   ← pure predicates (IsSatisfiedBy only)
         ↓ used by
DiscountPolicy                       ← pairs one spec + one strategy, owns ApplyTo
         ↓ composed by
DiscountRule                         ← holds list of DiscountPolicies, drives execution
```

---

## Layer 1: Specifications (pure predicates)

All specs implement `ISpecification<ReservationContext>` from `Cine.Shared.Domain`. No spec carries a `DiscountStrategy`. No spec has `ApplyTo`. The `DiscountSpecification` abstract class is deleted.

### Leaf Specifications

| Class | Condition |
|---|---|
| `DayOfWeekSpecification(DayOfWeek, Func<ReservationContext, DateTime>)` | date selector returns the given day of week |
| `MondaySpecification` | delegates to `DayOfWeekSpecification(Monday, ctx => ctx.StartAt)` |
| `AllowedDaysSpecification(params DayOfWeek[] days)` | `days.Contains(ctx.StartAt.DayOfWeek)` — positive whitelist |
| `GroupSpecification` | `SeatsCount > 4` |
| `MorningSpecification` | `StartAt.Hour >= 8 && StartAt.Hour < 12` |
| `MinimumAmountSpecification(double minimum)` | `Amount >= minimum` (pre-discount check) |

> **Note on `MinimumAmountSpecification`:** The original implementation speculatively cloned the context and applied a discount to check if the post-discount amount still met the minimum. The new version checks the **pre-discount** amount. This removes the clone complexity and is semantically correct for guarding a policy: "only apply this discount if the current amount is at least X."

### Composite Specifications (predicate-only, no `ApplyTo`)

| Class | Semantics |
|---|---|
| `AndSpecification(params ISpecification<ReservationContext>[] specs)` | `specs.All(s => s.IsSatisfiedBy(ctx))` |
| `NotSpecification(ISpecification<ReservationContext> spec)` | `!spec.IsSatisfiedBy(ctx)` |

Composites carry no strategy and do not override `ApplyTo`. They are pure boolean combinators.

---

## Layer 2: Strategies

No structural changes. `DiscountStrategy` and its implementations (`FixedAmountDiscountStrategy`, `PercentageDiscountStrategy`, `EmptyDiscountStrategy`) are unchanged.

`EmptyDiscountStrategy` is no longer needed as a workaround for composite specs that had no strategy — it can be deleted if no remaining code references it after the refactor.

---

## Layer 3: Policy

**New class:** `DiscountPolicy` in `DiscountPolicies/` folder.

```csharp
internal sealed class DiscountPolicy(
    ISpecification<ReservationContext> specification,
    DiscountStrategy strategy)
{
    public void ApplyTo(ReservationContext context)
    {
        if (specification.IsSatisfiedBy(context))
            context.ReduceAmount(strategy.Calculate(context));
    }
}
```

One responsibility: check the spec, apply the strategy if satisfied. Fully testable in isolation.

---

## Layer 4: Rules

`DiscountRule` base class moves the iteration loop in, so concrete rules only declare their policies:

```csharp
public abstract class DiscountRule
{
    protected abstract IReadOnlyList<DiscountPolicy> Policies { get; }

    public double ApplyDiscounts(ReservationContext context)
    {
        foreach (var policy in Policies)
            policy.ApplyTo(context);
        return context.Amount;
    }
}
```

### `MondaySpecialDiscountRule`

One policy: Monday + group (>4 seats) + amount ≥ 40 → 10% off.

```csharp
public sealed class MondaySpecialDiscountRule : DiscountRule
{
    protected override IReadOnlyList<DiscountPolicy> Policies { get; } =
    [
        new DiscountPolicy(
            new AndSpecification(
                new MinimumAmountSpecification(40.0),
                new MondaySpecification(),
                new GroupSpecification()),
            new PercentageDiscountStrategy(10.0))
    ];
}
```

### `BreakfastInAmericaDiscountRule`

One policy: Tue–Thu + morning (08:00–12:00) + amount ≥ 8 → £2 fixed discount.  
Also fixed: class becomes `sealed` (was missing).

```csharp
public sealed class BreakfastInAmericaDiscountRule : DiscountRule
{
    protected override IReadOnlyList<DiscountPolicy> Policies { get; } =
    [
        new DiscountPolicy(
            new AndSpecification(
                new MinimumAmountSpecification(8.0),
                new AllowedDaysSpecification(
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday),
                new MorningSpecification()),
            new FixedAmountDiscountStrategy(2.0))
    ];
}
```

---

## Data Flow

```
ApplyDiscounts(context)
  └─ foreach policy in Policies
       └─ policy.ApplyTo(context)
            └─ specification.IsSatisfiedBy(context) → bool
                 └─ if true: context.ReduceAmount(strategy.Calculate(context))
```

Each policy reads and writes `context.Amount` in declared order. Because each policy evaluates its own spec independently against the current (already-mutated-by-prior-policies) amount, ordering still matters for policies within a single rule — but this is now explicit and intentional, not an accidental side-effect of composite internals.

---

## Files Changed

| Action | File |
|---|---|
| Delete | `DiscountSpecifications/DiscountSpecification.cs` |
| Rewrite | `DiscountSpecifications/CommonSpecifications.cs` — And/Not become pure predicates; DayOfWeekSpecification stripped of strategy |
| Rewrite | `DiscountSpecifications/MinimumAmountDiscountSpecification.cs` → `MinimumAmountSpecification.cs` |
| Rewrite | `DiscountSpecifications/GroupDiscountSpecification.cs` → `GroupSpecification.cs` |
| Rewrite | `DiscountSpecifications/MondayDiscountSpecification.cs` → `MondaySpecification.cs` |
| Rewrite | `DiscountSpecifications/MorningDiscountSpecification.cs` → `MorningSpecification.cs` |
| Add | `DiscountSpecifications/AllowedDaysSpecification.cs` |
| Add | `DiscountPolicies/DiscountPolicy.cs` |
| Rewrite | `DiscountRules/DiscountRule.cs` |
| Rewrite | `DiscountRules/MondaySpecialDiscountRule.cs` |
| Rewrite | `DiscountRules/BreakfastInAmericaDiscountRule.cs` |
| Rewrite | `tests/.../DiscountsTests.cs` — meaningful test names, test via DiscountPolicy |

---

## Testing

Tests target behaviour, not implementation internals:

- **Leaf spec tests** — verify each `IsSatisfiedBy` in isolation (no context mutation needed).
- **DiscountPolicy tests** — verify that when spec is satisfied the correct amount is deducted; when not satisfied the amount is unchanged.
- **Rule integration tests** — verify full rule output (existing Sample1–Sample5 scenarios preserved, renamed to meaningful names).
