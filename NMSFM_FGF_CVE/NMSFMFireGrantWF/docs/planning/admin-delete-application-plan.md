# Admin Delete Application — Planning Document

> **Detailed implementation guide:**
> [`../admin-delete-application-implementation-plan.md`](../admin-delete-application-implementation-plan.md)

**Project:** NMSFM Fire Grant Web Application  
**Document version:** 1.0  
**Date:** June 29, 2026  
**Status:** Implemented

**Related artifacts:**

- Admin application list: [`Home.aspx`](../../NMSFMFireGrantWF/Admin/Home.aspx)
- Application service: [`FGApplicationService.cs`](../../../NMSFM.Services/FireGrant/FGApplicationService.cs)
- Main application entity: [`FGApplications.cs`](../../../NMSFM.Data/Codepal Tables/FGApplications.cs)
- Legacy delete confirm pattern: [`ManageLegacyApps.aspx`](../../NMSFMFireGrantWF/Admin/ManageLegacyApps.aspx)

---

## Overview

Add a **Delete** action to the Admin Home application list so internal admins can
permanently remove one or more Fire Grant applications and **all associated
application data** from the database.

Admins select applications using checkboxes in a new **Delete** column (last
column on the grid), then click a **Delete** button next to **Search**. The
button remains visible but is **disabled** until at least one application is
selected. A **confirmation prompt** appears before any data is removed.

This is a **hard delete** — data is physically removed and cannot be recovered
without a database backup restore.

---

## Confirmed decisions

| Topic | Decision |
|-------|----------|
| Target page | Admin Home (`Admin/Home.aspx`) |
| Audience | Internal admins (`Session["Role"] == "Internal"`) |
| Selection UI | Checkbox per row in new **Delete** column (last column) |
| Action button | **Delete** button to the right of **Search** |
| Button state | **Disabled** (not hidden) when no row is checked |
| Confirmation | Browser `confirm()` before postback |
| Delete type | **Hard delete** — `Remove()` from database tables |
| Scope | All `FG_App_*` child data + `FGApplications` parent row per selected application |
| Department record | **Not** deleted — `AddressId` / department remains |
| Reference data | **Not** deleted — help text, priorities, categories, FDIDs, fiscal-year settings |

---

## User flow

```mermaid
sequenceDiagram
  participant Admin
  participant Home as Admin Home
  participant Service as FGApplicationService
  participant DB as Database

  Admin->>Home: Log in as Internal admin
  Home-->>Admin: Application list for fiscal year
  Admin->>Home: Check Delete on one or more rows
  Home->>Home: Delete button becomes enabled
  Admin->>Home: Click Delete
  Home-->>Admin: Confirm permanent deletion?
  alt User cancels
    Admin-->>Home: Cancel
    Home-->>Admin: No changes
  else User confirms
    Admin->>Home: Confirm
    Home->>Service: DeleteFGApplicationsAsync(ids)
    Service->>DB: Hard delete all related rows
    Service-->>Home: Success count
    Home-->>Admin: Success message; grid refreshes
  end
```

---

## UI changes (summary)

### Search row

Add a **Delete** button immediately to the right of the existing **Search**
button on the date-range row. Styled as a destructive action (`btn-danger`).
Starts disabled on page load.

### Application grid

Add a final column:

| Column | Content |
|--------|---------|
| Delete | Checkbox to mark the row for deletion |

Existing columns (View/Edit, Department, County, etc.) are unchanged.

---

## Data impact

For each selected `ApplicationId`, the system removes rows from all Fire Grant
application tables, including (but not limited to):

- Form sections: General Info, Budget, Community, Response History, Water,
  Training, Apparatus, Communication, Hazards, PPE, Equipment Needs, Funding,
  Project Budget, Docs/Signatures, Review
- Child collections: aid districts, water sources, training opportunities,
  apparatus equipment, communication equipment, hazard events, standard PPE/SCBA,
  application equipment, documents, signatures, scores
- Parent record: `FGApplications`

Application documents are stored as `byte[]` in `FG_App_Documents`; no separate
on-disk cleanup is required for application uploads.

---

## Implementation phases

| Phase | What | Primary files |
|-------|------|----------------|
| **A** | Planning + implementation docs | `docs/planning/`, `docs/` |
| **B** | Cascade delete service methods | `IFGApplicationServices.cs`, `FGApplicationService.cs` |
| **C** | Admin Home UI (column, button, script) | `Home.aspx`, `Home.aspx.designer.cs` |
| **D** | Delete click handler + grid refresh | `Home.aspx.cs` |
| **E** | Build + manual QA | `build.ps1` |

---

## Success criteria

1. Internal admin sees a **Delete** column as the last column on the application
   list.
2. **Delete** button appears to the right of **Search** and is disabled when no
   checkbox is selected.
3. Checking one or more rows enables the **Delete** button; unchecking all
   disables it again.
4. Clicking **Delete** shows a confirmation prompt; cancel leaves data unchanged.
5. Confirming permanently removes the selected application(s) and all associated
   application data from the database.
6. The department/address record remains; a new application can be created for
   that department.
7. Admin sees a clear success or error message after the operation.
8. Build passes (`.\build.ps1`).

---

## Out of scope

- Soft delete / inactive flag on applications
- Deleting the department or address record
- Cross-page checkbox persistence in the paged grid (initial release: current page only)
- Audit trail or recycle-bin for deleted applications
- Database schema changes

---

## Risks

| Risk | Mitigation |
|------|------------|
| Irreversible data loss | Confirmation prompt with explicit permanent-delete wording |
| Partial delete on error | Single EF transaction per application; rollback on failure |
| Accidental bulk delete | Confirmation required; button disabled until explicit selection |

---

## Rollback

| Layer | Action |
|-------|--------|
| **Code** | Revert feature commits; redeploy prior build |
| **Data** | Restore affected tables from database backup (no undo in app) |
