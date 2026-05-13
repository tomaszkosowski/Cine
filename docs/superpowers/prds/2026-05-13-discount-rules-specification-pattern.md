# PRD: Discount Rules — Specification Pattern Refactor

**Type:** Technical PRD  
**Status:** Draft  
**Date:** 2026-05-13  
**Module:** `Cine.Modules.Sales.Domain`  
**Design Spec:** `docs/superpowers/specs/2026-05-13-discount-rules-specification-pattern-design.md`

---

## 1. Executive Summary

The Sales module's discount domain currently conflates predicate evaluation with discount application inside a single `DiscountSpecification` abstract class. This causes composite specifications (`AndSpecification`, `NotSpecification`) to produce order-dependent, semantically incorrect discount calculations. This PRD covers the refactor of the discount domain to a clean three-layer architecture — pure specifications, discount policies, and discount rules — that correctly separates concerns, eliminates the identified bugs, and makes the system straightforward to extend with new discount rules.

---

## 2. Problem Statement

### Current State

`DiscountSpecification` is an abstract class that implements both `IsSatisfiedBy(ReservationContext)` (predicate) and `ApplyTo(ReservationContext)` (action) while also owning a `DiscountStrategy`. Composite specs inherit and override `ApplyTo`.

### Problems

| # | Problem | Severity |
|---|---|---|
| 1 | `AndSpecification.ApplyTo` calls each child's `ApplyTo` sequentially on a mutating context — discount results change depending on declaration order | HIGH |
| 2 | `NotSpecification.ApplyTo` delegates to the inner spec's `ApplyTo`, meaning `Not(MondayDiscount)` applies a Monday discount on every day *except* Monday | HIGH |
| 3 | `BreakfastInAmericaDiscountRule` models Tue–Thu eligibility as four `Not(day)` negations — fragile, unreadable, and will silently break if a new rule adds a day | MEDIUM |
| 4 | `BreakfastInAmericaDiscountRule` is not `sealed`, allowing unintended inheritance | LOW |

### Evidence

- Problem 1: `AndSpecification` lines 11–22 (`CommonSpecifications.cs`) — each child `ApplyTo` mutates `reservationContext.Amount` before the next child evaluates its spec.
- Problem 2: `NotSpecification` lines 31–39 (`CommonSpecifications.cs`) — when `!IsSatisfiedBy` is true (i.e., spec *is not* satisfied), it calls `specification.ApplyTo(reservationContext)` which applies the inner discount.
- Problem 3: `BreakfastInAmericaDiscountRule` lines 13–17 — four `new NotSpecification(new DayOfWeekSpecification(...))` constructions to express "not Mon, not Fri, not Sat, not Sun."

### Why Now

The `CreatePaymentConsumer` in `Application.Sagas` instantiates `MondaySpecialDiscountRule` directly. As the business adds more discount rules (e.g., seasonal promotions, loyalty tiers), the current architecture will produce compounding, hard-to-diagnose discount calculation errors.

---

## 3. Objectives and Success Metrics

### Product Objectives

- Discount calculations are **deterministic and correct** regardless of specification composition order.
- New discount rules can be added by composing existing specs and strategies — no changes to base infrastructure required.
- The domain model is **readable** — a developer unfamiliar with the codebase can understand a rule's eligibility conditions and discount amount from its source alone.

### Business Objectives

- Eliminate the risk of incorrect discount amounts being applied to customer payments.
- Reduce the time to implement a new discount rule.

### Success Metrics

| Metric | Baseline | Target |
|---|---|---|
| Discount calculation correctness (unit tests pass) | Partial (bugs masked by limited test data) | 100% pass rate including edge cases |
| Lines of code per new discount rule | ~20–30 (with workarounds) | ≤15 (spec + policy composition only) |
| `AndSpecification` order-dependent failures | Present | Zero |
| `NotSpecification` inverted-application failures | Present | Zero |

### Guardrail Metrics

- No regression in existing payment processing (all integration tests pass).
- No change to `ReservationContext`'s public API — consumers (`CreatePaymentConsumer`) require no changes beyond the discount rule instantiation.

---

## 4. User Stories and Personas

### Personas

**Developer — Sales Module Contributor**  
A backend developer adding or maintaining discount rules in the Sales module. Familiar with C# and DDD patterns. Expects domain objects to behave as named (a `NotSpecification` should invert a predicate, not apply a discount). Expects composite specs to be safe to compose in any order.

**Developer — Platform Contributor**  
A developer in another module who consumes integration events or reads the Sales domain for reference. Relies on the Sales domain being a correct reference implementation of specification and strategy patterns.

