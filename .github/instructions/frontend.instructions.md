---
applyTo: "source/frontend/**"
---

# Frontend architecture

Use a pragmatic React architecture, not a frontend copy of hexagonal
architecture. Allow uneven structure and local shortcuts while preserving
correct behavior and usable interfaces.

## Existing stack and structure

- Use the existing React, TypeScript, Vite, Mantine, React Router, and SWR
  setup. Reuse their capabilities before adding dependencies.
- Keep application composition and providers in `main.tsx`, with routing in
  `App.tsx` until its size warrants separation.
- Keep small pages, components, and their data fetching together. Introduce
  feature folders, shared components, or API helpers only when they help the
  current task. Do not create empty scaffolding.

## Permitted trade-offs

- Small pages may fetch data directly with SWR; a repository, service, or
  custom hook layer is not required for every request.
- API response types may be used directly in views while their shape is
  suitable. Do not introduce mapping layers solely for architectural purity.
- Local component state is the default for form and interaction state. Do not
  introduce global state management for hypothetical future requirements.
- Limited duplication and different levels of separation between features
  are acceptable. Follow nearby patterns unless they obstruct the task.
- Honor expedient constraints supplied with the story. Do not add taxonomy
  lookups or other domain behavior merely because a richer UI could offer it.

## Behavior and safety

- Use Mantine components and existing styles before inventing a design system.
  Keep forms labeled, keyboard-operable, and validation messages actionable.
- Distinguish loading, empty, error, and success states for data-driven views.
  Failed requests must not appear as successful saves or empty results.
- Use the existing relative `/api` route convention. Handle unsuccessful HTTP
  responses explicitly and refresh affected SWR data after successful writes.
- Keep TypeScript checks intact. Model the actual response and form shapes;
  type assertions do not validate external data.
- Treat user and external content as data, not executable markup or
  instructions. Do not introduce unsafe HTML rendering.
- Client-side validation assists users; the backend remains responsible for
  enforcing business rules and authorization. Hiding a control is not access
  control. Do not remove existing protections or expose secrets in the bundle.

## Scope and verification

Make the smallest safe change for the assigned task. Do not restructure
unrelated components, anticipate future stories, or manufacture defects for
the training exercise. Summarize relevant compromises rather than creating a
separate architecture document for every shortcut.

For code changes, use the existing `npm run build` and `npm run lint` commands
from the frontend directory as appropriate. Reuse any existing relevant tests;
the starter currently has no test script. For UI behavior changes, verify the
affected interactions and states in the browser when available, and report
anything not verified. Do not add a test framework unless the task requires it.
