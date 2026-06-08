# NERIS ID 20-Character Update — Implementation Plan

**Project:** NMSFM Fire Grant Web Application  
**Document version:** 1.0  
**Date:** June 6, 2026  
**Status:** Implemented (June 6, 2026)  
**Scope:** Expand NERIS ID input from legacy 5-character NFIRS limits to 20 characters; enforce uppercase alpha on admin and registration; replace int-based master-list lookup with string-based lookup. Superseded code is **commented out**, not deleted.

**Related artifacts:**

- Cursor plan: `neris_id_20-char_update_02f7bb57.plan.md`
- Entity schema: [`FG_FDIDs.cs`](../../NMSFM.Data/Codepal%20Tables/FG_FDIDs.cs), [`FG_App_GeneralInfo.cs`](../../NMSFM.Data/Codepal%20Tables/FG_App_GeneralInfo.cs)
- Phase 1 (separate): General Information master-list override for FY rollover — not covered here

---

## 1. Problem statement

State-issued NERIS IDs may exceed the legacy 5-digit NFIRS FDID format and may include alphabetic characters. The application currently blocks longer IDs in several places:

| Location | Current constraint |
|----------|-------------------|
| Manage FD ID's modal (`ManageFDIDs.aspx`) | `MaxLength="5"` on `txtFDID` |
| Registration (`Register.aspx`) | `RadNumericTextBox`, `MaxLength="5"`, numeric-only |
| General Information (`GeneralInformation.aspx`) | No `MaxLength` (unlimited in UI) |
| `IsFDIDValid(int)` in `FGService.cs` | `Convert.ToInt32`, 4-digit zero-padding to 5 |

The **database entity model** does not impose a 5-character limit:

- `FG_FDIDs.FDID` — `string` primary key, no `[StringLength]`
- `FG_App_GeneralInfo.NFIRSID` — `string`, no `[StringLength]` (`NERISID` is a `[NotMapped]` alias)

The 5-character limit is **UI and legacy validation logic only**.

**Goal:** Allow NERIS IDs up to **20 characters** (numeric and alphanumeric), normalize alpha to uppercase on entry, and fix registration validation so it looks up the master list by string instead of by integer.

---

## 2. Current implementation status

| Item | Status |
|------|--------|
| Manage FD ID's modal (Phase 0) | **Working** — add/edit/save via vanilla JS modal |
| NERIS ID field width | **5 chars** on admin modal and registration |
| `IsFDIDValid(int)` | **Active** — single caller: `Register.aspx.cs` |
| `GetFG_FDID(int)` | **Defined** — zero callers in repo (unused) |
| General Information master-list override (Phase 1) | **Not implemented** — separate plan |

---

## 3. IsFDIDValid — usage analysis

### What it does

During **new-user registration**, `Register.aspx.cs` calls `IsFDIDValid` to confirm the entered NERIS ID exists in the `FG_FDIDs` master list before creating a web user account.

### What it does **not** do

- Does not gate login
- Does not validate General Information save
- Does not affect admin Manage FD ID's CRUD

### Call graph (today)

```
Register.aspx.cs
  └── Convert.ToInt32(txtFDID.Text)
        └── fgService.IsFDIDValid(int fdid)
              └── pad 4-digit → 5 with leading "0"
              └── FG_FDIDs.FirstOrDefault(a => a.FDID == strFDID)
                    └── null  → "You must enter a valid NERIS ID"
                    └── row   → continue registration
```

### Why it must change (not be deleted)

Registration **depends** on master-list validation. The method's **capability** stays; the **int-based signature and implementation** are superseded by `GetFDIDByIdAsync(string)`.

`GetFG_FDID(int)` has no callers and is unused, but per repo policy it will be **commented out** alongside `IsFDIDValid(int)` rather than removed.

---

## 4. Comment-out policy

**Do not delete superseded code.** Comment it out with a brief note explaining what replaced it and when.

