# Task 004 — Refactor `BreakfastInAmericaDiscountRule` with allowed days

**Priority:** Must  
**Story Points:** 3  
**Recommended Agent:** code-reviewer

## Objective
Replace negative day checks with a positive allowed-days whitelist and migrate to policy composition.

## Scope
- Add `AllowedDaysSpecification(params DayOfWeek[])`.
- Refactor rule to: `MinimumAmount(8) AND AllowedDays(Tue, Wed, Thu) AND Morning` -> `Fixed(2)`.
- Make `BreakfastInAmericaDiscountRule` sealed.

## Acceptance Criteria
- Rule contains no chained `NotSpecification` day exclusions.
- Allowed-day logic is explicit and readable.
- Friday morning regression scenario remains unchanged.

## Dependencies
- task-001
- task-002