### User Stories

**US-01 — Correct And Composition**  
*As a Sales module developer, I want `AndSpecification` to evaluate all child predicates before applying any discount, so that composing multiple specs produces the same result regardless of their order in the constructor.*

Acceptance criteria:
- `AndSpecification(MinimumAmount(40), Monday, Group).IsSatisfiedBy(ctx)` returns `true` only when all three conditions are met.
- Applying an `AndSpecification`-based policy deducts the discount exactly once.
- Swapping the order of specs inside `AndSpecification` does not change the calculated discount amount.

---

**US-02 — Correct Not Semantics**  
*As a Sales module developer, I want `NotSpecification` to invert predicate logic only, so that it never accidentally applies a discount on behalf of the spec it wraps.*

Acceptance criteria:
- `NotSpecification(Monday).IsSatisfiedBy(ctx)` returns `true` on non-Monday dates.
- `NotSpecification` has no `ApplyTo` method and carries no `DiscountStrategy`.
- A rule using `NotSpecification` as a filter applies the correct discount strategy, not the wrapped spec's (non-existent) strategy.

---

**US-03 — Readable Day Whitelist**  
*As a Sales module developer, I want to express "Tue–Thu only" as a positive allowed-days specification, so that adding or removing an allowed day requires a single, obvious change.*

Acceptance criteria:
- `AllowedDaysSpecification(Tuesday, Wednesday, Thursday)` returns `true` only on those three days.
- `BreakfastInAmericaDiscountRule` uses `AllowedDaysSpecification` instead of four `NotSpecification` negations.
- Adding a new allowed day is a single-line change with no risk of accidentally missing a negation.

---

**US-04 — Isolated Policy Unit**  
*As a Sales module developer, I want a `DiscountPolicy` class that pairs a specification with a strategy, so that I can test discount application independently of rule composition.*

Acceptance criteria:
- `DiscountPolicy(spec, strategy).ApplyTo(ctx)` reduces `ctx.Amount` by `strategy.Calculate(ctx)` when `spec.IsSatisfiedBy(ctx)` is `true`.
- `DiscountPolicy.ApplyTo(ctx)` makes no change to `ctx.Amount` when `spec.IsSatisfiedBy(ctx)` is `false`.
- `DiscountPolicy` is independently unit-testable with no dependency on `DiscountRule`.

---

**US-05 — Declarative Rule Definition**  
*As a Sales module developer, I want `DiscountRule` subclasses to declare their policies as data rather than imperative logic, so that the rule's behaviour is immediately visible without reading control flow.*

Acceptance criteria:
- `DiscountRule` base class implements `ApplyDiscounts` by iterating `Policies`.
- Concrete rules expose only `protected override IReadOnlyList<DiscountPolicy> Policies { get; }`.
- `MondaySpecialDiscountRule` and `BreakfastInAmericaDiscountRule` have no `ApplyDiscounts` override.

---

## 5. Functional Requirements

### Must-Have (MVP)

| ID | Requirement | Acceptance Criteria |
|---|---|---|
| FR-01 | All specification classes implement `ISpecification<ReservationContext>` (pure predicate) only — no `ApplyTo`, no embedded `DiscountStrategy` | Build passes; no spec class references `DiscountStrategy` |
| FR-02 | `AndSpecification` is a pure predicate combinator with no `ApplyTo` override | Unit test: composing Monday + Group + MinAmount specs satisfies and correctly evaluates all three |
| FR-03 | `NotSpecification` is a pure predicate combinator with no `ApplyTo` override | Unit test: `Not(Monday)` is satisfied on Tuesday and unsatisfied on Monday |
| FR-04 | `DiscountPolicy(ISpecification<ReservationContext>, DiscountStrategy)` created and owns sole `ApplyTo` | Unit test: policy applies discount when spec satisfied; no-op when not |
| FR-05 | `DiscountRule` base class iterates `Policies` in `ApplyDiscounts` | Both concrete rules pass through base `ApplyDiscounts` without override |
| FR-06 | `MondaySpecialDiscountRule` refactored: Monday + group + amount ≥ 40 → 10% | Existing Sample4 test scenario passes |
| FR-07 | `BreakfastInAmericaDiscountRule` refactored: Tue–Thu + morning + amount ≥ 8 → £2 fixed, class sealed | Existing Sample5 test scenario passes |
| FR-08 | `AllowedDaysSpecification(params DayOfWeek[])` added | Unit test: returns true for allowed days, false for others |
| FR-09 | `DiscountSpecification` abstract class deleted | No compilation error; no remaining references |
| FR-10 | Unit tests renamed to meaningful names and refactored to test via `DiscountPolicy` | All tests pass; test names describe business scenario |

