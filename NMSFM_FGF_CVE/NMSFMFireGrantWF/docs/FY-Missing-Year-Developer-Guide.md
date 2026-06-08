# Missing Fiscal Year — Developer Implementation Guide

**Project:** NMSFM Fire Grant Web Application  
**Plan version:** 3.0 (UI prefill walk-back)  
**Status:** Not implemented — ready for development  
**Audience:** Developer using Visual Studio (may use AI for assistance)  
**Full spec:** `fiscal-year-baseline-copy-implementation-plan.md` (same folder)

---

## 1. Summary for your coworker

We have no FY2026 application data in the database. Departments will start **FY2027** applications using **FY2025** as the source of prior information.

**The fix (v3.0):** Keep the workflow the app already uses — when a user opens a section, the form prefills from a prior year — but change *which* prior year is used. Instead of "always FY minus 1" or "newest application even if empty," walk backward until we find the **nearest prior fiscal year that actually has data** for that section (FY2025 when FY2026 is missing or empty).

**We are NOT** copying rows into the database when the application is created. The user still must **Save** each section (same as today). Estimated effort: **1–2 days**.

---

## 2. The problem in one example

| Situation | Today (buggy) | After fix (v3.0) |
|-----------|---------------|------------------|
| New FY2027 app; FY2026 missing; FY2025 has saved apparatus + firefighter counts | Apparatus may show FY2025 in UI; **General Information does not** (only looks at FY2026) | Both (and 4 other sections) prefill from **FY2025** when opened |
| Empty FY2026 application shell exists | Apparatus may show **nothing** (picks empty FY2026 first) | Walk-back skips empty FY2026 → uses FY2025 |

---

## 3. What we decided NOT to do (v2.0 — superseded)

An earlier plan copied all application sections into the database on `CreateNewApplication`. We chose **v3.0** instead because it:

- Matches how the app already behaves (prefill on page open)
- Touches fewer files (~1–2 days vs 3–4 days)
- Does not require new DB write logic for 13 tables

**Do not implement v2.0** (no copy orchestrator in `CreateNewApplication`) unless product explicitly revisits that decision.

---

## 4. Sections in scope (6 areas only)

Only sections that **already prefill** from a prior year get the walk-back fix:

| # | Sidebar menu | Code-behind | Service method (today) |
|---|--------------|-------------|------------------------|
| 1 | General Information | `GeneralInformation.aspx.cs` | Inline `FiscalYear - 1` in `LoadDepartment` |
| 2 | Apparatus | `Apparatus.aspx.cs` | `GetPriorFGApplicationApparatusAsync` |
| 3 | Community Information | `CommunityInfo.aspx.cs` | `GetFGApplicationPriorYearCommunityInfoAsync` |
| 4 | Water Availability | `WaterAvailability.aspx.cs` | `GetFGApplicationPriorYearWaterAvailabilityAsync` |
| 5 | Communication Equipment | `CommunicationEquipment.aspx.cs` | `GetFGApplicationPriorYearCommunicationAsync` |
| 6 | Hazards/Threats | `HazardsThreats.aspx.cs` | `GetFGApplicationPriorYearHazardsThreatsAsync` |

**Out of scope:** Budget, Response History, Training, PPE, Equipment Needs, Funding Justification, Project Budget — no prefill today; leave unchanged. Also: documents, signatures, review, scores.

---

## 5. Implementation — what to build

### 5.1 One shared helper (core of the fix)

**File:** `NMSFM.Services/FireGrant/FGApplicationService.cs`

```csharp
private async Task<FGApplications> FindNearestPriorApplicationWithDataAsync(
  Guid addressId,
  short currentFiscalYear,
  Guid currentApplicationId,
  Func<Guid, Task<bool>> sectionHasDataAsync)
```

**Algorithm:**

1. Query `FGApplications` for this `addressId`, `FiscalYear < currentFiscalYear`, order by `FiscalYear DESC`.
2. For each candidate, call `sectionHasDataAsync(applicationId)`.
3. Return the first app with data; else `null`.

**Section predicates** (what "has data" means):

| Section | Predicate |
|---------|-----------|
| General Info | `FG_App_GeneralInfo` row exists |
| Apparatus | Any `FG_App_ApparatusEquipment` OR `FG_App_Apparatus` row |
| Community | `FG_App_CommunityInfo` row exists |
| Water | `FG_App_WaterAvailability` row exists |
| Communication | `FG_App_Communication` row exists |
| Hazards | `FG_App_HazardsThreats` row exists |

### 5.2 Refactor five existing methods

