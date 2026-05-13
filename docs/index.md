# Cine – Documentation

Welcome to the Cine documentation. The docs are split into two areas:

| Area | What you'll find |
|------|-----------------|
| [Domain](./domain/overview.md) | Business concepts, rules, vocabulary, and how the modules relate to each other |
| [Architecture](./architecture/overview.md) | Technical patterns, project layout, and cross-cutting infrastructure |

---

## Domain documentation

| Document | Summary |
|----------|---------|
| [Domain overview](./domain/overview.md) | Ubiquitous language, aggregate map, cross-module relationships |
| [Movies module](./domain/modules/movies.md) | Movie catalogue – titles, genres, cast & directors |
| [Theater module](./domain/modules/theater.md) | Physical halls and seat configuration |
| [Shows module](./domain/modules/shows.md) | Scheduling movies in halls |
| [Tickets module](./domain/modules/tickets.md) | Seat reservations and their lifecycle |
| [Sales module](./domain/modules/sales.md) | Payments, discounts, and the checkout saga |

---

## Architecture documentation

| Document | Summary |
|----------|---------|
| [Architecture overview](./architecture/overview.md) | Modular monolith structure, layer rules, project naming |
| [Shared kernel](./architecture/shared-kernel.md) | Shared.Domain / Application / Infrastructure primitives |
| [CQRS](./architecture/cqrs.md) | Commands, queries, validation, and result types |
| [Event-driven design](./architecture/event-driven.md) | Domain events → Outbox → RabbitMQ → Integration events |
| [Sagas](./architecture/sagas.md) | MassTransit state-machine for the payment flow |
| [Testing](./architecture/testing.md) | Unit, integration, and architecture test strategies |
