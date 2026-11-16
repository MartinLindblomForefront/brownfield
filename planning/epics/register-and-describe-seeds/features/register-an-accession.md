# F-1: Register an accession

Part of [Register and describe seeds](../README.md). These are draft user
stories, not tracked work items or confirmed requirements.

## US-14: Establish the development foundation

As a developer, I want a minimal application skeleton with repeatable build and
test commands so that I can begin implementing accession registration.

- [ ] T-42: Agree with the developer on the initial technology stack and
  prerequisites.
- [ ] T-43: Scaffold the smallest runnable application using the agreed stack.
- [ ] T-44: Set up a repeatable build and test command with a basic application
  smoke test.
- [ ] T-45: Document prerequisites and commands for building, running, and
  testing the application.

### Definition of done

- The application builds and starts using the documented commands.
- The documented test command runs the smoke test successfully.
- Setup instructions identify the prerequisites needed by another developer.

## US-1: Create an accession

As a collection staff member, I want to create an accession record with a
unique identifier so that each addition to the collection can be distinguished
from others.

Prerequisite: US-14 establishes the development foundation.

- [ ] T-1: Define the accession identifier format and how identifiers are assigned.
- [ ] T-2: Persist new accession records and enforce identifier uniqueness,
  including concurrent creation.
- [ ] T-3: Provide a create action that returns the assigned identifier or a
  meaningful error.
- [ ] T-4: Test successful creation and duplicate identifier rejection.

### Definition of done

- Staff can create a persisted accession and receive its unique identifier.
- Duplicate identifiers cannot be saved, including during concurrent creation;
  failures are communicated clearly.
- Focused creation and uniqueness tests pass.

## US-2: Record basic identification

As a collection staff member, I want to record basic identifying information
when registering an accession so that colleagues can recognize the material.

- [ ] T-5: Agree with staff on the basic fields and which are required at creation.
- [ ] T-6: Add those fields to accession creation and persist their values.
- [ ] T-7: Validate required fields and expose actionable validation errors.
- [ ] T-8: Test creation with valid, missing, and optional identification details.

### Definition of done

- The agreed basic identification fields can be saved during registration.
- Missing required fields produce actionable errors; optional fields can be
  omitted.
- Focused validation and persistence tests pass.
