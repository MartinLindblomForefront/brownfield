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

### Definition of done

- Known collection place and date details can be saved and viewed, including
  incomplete dates in the agreed representation.
- Absent details are supported and invalid date input is rejected.
- Focused tests for these cases pass.

## US-6: Record the source or donor

As a collection staff member, I want to record the source or donor of an
accession so that its history can be traced.

- [ ] T-18: Agree on the source and donor details to retain, including any privacy
  requirements.
- [ ] T-19: Allow source or donor details to be saved and viewed on an accession.
- [ ] T-20: Test that recorded details survive a save and reload.

### Definition of done

- The agreed source or donor details can be saved and viewed after reload in
  accordance with agreed privacy requirements.
- A focused persistence test passes.

## US-7: Document missing or uncertain provenance

As a collection staff member, I want to document missing or uncertain
provenance so that colleagues can distinguish it from confirmed information.

- [ ] T-21: Agree on how to record uncertainty or missing information for each
  provenance detail.
- [ ] T-22: Allow staff to save and view those indications alongside the relevant
  provenance details.
- [ ] T-23: Test confirmed, uncertain, and missing provenance cases.

### Definition of done

- Staff can record and view missing or uncertain indications for the agreed
  provenance details.
- Confirmed, uncertain, and missing cases are distinguishable and covered by
  passing tests.
