---
applyTo: "source/backend/**"
---

# Backend architecture

Use a pragmatic hexagonal architecture: recognizable boundaries with limited
enforcement, not deliberately broken code.

## Structure

Keep one production project initially. Add folders as behavior requires them,
without creating empty scaffolding:

- `Domain`: business concepts and rules.
- `Application`: use cases and ports, such as a repository interface.
- `Adapters/Inbound`: HTTP endpoints.
- `Adapters/Outbound`: persistence and external integrations.
- `Program.cs`: application composition and dependency registration.

## Boundaries

- Domain code must not depend on HTTP or persistence frameworks.
- Application code depends on domain code and ports, not concrete adapters.
- Endpoints delegate business decisions to application or domain code.
- Wire concrete adapters to ports at the composition root.

## Permitted trade-offs

- Use folders rather than separate assemblies to express boundaries.
- Prefer simple application services; do not introduce a mediator or command
  framework merely to follow an architectural pattern.
- Domain objects may serve as response models when their shape and exposed
  data are appropriate for the contract.
- Keep persistence straightforward; generic repositories and a unit-of-work
  abstraction are not required.
- Prefer limited duplication to premature shared abstractions.

Follow these boundaries for new behavior, but do not restructure unrelated
code. Where existing code departs from them, make the smallest safe change
and identify any relevant compromise. Do not manufacture defects or arbitrary
violations to simulate a brownfield system.
