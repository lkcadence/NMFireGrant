# Prior-Year Prefill Save — Detailed Implementation Plan

**Project:** NMSFM Fire Grant Web Application  
**Document version:** 1.0  
**Date:** June 25, 2026  
**Status:** Planned (not yet implemented)  
**Scope:** UUID remap for prior-year grid prefill save; preserve section data when part-of-project = No; block navigation on validation failure for five grid sections.

**Related artifacts:**

- Planning summary: [`docs/planning/prior-year-prefill-save-plan.md`](planning/prior-year-prefill-save-plan.md)
- FY walk-back guide: [`FY-Missing-Year-Developer-Guide.md`](FY-Missing-Year-Developer-Guide.md)
- Findings: [`Fire-Grant-Prefill-And-Add-Button-Findings.docx`](Fire-Grant-Prefill-And-Add-Button-Findings.docx)

---

## 1. Problem statement

### 1.1 UUID prefill save failure

Walk-back **load** (`FindNearestPriorApplicationWithDataAsync` and prior-year getters in `FGApplicationService.cs`) is implemented. When a user opens a section with no current-FY data, child rows from the nearest prior FY are assigned to ViewState **with prior FY `ApplicationId` and child PK GUIDs unchanged**.

On Save / Next / Previous / sidebar, `SaveForm` sets the section header `model.ApplicationId` from `hfApplicationId` (current FY), but child collections pass through unchanged. Private save helpers (e.g. `SaveApparatusEquipment`) look up rows by `(model.ApplicationId, model.ApparatusId)` from each child row — still pointing at the prior FY — so writes miss the current application.

**User-visible symptom:** “Information Loaded from Previous Application” → Save → return → banner and empty form again.

### 1.2 Data loss on part-of-project = No

Commit `3382b39` and [`part-of-project-conditional-sections-implementation-plan.md`](part-of-project-conditional-sections-implementation-plan.md) implemented hide-on-No **with field/list clearing**. Product now requires:

- **No** = gate answer only (not part of project).
- Section data **retained** in DB under current FY when user saves with No selected.
- UI still hidden until user selects Yes again.

### 1.3 False-success navigation

Five grid sections return `retVal` from the DB call even when `isValid == false`, allowing Next / Previous / sidebar to navigate away.

---

## 2. Out of scope

| Item | Reason |
|------|--------|
| General Information | Leave as-is per product decision |
| Training | No prior-year prefill; separate upload issues |
| Add-button / modal / jQuery | Separate track |
| Expanding `*Only=true` partial display on prior-year load | Not required for save fix |
| `publish/` or `_Backup_*` folders | Build output / archive only |
| DB schema changes | None required |

---

## 3. Implementation rules

1. **Do not delete code.** Comment out replaced logic with a short note (e.g. `// Disabled: preserve section data when PartOfProject = No`).
2. Edit source under `NMSFMFireGrantWF/NMSFMFireGrantWF/` and `NMSFM.Services/` only.
3. Implement phases **in order** (1 → 2 → 3 → 4). Phase 2 depends on Phase 1 for prefilled rows to persist to current FY.
4. Run `.\build.ps1` from `NMSFM_FGF_CVE/NMSFMFireGrantWF/` after all code changes.

---

## 4. Phase 1 — UUID remap

### 4.1 Goal

Ensure every child row saved for the current application uses:

- `ApplicationId` = current FY application GUID
- A **new** child primary-key GUID (so lookups cannot match prior-year rows)

### 4.2 Primary fix — prefill load (UI layer)

After prior-year fetch succeeds and **before** assigning collections to ViewState, remap each child row.

| Page | Prior-year branch | ViewState key | PK property | Service getter |
|------|-------------------|---------------|-------------|----------------|
| Apparatus | `Page_Load` ~L165–181 | `dtApparatusEquipment` | `ApparatusId` | `GetPriorFGApplicationApparatusAsync` |
| CommunityInfo | ~L130–146 | `dtAidDistricts` | `AidDistrictId` | `GetFGApplicationPriorYearCommunityInfoAsync` |
| WaterAvailability | ~L126–142 | `dtWaterSources` | `WaterSourceId` | `GetFGApplicationPriorYearWaterAvailabilityAsync` |
| CommunicationEquipment | ~L162–178 | `dtCommunicationEquipment` | `CommunicationEquipmentId` | `GetFGApplicationPriorYearCommunicationAsync` |
| HazardsThreats | ~L139–155 | `dtHazardsThreats` | `HazardId` | `GetFGApplicationPriorYearHazardsThreatsAsync` |

**Suggested shared helper** (new file — propose path before implementation):

`NMSFMFireGrantWF/Application/PrefillChildRowRemap.cs`

```csharp
public static class PrefillChildRowRemap
{
  public static void RemapApparatusEquipment(
    IList<FG_App_ApparatusEquipment> rows, Guid currentApplicationId)
  {
    if (rows == null) { return; }
    foreach (var row in rows)
    {
      row.ApplicationId = currentApplicationId;
      row.ApparatusId = Guid.NewGuid();
    }
  }
  // Parallel methods for AidDistricts, WaterSources, CommunicationEquipment, HazardThreatEvents
}
```

