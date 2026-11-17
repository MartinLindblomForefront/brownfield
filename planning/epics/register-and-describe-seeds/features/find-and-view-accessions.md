# F-4: Find and view accessions

Part of [Register and describe seeds](../README.md). These are draft user
stories, not tracked work items or confirmed requirements.

## US-8: Search accessions

As a collection staff member, I want to search for accessions by identifier or
plant name so that I can find the material I need.

### Expedient constraints

- Search the recorded free-text plant names. Taxonomy lookup, synonym
  resolution, and external name verification are outside this delivery.

- [ ] T-24: Define matching behavior for identifiers and plant names with staff.
- [ ] T-25: Implement a search action returning matching accessions with their
  identifiers and available plant names.
- [ ] T-26: Test identifier and name searches, including no matches.

### Frontend implementation

- [ ] T-58: Add a labeled search input and API-backed result list showing
  accession identifiers and available names. Use the agreed matching rules
  and distinct loading, no-match, and request-error states.
- [ ] T-59: Verify identifier and free-text name searches in the browser,
  including keyboard submission, no matches, and a failed request.

### Definition of done

- Staff can search by identifier or plant name using the agreed matching rules
  and see matching identifiers and available names.
- No-match results are clear, and focused search tests pass.
- Staff can search from the frontend; loading and request errors are not
  mistaken for an empty result.

## US-9: Browse and view accessions

As a collection staff member, I want to browse accessions and view their
recorded identification and provenance so that I can understand what the
collection contains.

- [ ] T-27: Provide a browsable accession list and a way to open a record.
- [ ] T-28: Display the record's identification and provenance, including
  unknown or uncertain information.
- [ ] T-29: Test browsing, viewing a record, and requesting a nonexistent record.

### Frontend implementation

- [ ] T-60: Implement or extend the React list and detail routes for browsing
  accessions and opening their identification and provenance. Connect to the
  API and distinguish loading, empty collection, not-found, and error states.
- [ ] T-61: Verify list-to-detail navigation and direct detail URLs in the
  browser, including unknown details, an empty list, a missing accession, and
  request failures.

### Definition of done

- Staff can browse accessions and open a record to see its identification and
  provenance, including unknown or uncertain information.
- A nonexistent record is reported clearly, and focused browsing and viewing
  tests pass.
- The frontend supports browsing and opening records, including direct detail
  URLs, with distinct empty, missing-record, and request-error feedback.
