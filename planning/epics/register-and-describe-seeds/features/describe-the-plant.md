# F-2: Describe the plant

Part of [Register and describe seeds](../README.md). These are draft user
stories, not tracked work items or confirmed requirements.

## US-3: Record plant identification

As a collection staff member, I want to record a plant's scientific name and
other known identification details so that the accession can be understood and
compared with others.

### Expedient constraints

- Store scientific names as free text to avoid introducing reference data in
  this delivery. Do not add a taxonomy lookup or validate names against an
  external authority. Ordinary input validation still applies; stored names
  are not thereby taxonomically verified.

- [ ] T-9: Agree with staff on the plant identification fields and their meanings.
- [ ] T-10: Persist the scientific name and agreed details on an accession and
  expose them when the record is viewed.
- [ ] T-11: Test that known identification details survive a save and reload.
  Include a name not present in any reference catalogue.

### Frontend implementation

- [ ] T-48: Add labeled free-text plant identification inputs and a saved
  details display to the accession UI, wired to the API. Show pending saves
  and errors without suggesting taxonomy verification.
- [ ] T-49: Verify saving and reloading identification in the browser, including
  a free-text name and a failed save that retains entered values.

### Definition of done

- The agreed plant identification details can be saved and viewed on an
  accession after reloading it.
- A focused persistence test passes.
- Free-text names can be recorded without taxonomy lookup or verification.
- Staff can enter and view saved plant identification in the frontend without
  a taxonomy lookup; failed saves remain visible and preserve their input.

## US-4: Record identification uncertainty

As a collection staff member, I want to indicate when identification is
uncertain or unknown so that an unverified name is not mistaken for a confirmed
one.

- [ ] T-12: Agree on identification certainty values and how an unknown name is
  represented.
- [ ] T-13: Allow certainty to be recorded and displayed alongside plant
  identification, including when no name is known.
- [ ] T-14: Test confirmed, uncertain, and unknown identification cases.

### Frontend implementation

- [ ] T-50: Add a labeled certainty control using the agreed values and display
  the saved indication alongside identification, including an unknown name.
  Wire it to the API with save progress and error feedback.
- [ ] T-51: Verify confirmed, uncertain, and unknown identification in the
  browser after saving and reloading, including a failed save.

### Definition of done

- Staff can record and view the agreed certainty indication, including when
  the plant name is unknown.
- Confirmed, uncertain, and unknown cases are distinguishable and covered by
  passing tests.
- The frontend makes the agreed certainty states distinguishable without
  requiring a name for the unknown case and reports save failures.