Standard prefix:

```csharp
// Legacy (pre-NERIS 20-char): ...
```

Apply to:

| File type | Action |
|-----------|--------|
| `.aspx` markup | Comment out old control; add new control below |
| `.aspx.cs` | Comment out old call site; add new call below |
| `IFGService.cs` | Comment out old method signatures; add new signature |
| `FGService.cs` | Comment out full method bodies; add new method below |
| `Register.aspx.designer.cs` | Comment out old field type; add active `TextBox` declaration |

**Out of scope for edits:** `publish/` folder and `NMSFMFireGrantWF_Backup_*` folders (deploy artifacts / backups).

---

## 5. Implementation steps

### Step 1 — UI width (MaxLength 20)

#### 5.1 [`ManageFDIDs.aspx`](../NMSFMFireGrantWF/Admin/ManageFDIDs.aspx)

**Current (line ~165):**

```aspx
<asp:TextBox ID="txtFDID" runat="server" Width="100px" ClientIDMode="Static" MaxLength="5" aria-required="true"></asp:TextBox>
```

**Change to:**

```aspx
<%-- Legacy (pre-NERIS 20-char): MaxLength="5", Width="100px"
<asp:TextBox ID="txtFDID" runat="server" Width="100px" ClientIDMode="Static" MaxLength="5" aria-required="true"></asp:TextBox>
--%>
<asp:TextBox ID="txtFDID" runat="server" Width="180px" ClientIDMode="Static" MaxLength="20" aria-required="true"></asp:TextBox>
```

#### 5.2 [`Register.aspx`](../NMSFMFireGrantWF/Account/Register.aspx)

**Current (line ~74):**

```aspx
<telerik:RadNumericTextBox ID="txtFDID" runat="server" ... MaxLength="5" ...></telerik:RadNumericTextBox>
```

**Change to:**

```aspx
<%-- Legacy (pre-NERIS 20-char): numeric-only RadNumericTextBox, MaxLength="5"
<telerik:RadNumericTextBox ID="txtFDID" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Type="Number" MaxLength="5" aria-required="true" ClientIDMode="Static" CausesValidation="true"></telerik:RadNumericTextBox>
--%>
<asp:TextBox ID="txtFDID" runat="server" CssClass="form-control" MaxLength="20" aria-required="true" ClientIDMode="Static" CausesValidation="true"></asp:TextBox>
```

#### 5.3 [`Register.aspx.designer.cs`](../NMSFMFireGrantWF/Account/Register.aspx.designer.cs)

Comment out:

```csharp
// Legacy (pre-NERIS 20-char): RadNumericTextBox for numeric-only 5-digit FDID.
// protected global::Telerik.Web.UI.RadNumericTextBox txtFDID;
```

Add:

```csharp
protected global::System.Web.UI.WebControls.TextBox txtFDID;
```

#### 5.4 [`GeneralInformation.aspx`](../NMSFMFireGrantWF/Application/GeneralInformation.aspx)

**Current (line ~25):**

```aspx
<asp:TextBox ID="txtFDID" runat="server" class="form-control" aria-required="true"></asp:TextBox>
```

**Change to:**

```aspx
<asp:TextBox ID="txtFDID" runat="server" class="form-control" MaxLength="20" aria-required="true"></asp:TextBox>
```

---

### Step 2 — Uppercase alpha normalization

#### 5.5 [`ManageFDIDs.aspx`](../NMSFMFireGrantWF/Admin/ManageFDIDs.aspx) — client script

Add helper and wire to `#txtFDID`:

