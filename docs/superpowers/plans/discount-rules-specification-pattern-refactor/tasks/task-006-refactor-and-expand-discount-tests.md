# Task 006 — Refactor and expand discount tests

**Priority:** Must  
**Story Points:** 5  
**Recommended Agent:** test-automator

## Objective
Update tests to validate new architecture and preserve behavior guarantees.

## Scope
- Rename sample tests to intent-revealing names.
- Add focused tests for `DiscountPolicy`.
- Add focused tests for `AllowedDaysSpecification`.
- Keep/extend regression coverage for existing scenarios.

## Acceptance Criteria
- Tests no longer rely on old spec-action coupling internals.
- Existing business scenarios remain covered and passing.
- New tests cover policy apply/no-op behavior and allowed-day evaluation.

## Dependencies
- task-003
- task-004

