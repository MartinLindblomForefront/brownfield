---
name: commit-task
description: Commit and push a small, task-scoped change on a branch created from main.
---

# Commit task work

Use this skill when asked to commit task work. Follow applicable repository
and higher-priority instructions throughout.

## Identify the task

Always ask the developer which task the commit belongs to, even if a task
appears to be implied by the conversation. Obtain its `T-` identifier and
enough of its description or scope to assess the changes. If the task is
unclear, ask for clarification; do not search planning notes to fill the gap
unless the developer explicitly permits it.

Use the lowercase task identifier as the branch name: for example, `T-42`
becomes `t-42`. Do not guess or silently alter an invalid identifier.

## Inspect and prepare

1. Inspect the current branch, working tree, index, and relevant diffs,
   including untracked files. Understand what each proposed change does before
   including it. Do not include secrets or unrelated changes.
2. Check local `main` and fetch the remote `main` if a remote is configured.
   Create the task branch from an up-to-date `main`. If local and remote
   `main` differ, or the branch already exists and its relationship to `main`
   is unclear, stop and ask how to proceed. Do not reset or rewrite either
   branch.
3. Switch branches only if Git can do so without losing or overwriting work.
   Preserve unrelated working-tree and index changes. If existing staged changes
   do not belong in this commit, ask before changing their staging state.
   If safe isolation is not possible, stop and explain the blocker.

## Commit and push

1. Stage only changes reasonably required for the selected task, including
   directly related tests and documentation. Inspect the staged diff and
   confirm no unrelated changes are included. If task and unrelated edits are
   intertwined, ask the developer how to separate them.
2. Keep each commit at or below **200 changed lines** (additions plus deletions)
   and **10 changed files**, including tests and documentation. Do not include
   binary files or generated bulk changes without explicit approval. If the
   work exceeds either limit, split it into small, coherent commits that each
   stay within scope; run appropriate checks for each where feasible. If it
   cannot be split safely, stop and ask rather than silently exceeding a limit.
3. Run focused checks appropriate to the staged work. Report failures and do
   not present a failed check as a successful verification.
4. Commit with a descriptive message and any trailers required by applicable
   instructions. Push the task branch to the configured remote, not to
   `main`. Do not force-push or overwrite remote work.
5. Verify the commit and remote branch, then report the task, branch, commit,
   checks, and any remaining uncommitted changes or blockers.
