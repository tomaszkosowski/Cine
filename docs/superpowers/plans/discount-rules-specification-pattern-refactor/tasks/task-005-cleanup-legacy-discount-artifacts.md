# Task 005 — Cleanup legacy discount artifacts

**Priority:** Should  
**Story Points:** 2  
**Recommended Agent:** code-reviewer

## Objective
Remove obsolete types/files from the old hybrid architecture and align naming.

## Scope
- Remove old discount specification files/types replaced by new predicate equivalents.
- Remove `EmptyDiscountStrategy` only if no references remain.
- Align class/file names for clarity (`*Specification` conventions).

## Acceptance Criteria
- No dead references to removed legacy specification classes.
- No dead references to `EmptyDiscountStrategy` if removed.
- Project compiles with cleaned structure.

## Dependencies
- task-003
- task-004

