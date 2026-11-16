---
name: refine-epic
description: Plan or refine an epic into features, user stories, definitions of done, and implementable tasks in the repository's planning notes.
---

# Refine an epic

Use this skill when a developer asks you to plan an epic or break one down
into features, user stories, or tasks. Produce planning notes, not application
code or tracked work items, unless the developer explicitly asks otherwise.

## Establish context

1. Identify the epic or outcome and the requested depth of refinement. If
   the developer supplies only a topic, draft an epic around it.
2. Read the relevant existing planning notes, glossary, and identifier
   conventions. Inspect the codebase when it helps establish existing
   behavior or constraints, without assuming that a feature is missing.
3. If an unresolved choice materially affects scope, behavior, or domain
   meaning, ask the developer. Mark domain assumptions and decisions needing
   specialist review rather than presenting guesses as facts.

## Refine the work

- Describe the epic's intended outcome and its boundaries. Include candidate
  outcome-focused KPIs when creating an epic or when asked for them; do not
  invent baselines or targets.
- Split the epic into distinct features that provide meaningful capabilities.
  Identify a small, useful first delivery when prioritization is requested.
- For each feature in scope, write user stories identifying an actor, a need,
  and the value of meeting it. Keep stories small enough to deliver and verify.
- For each story in scope, add concrete, appropriately sized task items and a
  story-specific definition of done. Include relevant validation or tests and
  any domain decision that must be resolved. State observable outcomes rather
  than simply repeating the task list.
- Keep tasks consistent with existing technology and architecture when known;
  do not invent a stack or add implementation detail unrelated to the story.
  Note dependencies or open decisions where they affect delivery.
- Preserve existing content and identifiers unless a change is requested or
  necessary for consistency. Avoid expanding neighboring work beyond the
  requested scope.

## Organize and verify

- Follow the existing `planning/` layout: an epic overview in its own folder,
  with feature files under `features/` for their stories and tasks. Update
  indexes and the shared glossary when directly relevant.
- Assign new work items unique identifiers using the documented `E-`, `F-`,
  `US-`, and `T-` prefixes. Check existing identifiers across the planning
  notes before assigning new numbers; do not renumber or reuse existing IDs.
  Use the same identifier for an item in its overview and detail file.
- Verify that requested stories have tasks and a definition of done, that
  identifiers are unique per item, and that new or changed links resolve.
- Summarize the resulting notes and call out assumptions, open decisions, and
  any requested work that remains incomplete.
