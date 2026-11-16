# F-2: Describe the plant

Part of [Register and describe seeds](../README.md). These are draft user
stories, not tracked work items or confirmed requirements.

## US-3: Record plant identification

As a collection staff member, I want to record a plant's scientific name and
other known identification details so that the accession can be understood and
compared with others.

- [ ] T-9: Agree with staff on the plant identification fields and their meanings.
- [ ] T-10: Persist the scientific name and agreed details on an accession and
  expose them when the record is viewed.
- [ ] T-11: Test that known identification details survive a save and reload.

### Definition of done

- The agreed plant identification details can be saved and viewed on an
  accession after reloading it.
- A focused persistence test passes.

## US-4: Record identification uncertainty

As a collection staff member, I want to indicate when identification is
uncertain or unknown so that an unverified name is not mistaken for a confirmed
one.

- [ ] T-12: Agree on identification certainty values and how an unknown name is
  represented.
- [ ] T-13: Allow certainty to be recorded and displayed alongside plant
  identification, including when no name is known.
- [ ] T-14: Test confirmed, uncertain, and unknown identification cases.

### Definition of done

- Staff can record and view the agreed certainty indication, including when
  the plant name is unknown.
- Confirmed, uncertain, and unknown cases are distinguishable and covered by
  passing tests.
