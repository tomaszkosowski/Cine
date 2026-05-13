# Status — Discount Rules Specification Pattern Refactor

## Progress
- Overall: 100%
- Current phase: Completed

## Task Board
| Task ID | Title | Priority | SP | Status | Depends On |
|---|---|---|---:|---|---|
| task-001 | Refactor specification abstractions to pure predicates | Must | 5 | Done | - |
| task-002 | Add `DiscountPolicy` and migrate `DiscountRule` base flow | Must | 3 | Done | task-001 |
| task-003 | Refactor `MondaySpecialDiscountRule` to policy composition | Must | 2 | Done | task-001, task-002 |
| task-004 | Refactor `BreakfastInAmericaDiscountRule` with `AllowedDaysSpecification` | Must | 3 | Done | task-001, task-002 |
| task-005 | Clean up legacy artifacts and naming consistency | Should | 2 | Done | task-003, task-004 |
| task-006 | Refactor and expand unit tests for policy/spec behavior | Must | 5 | Done | task-003, task-004 |
| task-007 | Run build/tests and finalize plan outcomes | Must | 2 | Done | task-005, task-006 |

## Dependency Graph
`task-001 -> task-002 -> (task-003, task-004) -> task-005 -> task-006 -> task-007`

## Next Steps
1. Create a commit for the completed refactor and tests.
2. Open a PR that references the spec and PRD artifacts.
3. Merge after CI and review sign-off.

## Completion Notes
- Specification classes are now predicate-only (`ISpecification<ReservationContext>`), with no strategy coupling and no `ApplyTo`.
- Added `DiscountPolicy` as the sole discount application unit.
- Refactored both discount rules to declarative policy lists.
- Added `AllowedDaysSpecification` and removed legacy `DiscountSpecification`/`EmptyDiscountStrategy`.
- Sales domain unit tests were refactored and expanded for policy/spec behavior and rule scenarios.
- Full solution integration tests still fail in this environment due Testcontainers/Docker startup issues (same external class-fixture failure pattern).
