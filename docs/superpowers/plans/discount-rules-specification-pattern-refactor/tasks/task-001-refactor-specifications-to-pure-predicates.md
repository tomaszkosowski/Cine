# Task 001 — Refactor specification abstractions to pure predicates

**Priority:** Must  
**Story Points:** 5  
**Recommended Agent:** code-reviewer

## Objective
Remove action/strategy concerns from specification types so all specs are predicate-only.

## Scope
- Delete `DiscountSpecification` base class.
- Convert existing specification classes to implement `ISpecification<ReservationContext>`.
- Ensure `AndSpecification` and `NotSpecification` are boolean combinators only.
- Keep behavior aligned with PRD-defined semantics.

## Acceptance Criteria
- No specification class exposes `ApplyTo`.
- No specification class depends on `DiscountStrategy`.
- `AndSpecification` and `NotSpecification` compile and operate as pure predicates.
- Build succeeds for Sales domain projects.

## Dependencies
- None.

