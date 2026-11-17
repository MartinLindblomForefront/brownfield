# F-3: Record provenance

Part of [Register and describe seeds](../README.md). These are draft user
stories, not tracked work items or confirmed requirements.

## US-5: Record collection details

As a collection staff member, I want to record the known place and date of
collection so that the origin of an accession can be understood.

- [ ] T-15: Agree on the collection place and date fields, including how to represent
  incomplete dates.
- [ ] T-16: Allow known collection details to be saved and viewed on an accession.
- [ ] T-17: Test saved details, absent details, and invalid date input.

### Frontend implementation

- [ ] T-52: Add labeled collection place and date inputs and a saved details
  display, using the agreed incomplete-date representation rather than forcing
  a full date. Connect to the API with save progress and validation feedback.
- [ ] T-53: Verify known, absent, and incomplete collection details in the
  browser after reload, plus invalid date input and a failed save.

### Definition of done

- Known collection place and date details can be saved and viewed, including
  incomplete dates in the agreed representation.
- Absent details are supported and invalid date input is rejected.
- Focused tests for these cases pass.
- Staff can record and view collection details in the frontend without
  inventing date components; invalid input and failed saves are reported.

## US-6: Record the source or donor

As a collection staff member, I want to record the source or donor of an
accession so that its history can be traced.

### Expedient constraints

- Record donor-supplied provenance as an unverified claim to avoid introducing
  an external verification process in this delivery. Save and display that
  status with the details; successful input validation is not verification
  of their authenticity.
- Validate external input and use synthetic donor details in local training.
  No donor integration or external lookup is required.

- [ ] T-18: Agree on the source and donor details to retain, including any privacy
  requirements.
- [ ] T-19: Allow source or donor details and their unverified status to be
  saved and viewed on an accession.
- [ ] T-20: Test that recorded details and their unverified status survive a
  save and reload.

### Frontend implementation

- [ ] T-54: Add labeled source or donor fields and a saved details display
  following the agreed privacy requirements. Connect to the API and visibly
  show the unverified status, save progress, and errors.
- [ ] T-55: Verify the donor flow using synthetic details in the browser,
  including unverified status after reload and input retained on save failure.

### Definition of done

- The agreed source or donor details can be saved and viewed after reload in
  accordance with agreed privacy requirements.
- A focused persistence test passes.
- Donor-supplied details remain visibly unverified after saving and reloading.
- Staff can enter and view permitted source or donor details in the frontend;
  their unverified status is visible and failed saves are not shown as success.

## US-7: Document missing or uncertain provenance

As a collection staff member, I want to document missing or uncertain
provenance so that colleagues can distinguish it from confirmed information.

### Expedient constraints

- Donor-supplied provenance is initially unverified. Keep that status distinct
  from missing information and from confirmed information; no external
  verification workflow is required in this delivery.

- [ ] T-21: Agree on how to record uncertainty or missing information for each
  provenance detail.
- [ ] T-22: Allow staff to save and view those indications alongside the relevant
  provenance details.
- [ ] T-23: Test confirmed, uncertain, and missing provenance cases.

### Frontend implementation

- [ ] T-56: Add controls for the agreed missing or uncertain provenance
  indications and display them beside their details. Preserve a distinct
  unverified donor status without adding an external verification workflow;
  show save progress and errors.
- [ ] T-57: Verify the agreed provenance states in the browser after saving
  and reloading, including missing details, unverified claims, and failed saves.

### Definition of done

- Staff can record and view missing or uncertain indications for the agreed
  provenance details.
- Confirmed, uncertain, and missing cases are distinguishable and covered by
  passing tests.
- The frontend distinguishes the agreed provenance states, including
  unverified donor claims, and preserves input when saving fails.
