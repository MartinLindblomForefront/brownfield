# F-6: Review record completeness

Part of [Register and describe seeds](../README.md). These are draft user
stories, not tracked work items or confirmed requirements.

## US-12: Show missing details

As a collection staff member, I want to see which locally required
identification or provenance details are missing from an accession so that I
know what needs follow-up.

- [ ] T-36: Agree with staff on required details and whether uncertain values count
  as complete.
- [ ] T-37: Evaluate an accession against those rules and show the missing details
  when it is viewed.
- [ ] T-38: Test complete, incomplete, and uncertain records against the agreed rules.

### Definition of done

- The accession view identifies missing required details using the rules agreed
  with staff, including their treatment of uncertain values.
- Complete, incomplete, and uncertain records are covered by passing tests.

## US-13: Find incomplete records

As a collection staff member, I want to find accessions with missing required
information so that I can prioritize improving their records.

- [ ] T-39: Add a way to list accessions with missing required information, using the
  same completeness rules as the individual record.
- [ ] T-40: Show the missing details for each result.
- [ ] T-41: Test that incomplete records appear and disappear from the list as their
  details are completed.

### Definition of done

- Staff can list incomplete accessions and see the missing details for each,
  using the same rules as the individual record.
- A completed record no longer appears in the list, and focused tests pass.