Each currently does `apps[0]` after `OrderByDescending(FiscalYear)`. Replace with the shared helper + section predicate, then load the same DTOs as today. **No new DB writes.**

### 5.3 Fix General Information page

**File:** `NMSFMFireGrantWF/Application/GeneralInformation.aspx.cs`

Replace in `LoadDepartment`:

```csharp
int fYear = Convert.ToInt16(Session["FiscalYear"].ToString()) - 1;
lastYearApp = fgAppService.GetFGApplication(addId, sYear);
```

With a service call that uses the same walk-back (e.g. `GetNearestPriorApplicationGeneralInfoAsync`). Keep the **same partial field prefill** (chief, phone, firefighter counts) — do not expand scope.

### 5.4 Do NOT change

- `CreateNewApplication` — shell only
- `Instructions.aspx.cs`
- Budget / Training / PPE / etc. pages

---

## 6. Visual Studio workflow

1. **Open solution:** `NMSFMFireGrantWF.sln` under `NMSFM_FGF_CVE/NMSFMFireGrantWF/`
2. **Primary projects:** `NMSFM.Services` (service layer), `NMSFMFireGrantWF` (web UI)
3. **Start here:** `FGApplicationService.cs` — search for `GetPriorFGApplicationApparatusAsync` (added Dec 2023)
4. **Build:** Rebuild solution; fix compile errors before testing
5. **Run:** IIS Express / local IIS; test DB with department that has FY2025 data and no FY2026
6. **Test path:** User Home → department → FY2027 → Instructions Accept → General Information and Apparatus

---

## 7. Testing checklist

- [ ] FY2025 app has saved General Info + Apparatus (ideally Community, Water, Comm, Hazards too)
- [ ] FY2026 missing OR empty shell — no section data
- [ ] Create FY2027 application via Instructions → Accept
- [ ] Open General Information — FY2025 chief / firefighter fields appear
- [ ] Open Apparatus — FY2025 equipment grid appears
- [ ] Before Save: SQL count on FY2027 `ApplicationId` = 0 rows in prefilled tables
- [ ] Save Apparatus — SQL count > 0 for FY2027
- [ ] Budget / Training still empty (unchanged)

**Sample SQL:**

```sql
SELECT FiscalYear, ApplicationId FROM FGApplications
WHERE AddressId = @DeptId AND FiscalYear < 2027
ORDER BY FiscalYear DESC;

SELECT COUNT(*) FROM FG_App_ApparatusEquipment
WHERE ApplicationId = @FY2027ApplicationId;
```

---

## 8. Using AI effectively (Cursor / Copilot)

**Good prompts for this task:**

- "In FGApplicationService, refactor GetPriorFGApplicationApparatusAsync to walk back to the nearest prior FY where FG_App_ApparatusEquipment has rows, using a shared helper."
- "Replace FiscalYear minus 1 in GeneralInformation LoadDepartment with the same walk-back pattern as apparatus."
- "Do not modify CreateNewApplication; UI prefill only."

**Verify AI output against this doc:**

- Must use walk-back, not hard-coded 2025
- Must not add DB insert on application create
- Must not add prefill to Budget/Training/PPE unless explicitly requested
- Match existing code style: 2-space indent, semicolons, existing logging patterns

**Reference for AI:** @-mention `fiscal-year-baseline-copy-implementation-plan.md` and the five `GetPrior*` methods.

---

## 9. User-facing behavior (after deploy)

1. Department accepts Instructions → FY2027 application shell created (unchanged).
2. User opens a section → if FY2027 has no saved data, form shows FY2025 values (when FY2026 empty/missing).
3. User reviews, edits, clicks Save (or navigates via sidebar, which triggers SaveForm on current page).
4. Data is stored under FY2027 `ApplicationId`.

Optional UX: show "Information loaded from prior fiscal year (FY2025). Please verify data is current."

---

## 10. Implementation checklist (developer)

- [ ] Add `FindNearestPriorApplicationWithDataAsync`
- [ ] Refactor `GetPriorFGApplicationApparatusAsync`
- [ ] Refactor four `GetFGApplicationPriorYear*Async` methods
- [ ] Fix `GeneralInformation.aspx.cs` LoadDepartment
- [ ] Rebuild; manual test six sections
- [ ] Confirm no DB rows until Save

---

## 11. Questions / escalation

If product asks to prefill Budget/Training or copy everything on create, that is a **scope change** (back toward v2.0). Discuss before implementing.

Background conversation export: `docs/Fire-Grant-FY-Rollover-Conversation.docx`

---

*Handoff document — Plan v3.0 — June 2026*
