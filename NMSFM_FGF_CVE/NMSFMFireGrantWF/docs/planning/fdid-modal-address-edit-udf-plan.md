# Manage FDIDs Modal — Address Edit + Department UDFs (Phase 2)

> **Detailed implementation guide:**
> [`../fdid-modal-address-edit-udf-implementation-plan.md`](../fdid-modal-address-edit-udf-implementation-plan.md)

**Project:** NMSFM Fire Grant Web Application  
**Document version:** 1.0  
**Date:** June 27, 2026  
**Status:** Planned

**Related artifacts:**

- Phase 1 (address link/create): [`fdid-modal-address-sync-plan.md`](fdid-modal-address-sync-plan.md)
- Phase 1 implementation: [`../fdid-modal-address-sync-implementation-plan.md`](../fdid-modal-address-sync-implementation-plan.md)
- Page under change: [`ManageFDIDs.aspx`](../../NMSFMFireGrantWF/Admin/ManageFDIDs.aspx)

---

## Overview

Phase 1 extended the Manage FDIDs modal to **link** or **create** fire-department `Addresses` rows so `AddressCode` aligns with `FG_FDIDs.FireDepartment`. Phase 2 closes three gaps:

1. **Department UDFs not saved** — ISO Rating, Main Stations, Substations, and Admin Buildings live in Codepal `UserDefValues` on the department `AddressId`. General Information reads them but the modal never writes them.
2. **No address edit path** — only `CreateAddressAsync`; existing linked addresses cannot be updated from the modal.
3. **No pre-load on edit** — opening a NERIS row does not populate the associated address or UDF fields.

**Primary fix:** extend the modal with **Create / Edit Address**, UDF inputs, pre-load of the associated address on open, and save of physical address + UDFs on all address paths.

---

## Confirmed decisions

| Topic | Decision |
|-------|----------|
| UDF fields | ISO Rating, Main Stations, Substations, Admin Buildings — saved to `UserDefValues` on department `AddressId` |
| Radio label | **Create / Edit Address** (replaces "Create new address") |
| Associated address | Active fire-dept row where `AddressCode` = `FireDepartment` (trim, case-insensitive) |
| Modal open (edit row) | If associated address exists: **select in Link dropdown** and **prefill Create/Edit panel** (physical + UDFs) |
| No association | Create/Edit panel empty; link dropdown shows ranked candidates |
| Duplicate names (Clovis) | Link dropdown disambiguates by `FullAddress`; selection loads edit panel |
| Link mode on save | Sync `AddressCode`; **also save UDFs** for selected `AddressId` |
| Schema | No database schema changes |
| Kill switch | Reuse `EnableFdidAddressSync` in Web.config |

---

## UDF field GUIDs

| Field | `UserDefFieldId` |
|-------|------------------|
| ISO Rating | `6b8517ef-9483-4b8b-8c95-5b95a6b8f579` |
| Main Stations | `7ad61001-cac8-4f3c-ae4e-32d28393f891` |
| Admin Buildings | `8baa0b86-f1e5-4d84-b4f9-a8219f4b11b8` |
| Substations | `4f34b96d-d944-44aa-9665-d47c55cc025d` |

ISO is the only UDF used in Fire Grant **Total Score**; station counts are reporting-only (snapshotted on General Information save).

---

## Admin workflows

### Open existing NERIS row (associated address)

1. Modal resolves address where `AddressCode` matches Department Name.
2. Link dropdown preselects that `AddressId`.
3. Create/Edit panel shows physical address + UDF values.
4. Admin may edit any field and save.

### Open existing NERIS row (no association)

1. Link dropdown shows ranked candidates.
2. Create/Edit panel is empty (create mode).
3. Admin creates address + UDFs or picks a link candidate.

### Duplicate department names

1. Admin picks correct row from Link dropdown (FullAddress disambiguation).
2. Edit panel loads that address; save updates only that `AddressId`.

---

## Implementation phases

| Phase | What | Primary files |
|-------|------|----------------|
| **A** | Planning + implementation docs | `docs/planning/`, `docs/` |
| **B** | AddressService UDF get/save + association lookup | `AddressService.cs`, `IAddressService.cs` |
| **C** | Modal UI: rename radio, UDF fields | `ManageFDIDs.aspx`, `.designer.cs` |
| **D** | Load: preselect link + populate edit panel | `ManageFDIDs.aspx.cs`, JS |
| **E** | Save: update vs create + UDF on all paths | `ManageFDIDs.aspx.cs` |
| **F** | Build + manual QA | `build.ps1` |

---

## Success criteria

1. New department: create address + UDFs; General Information shows ISO/stations after reload.
2. Existing associated address: modal pre-fills; edits persist to `Addresses` and `UserDefValues`.
3. Clovis duplicate: correct `AddressId` selected via link dropdown; save touches only that row.
4. Link-only save with UDF change persists UDFs without duplicate address.
5. Department rename updates `AddressCode`; reopen still resolves association.
6. Kill switch off hides address/UDF UI; FDID-only save unchanged.

---

## Out of scope

- Syncing in-flight `FG_App_GeneralInfo` snapshots (GI re-reads UDFs from department on load)
- FY Distribution / FPF module
- Remittance addresses or `AddressParties` creation

---

## Rollback

| Layer | Action |
|-------|--------|
| **Code** | Revert Phase 2 commits; Phase 1 remains |
| **Runtime** | `EnableFdidAddressSync=false` |
| **Data** | Restore UDF values from audit; revert address edits manually if needed |
