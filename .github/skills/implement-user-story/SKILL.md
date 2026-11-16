---
name: implement-user-story
description: Implement a developer-supplied user story using only the supplied story, its supplied tasks, and the existing codebase as project context.
---

# Implement a user story

Use this skill when a developer asks you to implement a specific user story.
The limited context is intentional: it simulates working with incomplete
organizational knowledge.

## Context boundary

- Treat the story and any task items, definition of done, acceptance criteria,
  or other context explicitly supplied by the developer as the scope of the
  assignment.
- Inspect the codebase as needed to understand existing behavior and implement
  the assignment. Search and read selectively.
- Do not browse or search `planning/`, follow links into it, or use other
  planning notes to fill gaps. Do not seek external domain information.
- If the developer points to a planning file, read only the explicitly
  identified story and its accompanying task items, definition of done, or
  acceptance criteria. Use a bounded line range where possible. If the story
  is not identifiable without reading unrelated material, ask the developer
  to supply the story text or an exact selection.
- If given only a story identifier without its text or location, ask the
  developer for it rather than searching the planning notes.
- Continue to follow applicable repository instructions and higher-priority
  instructions; this context boundary does not override them.

## Workflow

1. Identify the requested outcome, supplied constraints, task items, and
   definition of done. If no specific story is supplied, ask for one.
2. Inspect relevant existing code and tests. Identify what already exists and
   any missing information needed to implement the story safely.
3. If an ambiguity would materially change behavior, scope, or domain meaning,
   ask the developer rather than inventing a requirement or consulting hidden
   planning material. Otherwise make a small, explicit assumption consistent
   with the supplied context and code.
4. Implement the story and its supplied tasks within that scope, following
   existing patterns. Work in task-sized increments where practical. Do not
   implement neighboring stories or update planning notes unless explicitly
   requested.
5. For each task's changes ready to commit, use the
   [commit-task skill](../commit-task/SKILL.md). Follow its requirement to ask
   the developer which task the commit belongs to, and its checks for scope,
   size, branching, and pushing. Do not assume the whole story belongs in one
   task commit. If changes cannot be separated safely, ask the developer.
6. Check the supplied definition of done and run focused tests or other
   appropriate checks. Report the changes, assumptions, commits, and
   verification results, including any unmet criteria or unresolved blockers.