```javascript
function fdidNormalizeNerisId(value) {
    return (value || '').trim().toUpperCase();
}

function fdidApplyNerisIdNormalization() {
    var field = document.getElementById('txtFDID');
    if (!field) {
        return;
    }
    field.value = fdidNormalizeNerisId(field.value);
}

// In fdidClearForm — no change needed (clears to '')

// In fdidOpenForEdit — after setting txtFDID from data-fdid:
//   field.value = fdidNormalizeNerisId(link.getAttribute('data-fdid') || '');

// On DOM ready or end of script block:
(function () {
    var field = document.getElementById('txtFDID');
    if (!field) {
        return;
    }
    field.addEventListener('input', fdidApplyNerisIdNormalization);
    field.addEventListener('blur', fdidApplyNerisIdNormalization);
})();
```

Update `fdidOpenForEdit` to normalize the loaded value.

#### 5.6 [`ManageFDIDs.aspx.cs`](../NMSFMFireGrantWF/Admin/ManageFDIDs.aspx.cs) — server (authoritative)

In `btnSaveFDID_Click`, replace:

```csharp
string nerisId = txtFDID.Text.Trim();
```

With:

```csharp
string nerisId = txtFDID.Text.Trim().ToUpperInvariant();
```

Use `nerisId` for all duplicate checks, save, delete/replace, and comparison with `hfFDID.Value` (normalize `hfFDID` comparison values too if needed).

#### 5.7 [`Register.aspx`](../NMSFMFireGrantWF/Account/Register.aspx) — client script

Add similar uppercase-on-input script for `#txtFDID` (inline script block or shared pattern from ManageFDIDs).

#### 5.8 [`Register.aspx.cs`](../NMSFMFireGrantWF/Account/Register.aspx.cs) — server

Comment out legacy block (~line 314):

```csharp
// Legacy (pre-NERIS 20-char): numeric-only validation via IsFDIDValid(int).
// var fdid = await fgService.IsFDIDValid(Convert.ToInt32(txtFDID.Text));
```

Add before lookup:

```csharp
string nerisId = txtFDID.Text.Trim().ToUpperInvariant();
var fdid = await fgService.GetFDIDByIdAsync(nerisId);
```

---

### Step 3 — Service layer refactor

#### 5.9 [`IFGService.cs`](../../NMSFM.Services/FireGrant/IFGService.cs)

Comment out:

```csharp
// Legacy (pre-NERIS 20-char): int-based lookup; no alphanumeric support.
// Task<FG_FDIDs> GetFG_FDID(int fdid);
// Task<FG_FDIDs> IsFDIDValid(int fdid);
```

Add:

```csharp
Task<FG_FDIDs> GetFDIDByIdAsync(string nerisId);
```

#### 5.10 [`FGService.cs`](../../NMSFM.Services/FireGrant/FGService.cs)

Comment out full bodies of `GetFG_FDID(int)` and `IsFDIDValid(int)` (preserve for history).

Add new method:

```csharp
public async Task<FG_FDIDs> GetFDIDByIdAsync(string nerisId)
{
    FG_FDIDs result = null;
    if (string.IsNullOrWhiteSpace(nerisId))
    {
        return null;
    }

    string normalizedId = nerisId.Trim().ToUpperInvariant();

    try
    {
        result = await cwmContext.FG_FDIDs
            .FirstOrDefaultAsync(a => a.FDID == normalizedId);

        // Legacy fallback: 4-digit numeric NFIRS IDs stored as 5-digit with leading zero.
        if (result == null
            && normalizedId.Length == 4
            && normalizedId.All(char.IsDigit))
        {
            result = await cwmContext.FG_FDIDs
                .FirstOrDefaultAsync(a => a.FDID == "0" + normalizedId);
        }
    }
    catch (Exception ex)
    {
        _ = ex;
        logger.Error(
            "Unexpected exception caught while retrieving the Fire Grant FDID by id.",
            ex);
    }

    return result;
}
```

**Note on inactive IDs:** Current `IsFDIDValid` does not filter `Inactive == true`. Preserve that behavior unless product owner requests stricter registration rules. Document in a code comment if unchanged.

Requires `using System.Linq;` if not already present.

---

## 6. Data flow after implementation

