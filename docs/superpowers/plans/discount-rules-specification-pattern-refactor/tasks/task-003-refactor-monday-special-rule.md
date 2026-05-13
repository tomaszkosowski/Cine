# Task 003 — Refactor `MondaySpecialDiscountRule`

**Priority:** Must  
**Story Points:** 2  
**Recommended Agent:** code-reviewer

## Objective
Migrate Monday special logic to declarative policy composition.

## Scope
- Replace imperative/spec-strategy hybrid flow with policy list.
- Express rule as: `MinimumAmount(40) AND Monday AND Group` -> `10%`.

## Acceptance Criteria
- Rule no longer overrides custom imperative discount loop.
- Rule logic is represented as `DiscountPolicy` composition only.
- Existing scenario for Monday special discount remains correct.

## Dependencies
- task-001
- task-002