Call from each page’s prior-year `Page_Load` branch immediately after getter returns data, using `appIdGuid` from `hfApplicationId` / session.

**Alternative (minimal files):** private static methods in each code-behind if a new file is not approved.

**Do not remap** when loading **current** FY saved data (first branch in `Page_Load` — already correct IDs).

### 4.3 Defense in depth — service layer

**File:** `NMSFM.Services/FireGrant/FGApplicationService.cs`

Before each child `foreach` in both `isNew` and update branches, set parent `ApplicationId`:

| Parent method | Child collection | Private helper |
|---------------|------------------|----------------|
| `SaveApparatusAsync` | `model.ApparatusEquipment` | `SaveApparatusEquipment` |
| `SaveCommunityInformationAsync` | `model.AidDistricts` | `SaveAidDistrict` |
| `SaveWaterAvailabilityAsync` | `model.WaterSources` | `SaveWaterSource` |
| `SaveCommunicationAsync` | `model.CommunicationEquipment` | `SaveCommunicationEquipment` |
| `SaveHazardThreatsAsync` | `model.HazardsThreats` | `SaveHazardThreatEvents` |

**Pattern** (both foreach loops in each parent method):

```csharp
foreach (FG_App_ApparatusEquipment apparatusEquipment in model.ApparatusEquipment)
{
  apparatusEquipment.ApplicationId = model.ApplicationId;
  await SaveApparatusEquipment(apparatusEquipment);
}
```

**Optional hardening:** add `Guid targetApplicationId` parameter to each private child helper; use it for lookup/insert instead of `model.ApplicationId`.

**Verification note:** Repo search (June 2026) found **no** existing `item.ApplicationId = model.ApplicationId` or remap helper — this phase is net-new.

### 4.4 Data flow (after fix)

```mermaid
flowchart LR
  load [Prior FY rows from DB]
  remap [Remap ApplicationId + new child GUIDs]
  viewstate [ViewState current FY identity]
  saveHeader [SaveForm header = current FY]
  childSave [Service forces parent ApplicationId]
  currentDb [Rows under current ApplicationId]
  reload [Reload shows saved data]

  load --> remap --> viewstate --> saveHeader --> childSave --> currentDb --> reload
```

---

## 5. Phase 2 — Preserve data on No

### 5.1 Goal

When gate answer is **No**, still persist grids and section field values under current FY. Hide UI only; skip child validation when No.

### 5.2 Server — comment out wipe blocks

**Apparatus.aspx.cs** — `SaveForm()` ~L562–574:

Comment out block where `apparatusPart == 2` clears pump/hose fields, sets `apparatusEquipment = new List<>()`, and updates ViewState.

Keep:

- `apparatusPart == 1` validation (list or documents required).
- Saving `model.ApparatusPartOfProject = apparatusPart` including value `2` (No).

**CommunicationEquipment.aspx.cs** — `SaveForm()` ~L596–623:

Comment out block where `communicationPart == 2` clears radios, counts, text fields, sets empty equipment list.

**PPE.aspx.cs** — `SaveForm()` ~L559–565 and ~L600–603:

Comment out blocks where `ppePartOfProject == 2` or `scbaPartOfProject == 2` clear ViewState lists.

### 5.3 Client — comment out clear-on-hide

**Apparatus.aspx** — `showApparatusDetails()`:

- Keep `fadeOut` on No.
- Comment out `clearApparatusDetailFields()` call in the `else` branch (~L445).

**CommunicationEquipment.aspx** — `showCommunications()`:

- Comment out `clearCommunicationsFields()` call (~L395).

**PPE.aspx**:

- `showPPE()` — comment out `clearPPEFields()` (~L512).
- `showSCBA()` — comment out `clearNote2Id()` on hide (~L524) if it clears grid-related state; keep modal-only clear behavior if needed for Add flow.

Leave helper functions defined for modal / Add button use.

### 5.4 Expected behavior after Phase 2

| Gate | UI | Validation | DB |
|------|-----|------------|-----|
| No | Hidden | Gate only; no list-required | Child rows + fields saved |
| Yes | Visible | Existing rules | Full data |

Toggle No → Save → Yes on return: data reloads from current FY record.

---

## 6. Phase 3 — Block navigation on validation failure

Change final `SaveForm` return in these files only:

```csharp
return isValid && retVal;
```

| File | Approx. line |
|------|----------------|
| `Application/Apparatus.aspx.cs` | ~613 |
| `Application/CommunityInfo.aspx.cs` | ~438 |
| `Application/WaterAvailability.aspx.cs` | ~410 |
| `Application/CommunicationEquipment.aspx.cs` | ~669 |
| `Application/HazardsThreats.aspx.cs` | ~388 |