### Should-Have

| ID | Requirement | Notes |
|---|---|---|
| FR-11 | `EmptyDiscountStrategy` deleted if no remaining references | Verify with build after FR-01–FR-09 complete |
| FR-12 | `MinimumAmountSpecification` semantics documented in XML doc comment | Clarifies pre-discount check intent for future contributors |

### Could-Have

| ID | Requirement | Notes |
|---|---|---|
| FR-13 | `OrSpecification` combinator added (consistent with And/Not) | Not required by current rules but completes the combinator set |

### Won't-Have

- New discount rule types beyond the two existing rules.
- Changes to `ReservationContext`'s public API.
- Changes outside the Sales domain (other modules, infrastructure, API layer).
- ORM/migration changes.

---

## 6. Non-Functional Requirements

| Category | Requirement |
|---|---|
| **Correctness** | Discount calculations must be deterministic — same input always produces same output regardless of internal spec declaration order |
| **Testability** | Every new class (`DiscountPolicy`, each leaf spec, each composite) must be independently unit-testable with no infrastructure dependencies |
| **Extensibility** | Adding a new discount rule requires only: new spec classes (if needed) + new `DiscountRule` subclass. No changes to `DiscountPolicy`, `DiscountRule` base, or any existing rule |
| **Readability** | A developer should be able to read a concrete `DiscountRule` and understand eligibility conditions and discount amount without reading base class or shared infrastructure |
| **Dependency rules** | Domain layer must not reference Application or Infrastructure (existing architecture test must continue to pass) |
| **Build** | Solution builds with zero warnings after refactor |
| **Test coverage** | All existing discount test scenarios (Sample1–Sample5) remain covered; new tests added for `DiscountPolicy` isolation and `AllowedDaysSpecification` |

---

## 7. Scope Boundaries (Out of Scope)

| Item | Rationale |
|---|---|
| `CreatePaymentConsumer` changes | Consumer instantiates `MondaySpecialDiscountRule()` with no-arg constructor — public API of rule is unchanged |
| `ReservationContext` API changes | Public surface unchanged; `Clone()` may be removable as a side-effect but is not required |
| New discount rules (seasonal, loyalty, etc.) | Out of scope for this refactor; the refactored architecture enables them without further structural change |
| `Discount` aggregate entity refactor | The `Discount` class (`Discount.cs`) has separate issues (unused `isActive` constructor param, unused `DiscountSpecification` property); these are unrelated to specification-pattern correctness |
| Other modules | No changes to Theater, Movies, Shows, or Tickets domains |
| Integration/saga tests | Existing integration tests should pass without modification; no new integration tests required for this refactor |

---

## 8. Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| `MinimumAmountSpecification` pre-discount semantics change breaks an existing rule scenario | Medium | High | Run all existing unit test scenarios before and after; explicitly verify Sample4 (amount=80 → 72) and Sample5 (amount=10 → 10) |
| `EmptyDiscountStrategy` still referenced somewhere after deletion | Low | Medium | Search all projects for references before deleting; keep if any reference found |
| `BreakfastInAmericaDiscountRule` Sample5 test currently expects no discount (amount stays 10) because minimum not met — refactor must preserve this | Medium | High | Keep Sample5 test as a regression guard; verify `MinimumAmountSpecification(8.0)` with amount=10 is satisfied, morning hour=8 is satisfied, Friday is excluded by `AllowedDaysSpecification` |
| Architecture tests fail if `DiscountPolicies/` folder is placed incorrectly | Low | Low | Keep `DiscountPolicies/` inside `Cine.Modules.Sales.Domain` project — same project as specs and strategies |

---

## 9. Milestones

| Phase | Deliverable |
|---|---|
| 1 — Specifications | All leaf and composite specs rewritten as pure `ISpecification<ReservationContext>`; `DiscountSpecification` deleted; `AllowedDaysSpecification` added |
| 2 — Policy | `DiscountPolicy` class created and unit-tested |
| 3 — Rules | `DiscountRule` base + both concrete rules refactored; `BreakfastInAmericaDiscountRule` sealed |
| 4 — Cleanup | `EmptyDiscountStrategy` removed if unused; old spec files deleted |
| 5 — Tests | Unit tests refactored to meaningful names; policy isolation tests added; all tests pass |

---

*Derived from design spec: `docs/superpowers/specs/2026-05-13-discount-rules-specification-pattern-design.md`*
