# F-5: Maintain accession records

Part of [Register and describe seeds](../README.md). These are draft user
stories, not tracked work items or confirmed requirements.

## US-10: Add missing information

As a collection staff member, I want to add information to an existing
accession when it becomes available so that the record becomes more complete.

- [ ] T-30: Provide an update action for the identification and provenance fields on
  an existing accession.
- [ ] T-31: Preserve fields not included in an update and apply the relevant
  validation rules.
- [ ] T-32: Test adding missing details without changing unrelated fields.

### Definition of done

- Staff can add identification or provenance details to an existing accession
  without altering fields omitted from the update.
- Relevant validation is applied, and a focused update test passes.

## US-11: Correct existing information

As a collection staff member, I want to correct an accession's identification
or provenance details so that the catalogue reflects the best available
information.

- [ ] T-33: Allow staff to replace existing identification and provenance values and
  their associated certainty indications.
- [ ] T-34: Return clear errors for invalid updates or nonexistent accessions.
- [ ] T-35: Test corrections, validation failures, and persistence after reload.

### Definition of done

- Staff can correct existing identification or provenance details and their
  associated certainty indications; changes persist after reload.
- Invalid updates and nonexistent accessions produce clear errors, and focused
  tests pass.
