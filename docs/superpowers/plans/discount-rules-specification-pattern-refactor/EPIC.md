# EPIC: Discount Rules — Specification Pattern Refactor

**Source PRD:** `docs/superpowers/prds/2026-05-13-discount-rules-specification-pattern.md`  
**Scope:** `src/Modules/Sales/Cine.Modules.Sales.Domain` and related Sales domain unit tests  
**Priority:** High  
**Plan Type:** Filesystem

## Executive Summary
Refactor Sales discount modeling to separate predicate evaluation from discount application. The current implementation couples specifications and strategies, causing order-dependent calculations and incorrect `NotSpecification` behavior. This epic introduces pure specifications, a dedicated `DiscountPolicy`, declarative rules, and targeted tests to remove correctness risks and improve maintainability.

## Business Value
- Prevent incorrect discount application in payment creation flow.
- Reduce risk and effort when adding future discount rules.
- Improve readability and testability of domain logic.

## Success Metrics
- Zero order-dependent discount failures in unit tests.
- Zero `NotSpecification` behavior regressions.
- Existing discount scenarios continue to pass.
- New policy/spec-focused tests pass.

## Milestones
1. Specification layer refactor complete.
2. `DiscountPolicy` introduced and tested.
3. Rule definitions migrated to declarative policies.
4. Legacy artifacts cleaned up (`DiscountSpecification`, optional `EmptyDiscountStrategy`).
5. Unit tests modernized and regression coverage confirmed.

## External Dependencies
- None outside repository.
- Must continue to satisfy architecture constraints for Domain layer.

