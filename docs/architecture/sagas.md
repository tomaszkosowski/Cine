# Sagas

> How the Sales module orchestrates the multi-step payment flow using a MassTransit state-machine saga.

- [Back to architecture overview](./overview.md)
- [Sales domain](../domain/modules/sales.md)

---

## Why a saga?

Confirming a reservation and processing a payment involves multiple steps across time. A simple command handler isn't enough because:

- The payment gateway call can be slow or fail
- Each step needs to know the outcome of the previous one
- Compensating actions (cancellation) must happen on failure

MassTransit's `MassTransitStateMachine<TState>` provides a durable, event-driven state machine that coordinates these steps without blocking threads.

---

## State machine — PaymentSaga

The saga correlates all messages by `ReservationId`.

### States

| State | Description |
|-------|-------------|
| `Initial` | No saga instance yet |
| `PaymentPending` | Reservation confirmed; waiting for payment outcome |
| `Final` | Payment succeeded or failed; saga is finished |

### Events

| Event type | Carries | Source |
|------------|---------|--------|
| `ReservationConfirmed` | `ReservationId` | Tickets (integration event) |
| `PaymentCreated` | `ReservationId` | `CreatePaymentConsumer` |
| `PaymentSucceeded` | `ReservationId` | `ProcessPaymentConsumer` |
| `PaymentFailed` | `ReservationId` | `ProcessPaymentConsumer` |

### State transitions

```
[Initial]
    │  ReservationConfirmed
    │  → store ReservationId
    │  → Publish CreatePayment
    ▼
[PaymentPending]
    │  PaymentCreated
    │  → Publish ProcessPayment
    │
    ├─ PaymentSucceeded
    │  → Publish ConfirmPayment
    │  → Finalize
    │
    └─ PaymentFailed
       → Publish CancelPayment
       → Finalize
[Final]
```

---

## Consumers

Each saga transition publishes a message that a dedicated `IConsumer<T>` handles. Consumers are in `Cine.Modules.Sales.Application.Sagas`.

### CreatePaymentConsumer

**Trigger:** `CreatePayment` message published by the saga.

1. Fetches the reservation from the Tickets API via Refit (`ITicketsApiClient.GetReservationAsync`)
2. Calculates `Amount = SeatsCount × SeatPrice` (from config)
3. Creates a `Payment` aggregate
4. Applies `MondaySpecialDiscountRule` (see [Sales domain](../domain/modules/sales.md#discount-system))
5. Persists the payment
6. Publishes `PaymentCreated`

### ProcessPaymentConsumer

**Trigger:** `ProcessPayment` message published by the saga.

1. Loads the payment from the repository
2. Simulates a 5-second payment gateway call
3. Publishes `PaymentSucceeded` or `PaymentFailed`

> The current implementation always succeeds (`const bool IsSucceeded = true`). This is a placeholder for a real payment gateway integration.

### ConfirmPaymentConsumer

**Trigger:** `ConfirmPayment` message published by the saga on success.

1. Calls the Tickets API to confirm the reservation via HTTP
2. Changes payment status to `Confirmed` (`PaymentStatusType.Confirmed`)
3. Publishes `ConfirmPayment` (which triggers the saga to finalize)

### CancelPaymentConsumer

**Trigger:** `CancelPayment` message published by the saga on failure.

1. Changes payment status to `Canceled`
2. Saga finalizes without completing the reservation

---

## API clients used by the saga

The saga consumers call other modules over HTTP using Refit:

| Client | Interface | Base URL config key | Used by |
|--------|-----------|--------------------|---------| 
| `ITicketsApiClient` | `GET /reservations/{id}` | `Clients:TicketsApi` | `CreatePaymentConsumer` |

---

## Saga persistence

The saga state (`PaymentState`) is persisted via **MassTransit Entity Framework Core** (`MassTransit.EntityFrameworkCore`) using SQL Server. The state includes `ReservationId` and `CurrentState`.

---

## See also

- [Sales domain](../domain/modules/sales.md)
- [Event-driven design](./event-driven.md) — how `ReservationConfirmedIntegrationEvent` reaches the saga
- [Tickets module](../domain/modules/tickets.md) — source of reservation confirmations
