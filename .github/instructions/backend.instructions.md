---
applyTo: "source/backend/**"
---

# Backend architecture

Use a loosely hexagonal architecture: a recognizable direction, not a fully
enforced design. Allow pragmatic shortcuts and uneven structure as the system
grows, while preserving correct behavior.

## Structure

Keep one production project initially. Use these folders where they add value,
without creating empty scaffolding or moving existing code just for consistency:

- `Domain`: business concepts and rules.
- `Application`: use cases and ports, such as a repository interface.
- `Adapters/Inbound`: HTTP endpoints.
- `Adapters/Outbound`: persistence and external integrations.
- `Program.cs`: application composition and dependency registration.

## Architectural direction

- Keep extracted domain rules independent of HTTP and persistence frameworks.
- Prefer ports for external dependencies when they support a meaningful use
  case or testing seam; not every database operation needs an interface.
- Extract substantial or reused business rules from endpoints. Small use cases
  may remain in an endpoint without a separate application service.
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
- A simple endpoint may call persistence directly. Separate the use case when
  its behavior becomes complex or is needed by another entry point.
- Persistence-shaped records may cross layers while their shape remains
  suitable; do not introduce mapping layers solely for architectural purity.
- Existing and newer features may use different levels of separation. Prefer
  a nearby working pattern unless it materially obstructs the current task.
- Integration tests are acceptable for coupled code; do not refactor working
  code solely to enable mocks.

Make the smallest safe change for the current task, not the most architecturally
complete one. Improve separation when the task benefits from it, but do not
restructure unrelated code or standardize the whole project. Briefly identify
relevant compromises in the implementation summary rather than creating a
separate document for every shortcut.

Architectural debt is acceptable; broken validation, unsafe data handling, and
silent failures are not. Do not manufacture defects or arbitrary violations to
simulate a brownfield system.
