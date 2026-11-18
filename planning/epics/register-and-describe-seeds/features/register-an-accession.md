# ~~F-1: Register an accession~~

Part of [Register and describe seeds](../README.md). These are draft user
stories, not tracked work items or confirmed requirements.

## ~~US-14: Establish the development foundation~~

As a developer, I want a minimal application skeleton with repeatable build and
test commands so that I can begin implementing accession registration.

- [x] T-42: Agree with the developer on the initial technology stack and
  prerequisites.
- [x] T-43: Scaffold the smallest runnable application using the agreed stack.
- [x] T-44: Set up a repeatable build and test command with a basic application
  smoke test.
- [x] T-45: Document prerequisites and commands for building, running, and
  testing the application.

### Definition of done

- The application builds and starts using the documented commands.
- The documented test command runs the smoke test successfully.
- Setup instructions identify the prerequisites needed by another developer.

## ~~US-1: Create an accession~~

As a collection staff member, I want to create an accession record with a
unique identifier so that each addition to the collection can be distinguished
from others.

Prerequisite: US-14 establishes the development foundation.

### Expedient constraints

- Simulated legacy contract: other records refer to the assigned accession
  identifier. Once assigned, it must remain stable through subsequent edits.
  Preserve uniqueness; no external legacy integration is required.

- [x] T-1: Define the accession identifier format and how identifiers are assigned.
- [x] T-2: Persist new accession records and enforce identifier uniqueness,
  including concurrent creation.
- [x] T-3: Provide a create action that returns the assigned identifier or a
  meaningful error.
- [x] T-4: Test successful creation and duplicate identifier rejection.

### Definition of done

- Staff can create a persisted accession and receive its unique identifier.
- Duplicate identifiers cannot be saved, including during concurrent creation;
  failures are communicated clearly.
- Focused creation and uniqueness tests pass.

## ~~US-2: Record basic identification~~

As a collection staff member, I want to record basic identifying information
when registering an accession so that colleagues can recognize the material.

- [x] T-5: Agree with staff on the basic fields and which are required at creation.
- [x] T-6: Add those fields to accession creation and persist their values.
- [x] T-7: Validate required fields and expose actionable validation errors.
- [x] T-8: Test creation with valid, missing, and optional identification details.

### Frontend implementation

- [x] T-46: Add a labeled Mantine registration form using the agreed basic
  fields and the accession creation API. Show submission progress, field
  errors, and the assigned identifier on success; retain input on failure.
- [x] T-47: Verify the form in the browser with required and optional fields,
  keyboard submission, successful creation, and a failed request.

### Definition of done

- The agreed basic identification fields can be saved during registration.
- Missing required fields produce actionable errors; optional fields can be
  omitted.
- Focused validation and persistence tests pass.
- Staff can register an accession from the frontend and see its assigned
  identifier; a failed save does not discard their input or show success.