```
Admin ManageFDIDs modal
  └── uppercase normalize (client + server)
  └── SaveFDIDAsync → FG_FDIDs.FDID

Registration
  └── uppercase normalize (client + server)
  └── GetFDIDByIdAsync(string) → FG_FDIDs
  └── null → block registration

General Information
  └── MaxLength="20" on txtFDID
  └── saves to FG_App_GeneralInfo.NFIRSID (no master-list lookup in this change)
```

No changes to `NMSFM.Data` entity files are required.

---

## 7. Files changed (summary)

| File | Change |
|------|--------|
| `NMSFMFireGrantWF/Admin/ManageFDIDs.aspx` | MaxLength 20, width, uppercase JS |
| `NMSFMFireGrantWF/Admin/ManageFDIDs.aspx.cs` | `ToUpperInvariant()` on save |
| `NMSFMFireGrantWF/Account/Register.aspx` | TextBox replaces RadNumericTextBox; MaxLength 20; uppercase JS |
| `NMSFMFireGrantWF/Account/Register.aspx.cs` | String lookup via `GetFDIDByIdAsync` |
| `NMSFMFireGrantWF/Account/Register.aspx.designer.cs` | TextBox field declaration |
| `NMSFMFireGrantWF/Application/GeneralInformation.aspx` | MaxLength 20 |
| `NMSFM.Services/FireGrant/IFGService.cs` | New interface method; comment out old |
| `NMSFM.Services/FireGrant/FGService.cs` | New implementation; comment out old |

**Build order:** `NMSFM.Data` → `NMSFM.Services` → `NMSFMFireGrantWF`

**Build command** (from `NMSFMFireGrantWF/`):

```powershell
.\build.ps1 -SkipToolingAudit -SkipDependencyAudit
```

---

## 8. Test plan

| # | Scenario | Expected result |
|---|----------|-----------------|
| 1 | Admin: enter mixed-case alphanumeric ID ≤ 20 chars | Auto-uppercases on input; saves; appears in grid |
| 2 | Admin: enter ID longer than 5 chars | Saves successfully (regression for original issue) |
| 3 | Admin: edit existing ID, change department name only | Updates without error |
| 4 | Admin: change NERIS ID to new value | Old row deleted, new row saved |
| 5 | Registration: valid numeric ID from master list | Registration proceeds |
| 6 | Registration: valid alphanumeric ID from master list | Registration proceeds |
| 7 | Registration: ID not in master list | Error: "You must enter a valid NERIS ID" |
| 8 | Registration: legacy 4-digit numeric ID stored as 5-digit with leading zero | Validates via fallback |
| 9 | General Information: save/load 20-char NERIS ID | Persists to `NFIRSID` |
| 10 | Build solution | No compile errors |

---

## 9. Rollback

Because superseded code remains commented in place:

1. Revert active lines to commented legacy blocks (swap comment markers).
2. Rebuild and redeploy.
3. No database migration required — schema unchanged.

---

## 10. Out of scope (this document)

- Phase 1: General Information prefill from `FG_FDIDs` master list by department name (Option A FY rollover)
- Bulk import/sync of NERIS IDs
- Login/password flow changes (NERIS ID is not used as password in current login code)
- Filtering inactive IDs during registration (unless explicitly requested)
- Changes to `publish/` or backup folders

---

## 11. Implementation checklist

- [x] Step 1: UI MaxLength 20 (ManageFDIDs, Register, GeneralInformation)
- [x] Step 1: Register designer TextBox type
- [x] Step 2: Uppercase JS + server normalization (admin + registration)
- [x] Step 3: `GetFDIDByIdAsync` in IFGService / FGService
- [x] Step 3: Comment out `IsFDIDValid(int)` and `GetFG_FDID(int)`
- [x] Step 3: Update Register.aspx.cs caller
- [x] Build passes
- [ ] Manual test plan (section 8) complete