**Do not change** `GeneralInformation.aspx.cs`.

Comment out prior `return retVal;` line; do not delete.

---

## 7. Phase 4 — Build and test

### 7.1 Build

From `NMSFM_FGF_CVE/NMSFMFireGrantWF/`:

```powershell
.\build.ps1
```

Fix compile errors before QA. Deploy with `build-release.ps1` when ready for tester environment.

### 7.2 Test setup

- Department with **saved FY2025** data in grid sections (minimum: Apparatus + Community Info).
- **Empty or missing FY2026** section data.
- New **FY2027** application via Instructions → Accept.
- FY2027 `FGApplicationSettings` window open.

### 7.3 Test cases — UUID prefill save

| # | Steps | Expected |
|---|--------|----------|
| 1 | Open Apparatus — prior-year grid visible | Info banner |
| 2 | Save, then reload Apparatus | Saved FY2027 data; no prefill banner |
| 3 | Repeat Save via **Next**, **Previous**, sidebar | Same |
| 4 | SQL: `FG_App_ApparatusEquipment` for FY2027 `ApplicationId` | Count > 0 |
| 5 | SQL: same tables for FY2025 `ApplicationId` | Unchanged vs before test |
| 6 | Repeat for Community, Water, Communication, Hazards | Child rows under FY2027 |

```sql
DECLARE @FY2027AppId uniqueidentifier = '...';

SELECT COUNT(*) AS ApparatusEquipment
FROM FG_App_ApparatusEquipment WHERE ApplicationId = @FY2027AppId;

SELECT COUNT(*) AS AidDistricts
FROM FG_App_AidDistricts WHERE ApplicationId = @FY2027AppId;

SELECT COUNT(*) AS WaterSources
FROM FG_App_WaterSources WHERE ApplicationId = @FY2027AppId;

SELECT COUNT(*) AS CommEquipment
FROM FG_App_CommunicationEquipment WHERE ApplicationId = @FY2027AppId;

SELECT COUNT(*) AS HazardEvents
FROM FG_App_HazardThreatEvents WHERE ApplicationId = @FY2027AppId;
```

### 7.4 Test cases — preserve on No

| # | Steps | Expected |
|---|--------|----------|
| 1 | Apparatus: prefilled grid, select **Yes** then **No** | Section hides |
| 2 | Save or navigate away and back | — |
| 3 | Select **Yes** | Grid and fields return |
| 4 | SQL: equipment count > 0 while `ApparatusPartOfProject = 2` | Data preserved |
| 5 | Repeat for Communication Equipment and PPE | Same pattern |

### 7.5 Test cases — validation navigation

| # | Steps | Expected |
|---|--------|----------|
| 1 | Open Community Info with empty required scalars, click **Next** | Stay on page; error shown |
| 2 | Fix errors, Save | Navigate succeeds |

---

## 8. File change checklist

### New (optional, pending approval)

- [ ] `NMSFMFireGrantWF/Application/PrefillChildRowRemap.cs` — shared remap helper

### Modified — service

- [ ] `NMSFM.Services/FireGrant/FGApplicationService.cs` — five parent `Save*Async` child loops

### Modified — UUID remap at load

- [ ] `NMSFMFireGrantWF/Application/Apparatus.aspx.cs`
- [ ] `NMSFMFireGrantWF/Application/CommunityInfo.aspx.cs`
- [ ] `NMSFMFireGrantWF/Application/WaterAvailability.aspx.cs`
- [ ] `NMSFMFireGrantWF/Application/CommunicationEquipment.aspx.cs`
- [ ] `NMSFMFireGrantWF/Application/HazardsThreats.aspx.cs`

### Modified — preserve on No

- [ ] `NMSFMFireGrantWF/Application/Apparatus.aspx.cs`
- [ ] `NMSFMFireGrantWF/Application/Apparatus.aspx`
- [ ] `NMSFMFireGrantWF/Application/CommunicationEquipment.aspx.cs`
- [ ] `NMSFMFireGrantWF/Application/CommunicationEquipment.aspx`
- [ ] `NMSFMFireGrantWF/Application/PPE.aspx.cs`
- [ ] `NMSFMFireGrantWF/Application/PPE.aspx`

### Modified — validation return

- [ ] Five grid code-behinds listed in §6 (overlap with above)

---

## 9. Rollback

All behavioral changes are comment-based. Rollback = uncomment original blocks and comment out new lines. No migration or schema rollback.

---

## 10. Estimated effort

| Phase | Hours |
|-------|-------|
| 1 — UUID remap | 4–6 |
| 2 — Preserve on No | 2–3 |
| 3 — Validation return | 0.5–1 |
| 4 — Build + QA | 2–3 |
| **Total** | **~8–13 (1–1.5 days)** |

---

## 11. Document history

| Version | Date | Notes |
|---------|------|-------|
| 1.0 | 2026-06-25 | Initial plan; UUID remap verified absent from repo; GI out of scope |
