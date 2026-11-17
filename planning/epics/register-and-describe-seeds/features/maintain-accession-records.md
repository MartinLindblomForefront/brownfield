# F-5: Maintain accession records

Part of [Register and describe seeds](../README.md). These are draft user
stories, not tracked work items or confirmed requirements.

## US-10: Add missing information

As a collection staff member, I want to add information to an existing
accession when it becomes available so that the record becomes more complete.

### Expedient constraints

- Simulated legacy contract: the assigned accession identifier remains stable
  when details are added. No external legacy integration is required.

- [ ] T-30: Provide an update action for the identification and provenance fields on
  an existing accession.
- [ ] T-31: Preserve fields not included in an update and apply the relevant
  validation rules.
- [ ] T-32: Test adding missing details without changing unrelated fields.
  Verify that the assigned accession identifier remains unchanged.

### Frontend implementation

- [ ] T-62: Add or extend an accession edit form to fill missing details using
  the update API without overwriting omitted fields. Keep the identifier
  read-only, show load/save progress and errors, and refresh displayed data
  after a successful update.
- [ ] T-63: Verify adding details and reloading in the browser, including
  unchanged existing values and identifier, plus a failed save retaining input.

### Definition of done

- Staff can add identification or provenance details to an existing accession
  without altering fields omitted from the update.
- Relevant validation is applied, and a focused update test passes.
- The assigned accession identifier remains unchanged.
- Staff can add missing details in the frontend without losing existing data;
  the saved view refreshes and failed saves preserve their input.

## US-11: Correct existing information

As a collection staff member, I want to correct an accession's identification
or provenance details so that the catalogue reflects the best available
information.

### Expedient constraints

- Simulated legacy contract: correcting details must not change the assigned
  accession identifier. No external legacy integration is required.

- [ ] T-33: Allow staff to replace existing identification and provenance values and
  their associated certainty indications.
- [ ] T-34: Return clear errors for invalid updates or nonexistent accessions.
- [ ] T-35: Test corrections, validation failures, and persistence after reload.
  Verify that the assigned accession identifier remains unchanged.

### Frontend implementation

- [ ] T-64: Extend the accession edit form to correct existing values and
  certainty indications. Keep the identifier read-only, surface validation
  and missing-record errors, and refresh the view after a successful update.
- [ ] T-65: Verify corrections and certainty changes in the browser after
  reload, including an unchanged identifier and a failed save retaining edits.

### Definition of done

- Staff can correct existing identification or provenance details and their
  associated certainty indications; changes persist after reload.
- Invalid updates and nonexistent accessions produce clear errors, and focused
  tests pass.
- The assigned accession identifier remains unchanged.
- Staff can correct details from the frontend and see refreshed saved values;
  validation and request failures do not discard edits or show success.
