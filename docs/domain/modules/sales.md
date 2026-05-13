# Sales Module

> **Responsibility:** Calculates the price of a reservation, applies discounts, processes the payment, and orchestrates the checkout flow via a saga.

- [Domain overview](../overview.md) | [Architecture overview](../../architecture/overview.md)

---

## Domain concepts

### Payment (Aggregate Root)

A `Payment` records the financial transaction for a single reservation.

| Property | Type | Description |
|----------|------|-------------|
| `PaymentId` | `TypedId<PaymentId>` | Unique identifier |
| `ReservationId` | `ReservationId` | The reservation being paid for |
| `Amount` | `double` | Final amount after discounts |
| `Status` | `PaymentStatusType` | `Pending` / `Confirmed` / `Canceled` |

**Factory:** `Payment.Create(reservationId, amount)` — starts in `Pending` status.

**`ApplyDiscount(amount)`** — replaces the stored amount with the discounted value.

**`ChangeStatus(status)`** — transitions to `Confirmed` or `Canceled`, raising the appropriate domain event each time.

#### Payment status

| Status | Meaning |
|--------|---------|
| `Pending` | Payment created, waiting for processing |
| `Confirmed` | Payment processed successfully |
| `Canceled` | Payment processing failed |

---

### Discount system

The discount system uses a composition of three patterns: **Specification**, **Strategy**, and **Rule**.

#### DiscountRule

The top-level entry point. Each rule encapsulates a complete discount scenario.

| Rule | Description |
|------|-------------|
| `MondaySpecialDiscountRule` | On **Mondays**, groups of more than 4 seats receive a **10% discount** (minimum total: £40) |
| `BreakfastInAmericaDiscountRule` | On **Tue–Thu mornings**, any booking receives a **£2 fixed discount** (minimum total: £8) |

Rules are applied sequentially by passing a `ReservationContext` through each rule's `ApplyDiscounts` method.

#### ReservationContext

A mutable value object that carries the data needed to evaluate discounts:

| Property | Description |
|----------|-------------|
| `Amount` | Current total (reduced as discounts are applied) |
| `StartAt` | Show start time (used for time/day-based rules) |
| `SeatsCount` | Number of seats (used for group rules) |

`ReduceAmount(discount)` subtracts the discount from `Amount`. It throws if the result would go negative.

#### DiscountSpecification

Decides whether a discount applies and, if so, delegates calculation to a `DiscountStrategy`.

| Specification | Condition |
|---------------|-----------|
| `MondayDiscountSpecification` | Show starts on a Monday |
| `MorningDiscountSpecification` | Show starts before noon |
| `GroupDiscountSpecification` | Reservation has more than 4 seats |
| `MinimumAmountDiscountSpecification` | Total must reach a minimum threshold |

Specifications can be composed with `AndSpecification` and `NotSpecification` (from `Cine.Shared.Domain.Specifications`).

#### DiscountStrategy

Calculates the monetary value of the discount given the `ReservationContext`.

| Strategy | Calculation |
|----------|------------|
| `PercentageDiscountStrategy(p)` | `amount × p × 0.01` |
| `FixedAmountDiscountStrategy(v)` | `v` (a flat reduction) |
| `EmptyDiscountStrategy` | `0` (no reduction — used as a no-op placeholder in composed specs) |

---

### Discount (Entity)

`Discount` is a persistable entity that can be toggled on/off. It carries a reference to the `DiscountSpecification` type. This is the persistence representation; the actual discount logic lives in the specification/strategy classes above.

---

## Domain events

| Event | Raised when |
|-------|------------|
| `PaymentCreatedDomainEvent` | A payment record is created |
| `PaymentDiscountAppliedDomainEvent` | A discount is applied to a payment |
| `PaymentConfirmedDomainEvent` | Payment confirmed; carries `ReservationId` |
| `PaymentCanceledDomainEvent` | Payment canceled |

---

## Integration events

| Event | Direction | Purpose |
|-------|-----------|---------|
| `ReservationConfirmedIntegrationEvent` | **Consumed** | Triggers the payment saga (published by Tickets) |
| `PaymentConfirmedIntegrationEvent` | **Published** | Tells Tickets to complete the reservation |

---

## Payment saga

The checkout flow is orchestrated by a **MassTransit state-machine saga** (`PaymentSaga`). See [Sagas](../../architecture/sagas.md) for the full state diagram and consumer details.

**High-level flow:**

1. `ReservationConfirmed` → saga starts, publishes `CreatePayment`
2. `CreatePaymentConsumer` → fetches reservation from Tickets API, calculates price, applies `MondaySpecialDiscountRule`, saves payment, publishes `PaymentCreated`
3. `ProcessPaymentConsumer` → simulates gateway call (5 s delay), publishes `PaymentSucceeded` or `PaymentFailed`
4. `ConfirmPaymentConsumer` → calls `ConfirmReservationCommand` on the Tickets API, changes payment status to `Confirmed`, publishes `ConfirmPayment` → saga finalises
5. `CancelPaymentConsumer` → changes payment status to `Canceled` → saga finalises

---

## Pricing

Seat price is configured via `Features:Payments:SeatPrice` (app settings).  
`Amount = SeatsCount × SeatPrice`  
Discounts are then applied by running the `ReservationContext` through the configured `DiscountRule` chain.

---

## HTTP endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/payments/{id}` | Get payment status |

---

## See also

- [Tickets module](./tickets.md) — source of reservation data (HTTP + integration event)
- [Architecture: Sagas](../../architecture/sagas.md)
- [Architecture: Event-driven design](../../architecture/event-driven.md)
