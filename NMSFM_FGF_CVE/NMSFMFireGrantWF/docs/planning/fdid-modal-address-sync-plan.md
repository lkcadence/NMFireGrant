# Manage FDIDs Modal — Address Link + Create (Option B)

> **Detailed implementation guide:**
> [`../fdid-modal-address-sync-implementation-plan.md`](../fdid-modal-address-sync-implementation-plan.md)

**Project:** NMSFM Fire Grant Web Application  
**Document version:** 1.0  
**Date:** June 27, 2026  
**Status:** Planned (not yet implemented)

**Related artifacts:**

- NERIS migration context: [`../neris-id-20-char-implementation-plan.md`](../neris-id-20-char-implementation-plan.md)
- Manage FDIDs filter/sort (prerequisite UI): [`../manage-fdid-list-filter-sort-implementation-plan.md`](../manage-fdid-list-filter-sort-implementation-plan.md)
- General Information error fix: [`../general-information-error-fix-plan.md`](../general-information-error-fix-plan.md)
- Optional SQL bulk backlog: bulk sync plan (conversation artifact)

---

## Overview

After the NERIS migration, **`FG_FDIDs`** was updated (new NERIS IDs and department names) but **`Addresses.AddressCode`** was not synced. The app joins departments by string match:

**`FG_FDIDs.FireDepartment` ↔ `Addresses.AddressCode`** (trim, case-insensitive)

There is no foreign key between the tables. Mismatches break NERIS prefill on General Information, Edit User department dropdown labels, and award/denial/print document headers.

**Primary fix:** extend the existing **Manage FDIDs** modal so admins can **link** an existing fire-department address or **create** a new one with a **full physical address** when no match exists.

**Secondary fix:** stop the false “No NERIS ID found in master list” warning on General Information when a saved NERIS ID already exists.

---

## Confirmed decisions

| Topic | Decision |
|-------|----------|
| Admin entry point | Extend [`ManageFDIDs.aspx`](../NMSFMFireGrantWF/Admin/ManageFDIDs.aspx) modal — no new Admin menu |
| New departments | **Option B** — create new `Addresses` row via `CreateAddressAsync` with **full address** (invoices/legal docs need street, city, county, state, zip) |
| Duplicate names | Use **`AddressId`** in dropdown; display **`FullAddress`** from `v_Addresses2` to disambiguate (Clovis lesson) |
| Schema | **No changes** — do not add `AddressId` to `FG_FDIDs` |
| Auto-select match | **Never** — admin must choose |
| Rollback | Feature branch, split commits, `EnableFdidAddressSync` kill switch, manual data rollback SQL |
| Out of scope v1 | Remittance address rows, `AddressParties` user links, `FG_App_GeneralInfo.DepartmentName` sync |

---

## Two admin paths

### Path A — Link existing address

1. Admin opens NERIS row in Manage FDIDs modal.
2. Picks a ranked match from **`v_Addresses2`** (fire dept type, active only).
3. On save: update **`FG_FDIDs`** (existing) + set **`Addresses.AddressCode`** = Department Name on selected **`AddressId`**.

### Path B — Create new address

1. Admin chooses “Create new fire department address” (or no suitable match).
2. Enters full physical address (street number/name, city, county, state, zip).
3. On save: update **`FG_FDIDs`** + **`CreateAddressAsync`** with `AddressCode` = Department Name.

---

## Why full address is required

Reporting pages ([`GrantAwarded.aspx.cs`](../NMSFMFireGrantWF/Application/Reporting/GrantAwarded.aspx.cs), ApplicationPrint, DenialLetter, NotFunded) build printed addresses from **`v_AddressParties`** / **`v_Addresses2`** — not from `FG_App_GeneralInfo`. A name-only stub would leave blank street lines on award letters and invoices.

---

## Fix summary (implementation phases)

| Phase | What | Primary files |
|-------|------|----------------|
| **0** | Feature branch, kill switch in Web.config, export mismatch baseline | `Web.config` |
| **1** | GI false NERIS warning fix | `GeneralInformation.aspx.cs` |
| **2** | AddressService match + duplicate-check methods | `AddressService.cs`, `IAddressService.cs` |
| **3** | Modal UI: link dropdown + create address panel + JS toggle | `ManageFDIDs.aspx`, `.designer.cs` |
| **4** | Save handler: link + create paths, validation, logging | `ManageFDIDs.aspx.cs` |
| **5** | Build, pilot QA (Clovis), acceptance gate | `build-release.ps1` |

---

## Success criteria

1. Admin links correct duplicate-name address by **FullAddress** → Edit User dropdown + GI department name + NERIS prefill align.
2. Admin creates new dept with full address → row appears in `v_Addresses2`; reports show complete street/city/county block when user is linked to that address.
3. Duplicate **`AddressCode`** among active fire depts is blocked on link and create.
4. Saved application with existing NERIS ID loads GI **without** false master-list warning.
5. Kill switch `EnableFdidAddressSync=false` restores FDID-only save with no address side effects.

---

## Rollback (summary)

| Layer | Action |
|-------|--------|
| **Code** | Revert modal commit; keep GI fix in separate commit if still wanted |
| **Runtime** | Set `EnableFdidAddressSync` to `false` |
| **Data** | Manual — restore `AddressCode` from audit/log; inactivate new rows if unreferenced |

See implementation plan §12 for SQL examples and pilot gate.

---

## Estimated effort

~2–3 days including service methods, modal UI, save handler, GI fix, build, and manual QA on dev DB (pilot 1–2 rows before bulk backlog).
