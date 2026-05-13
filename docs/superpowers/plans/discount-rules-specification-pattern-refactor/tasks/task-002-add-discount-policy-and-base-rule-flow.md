# Task 002 — Add `DiscountPolicy` and migrate `DiscountRule` base flow

**Priority:** Must  
**Story Points:** 3  
**Recommended Agent:** code-reviewer

## Objective
Introduce a dedicated policy unit that pairs a predicate spec with a strategy, and centralize rule execution in the base class.

## Scope
- Add `DiscountPolicies/DiscountPolicy.cs`.
- Update `DiscountRule` to iterate `Policies` and return final amount.
- Keep rule public API (`ApplyDiscounts`) stable.

## Acceptance Criteria
- `DiscountPolicy.ApplyTo` applies strategy only when spec is satisfied.
- `DiscountRule` exposes declarative `Policies` and owns the apply loop.
- Existing consumers compile without contract changes.

## Dependencies
- task-001

