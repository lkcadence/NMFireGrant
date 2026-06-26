# Prior-Year Prefill Save — Planning Summary

> **Detailed implementation guide:**
> [`../prior-year-prefill-save-implementation-plan.md`](../prior-year-prefill-save-implementation-plan.md)

**Project:** NMSFM Fire Grant Web Application  
**Document version:** 1.0  
**Date:** June 25, 2026  
**Status:** Planned (not yet implemented)

**Related artifacts:**

- FY walk-back design: [`../FY-Missing-Year-Developer-Guide.md`](../FY-Missing-Year-Developer-Guide.md)
- Prior analysis: [`../Fire-Grant-Prefill-And-Add-Button-Findings.docx`](../Fire-Grant-Prefill-And-Add-Button-Findings.docx)
- Part-of-project hide (conflicts with preserve-on-No): [`part-of-project-conditional-sections-plan.md`](part-of-project-conditional-sections-plan.md)

---

## Overview

Fix FY application data that **loads** from the nearest prior fiscal year but **does not persist** under the current application when the user clicks Save, Next, Previous, or sidebar navigation. Also fix **data loss when answering No** on Apparatus, Communication Equipment, and PPE gate questions.

**Verified (June 2026):** UUID remap and preserve-on-No fixes are **not** in the repo. Walk-back **load** exists; save-side remap does not.

---

## Problems

### 1. GUID / UUID prefill save (primary)

Prefilled **grid/list rows** keep the prior FY `ApplicationId` and child row GUIDs in ViewState. Save helpers write using those stale IDs, so data lands on the wrong application (or updates prior-year rows). Reload of the current FY looks empty; prefill banner appears again.

**Affected sections:** Apparatus, Community Information, Water Availability, Communication Equipment, Hazards/Threats.

### 2. Data wiped when user answers No (secondary)

When **No** is selected on Apparatus, Communication Equipment, or PPE and the user saves or navigates, `SaveForm` clears ViewState lists and JS clears field values. Switching back to **Yes** does not restore data.

This behavior was introduced by the part-of-project conditional sections work (June 2026). Product now requires the opposite: **hide UI on No, but persist section data** so a mistaken No can be undone.

---

## Confirmed decisions

| Topic | Decision |
|-------|----------|
| General Information | **Out of scope** — leave as-is until testers report a problem |
| Training | **Out of scope** — no prior-year prefill; upload issues separate |
| Code changes | **Comment out** old logic; do not delete |
| Edit scope | `NMSFMFireGrantWF/NMSFMFireGrantWF/` and `NMSFM.Services/` only — not `publish/` or `_Backup_*` |
| Phase order | 1 → UUID remap, 2 → preserve on No, 3 → validation navigation, 4 → build/test |

---

## Fix summary (four phases)

| Phase | What | Pages / files |
|-------|------|----------------|
| **1** | Remap child `ApplicationId` + new child GUIDs at prefill load; force parent `ApplicationId` on save in service layer | 5 grid code-behinds + `FGApplicationService.cs` |
| **2** | Comment out wipe-on-No in `SaveForm`; comment out JS clear-on-hide | Apparatus, Communication Equipment, PPE (`.aspx` + `.cs`) |
| **3** | `return isValid && retVal` from `SaveForm` | Five grid sections only (not General Information) |
| **4** | `build.ps1`, manual QA, SQL verification, release deploy | — |

---

## Success criteria

1. Prior-year grid data survives Save / Next / Previous / sidebar under **current** `ApplicationId`.
2. SQL row counts for current FY increase after save; prior FY unchanged.
3. User selects No → saves → selects Yes → grid and section fields return from DB.
4. Invalid sections do not navigate away on failed validation.

---

## Estimated effort

~1–1.5 days including manual QA across five grid sections and three part-of-project pages.
