# Part-of-Project Conditional Sections — Detailed Implementation Plan

**Project:** NMSFM Fire Grant Web Application  
**Document version:** 1.0  
**Date:** June 22, 2026  
**Status:** Implemented  
**Scope:** Conditional hide/show, field clearing, and validation bypass for PPE, Communication
Equipment, and Apparatus gate questions.

**Related artifacts:**

- Requirements summary:
  [`docs/planning/part-of-project-conditional-sections-plan.md`](planning/part-of-project-conditional-sections-plan.md)
- Target pages:
  [`NMSFMFireGrantWF/Application/PPE.aspx`](../NMSFMFireGrantWF/Application/PPE.aspx),
  [`CommunicationEquipment.aspx`](../NMSFMFireGrantWF/Application/CommunicationEquipment.aspx),
  [`Apparatus.aspx`](../NMSFMFireGrantWF/Application/Apparatus.aspx)

---

## 1. Problem statement

Applicants who answer **No** to "is part of the project?" questions should not be blocked by
required fields below the gate. Today:

- **PPE:** UI hides `#dvPPE` / `#dvSCBA`, but inspected validation reads wrong radios; child
  data is not cleared on hide.
- **Communication Equipment:** Interoperability/repeater sections stay visible; radio counts
  validate even when gate = No.
- **Apparatus:** Pump/hose sections stay visible when gate = No; print view still shows details.

---

## 2. Target behavior

| Gate answer | UI | Validation | Persisted data |
|---|---|---|---|
| Unanswered | Show gate only | Gate required | Unchanged |
| Yes | Show all child fields | Child fields required per existing rules | Full child data |
| No | Hide all child fields | Only gate required; `IsValid = true` | Child fields cleared |

When user toggles **Yes → No**, hidden values are cleared client-side (immediate UX) and
server-side on save (authoritative).

---

## 3. PPE page

### 3.1 UI — `PPE.aspx`

**Containers (unchanged):**

- `#dvPPE` — PPE inspected + Standard Compliant PPE grid
- `#dvSCBA` — Standard Compliant SCBA grid (independent gate below PPE block)

**JavaScript updates:**

`showPPE()` — on hide, call `clearPPEFields()`:

- Uncheck `#rbPPEInspectedYes`, `#rbPPEInspectedNo`
- Call `clearNoteId()` with corrected field IDs (`Compliaint` spelling)

`showSCBA()` — on hide, call `clearNote2Id()`

Fix `clearNoteId()` IDs:

```javascript
$('#hfStandardCompliaintPPEId').val('');
$('#txtStandardCompliaintPPEYear').val('');
$('#txtStandardCompliaintPPEQuantity').val('');
$('#txtStandardCompliaintPPEAge').val('');
$('#ddlPPEType').val('');
```

### 3.2 Server — `PPE.aspx.cs` `SaveForm()`

**Bug fix** — replace lines reading `rbPPEYes`/`rbPPENo` for inspected with:

```csharp
if (rbPPEInspectedYes.Checked) { ppeInspected = 1; }
if (rbPPEInspectedNo.Checked) { ppeInspected = 2; }
```

**When `ppePartOfProject == 2`:**

```csharp
ppeInspected = 0;
rbPPEInspectedYes.Checked = false;
rbPPEInspectedNo.Checked = false;
standardPPE = new List<FG_App_StandardPPE>();
ViewState["dtPPE"] = standardPPE;
```

**When `scbaPartOfProject == 2`:**

```csharp
standardSCBA = new List<FG_App_StandardSCBA>();
ViewState["dtSCBA"] = standardSCBA;
```

Existing `if (ppePartOfProject == 1 && …)` / `if (scbaPartOfProject == 1 && …)` blocks
remain unchanged.

---

## 4. Communication Equipment page

### 4.1 UI — `CommunicationEquipment.aspx`

Move `</div>` closing `#dvCommunications` from after equipment grid (line ~100) to after
`#dvRepeaterDescription` (line ~176). Admin block `#dvAdmin` stays outside.

**`showCommunications()` clear-on-hide:**

- `#ApplicationContent_txtHandheldRadios`, `_txtBaseStations`, `_txtMobileRadios` → empty/0
- Uncheck `#rbAppNoRadioYes`, `#rbAppNoRadioNo`
- Uncheck interoperability radios (`#rbLawEnforcement*`, `#rbEmergencyMedical*`,
  `#rbOtherFD*`, `#rbOther*`, `#rbNotCovered*`)
- Clear `#txtOtherDescription`, `#txtRepeaterDescription`
- Call `showOtherDesc()` and `showRepeaterDesc()` to sync nested visibility

### 4.2 Server — `CommunicationEquipment.aspx.cs` `SaveForm()`

Gate ungated validations:

```csharp
if (communicationPart == 1 && txtHandheldRadios.Text == "") { … }
if (communicationPart == 1 && txtBaseStations.Text == "") { … }
if (communicationPart == 1 && txtMobileRadios.Text == "") { … }
if (communicationPart == 1 && other == 1 && txtOtherDescription.Text == "") { … }
if (communicationPart == 1 && notCovered == 1 && txtRepeaterDescription.Text == "") { … }
```

**When `communicationPart == 2`:** reset all child flags to 0, numeric fields to 0, clear
text fields, empty `communicationEquipment` list and ViewState.

---

## 5. Apparatus page

### 5.1 UI — `Apparatus.aspx`

Add wrapper after gate question:

```html
<div id="dvApparatusDetails" style="display:none">
  <!-- pump tests, hose tests, statutes, dvApparatus list -->
</div>
```

Remove `style="display:none"` from inner `#dvApparatus` (parent controls visibility).

Rename/refactor `showAddApparatus()` → show/hide `#dvApparatusDetails`.

**Clear-on-hide:**

- Uncheck pump/hose radios
- Clear `#ApplicationContent_txtNoPumpTestsExp`, `_txtNoHoseTests`
- Hide `#dvNoPumpTestsExp`, `#dvNoHoseTests`
- Call `clearNoteId()` for modal fields

### 5.2 Server — `Apparatus.aspx.cs` `SaveForm()`

Validation already gated on `apparatusPart == 1`.

**When `apparatusPart == 2`:**

```csharp
pumpTests = 0;
hoseTests = 0;
txtNoPumpTestsExp.Text = "";
txtNoHoseTests.Text = "";
apparatusEquipment = new List<FG_App_ApparatusEquipment>();
ViewState["dtApparatusEquipment"] = apparatusEquipment;
```

Clear radio button checked states on server.

---

## 6. Print view — `ApplicationPrint.aspx` / `.cs`

Wrap apparatus detail rows in `<tbody id="tbApparatusPart" runat="server">` (pump/hose rows
in first table).

Add `runat="server" id="tbApparatusList"` on apparatus equipment list table.

In `LoadApparatus()`:

```csharp
bool showDetails = model.ApparatusPartOfProject == 1;
tbApparatusPart.Visible = showDetails;
tbApparatusList.Visible = showDetails;
```

Communication and PPE print views already hide child sections — no change.

---

## 7. Validation matrix

| Field group | Gate = Yes | Gate = No |
|---|---|---|
| Gate question | Required | Required |
| PPE inspected + PPE grid | Required | Skipped |
| SCBA grid | Required | Skipped |
| Comm counts + interoperability + repeater | Required | Skipped |
| Apparatus pump/hose + list | Required | Skipped |
| Admin comments | Admin only | Admin only |

---

## 8. Manual test cases

### TC-PPE-01 — PPE gate No

1. Open PPE page; select **No** for PPE question.
2. Verify `#dvPPE` hidden; SCBA question still visible.
3. Answer SCBA question; Save.
4. Expect no validation errors; `IsValid = true`.

### TC-PPE-02 — PPE gate Yes incomplete

1. Select **Yes** for PPE; leave inspected unanswered.
2. Save.
3. Expect "PPE Inspected" validation error.

### TC-PPE-03 — PPE Yes → No toggle

1. Select Yes; add PPE grid row; select No.
2. Save and reload.
3. Expect PPE child data cleared in DB.

### TC-SCBA-01 — SCBA gate No

Same pattern for SCBA section independently.

### TC-COMM-01 — Communications gate No

1. Select **No** for Communications.
2. Verify interoperability/repeater sections hidden.
3. Save — expect success with only gate answered.

### TC-COMM-02 — Communications gate Yes incomplete

1. Select Yes; leave interoperability unanswered.
2. Save — expect validation errors.

### TC-APP-01 — Apparatus gate No

1. Select **No**; verify pump/hose/list hidden.
2. Save — success.

### TC-APP-02 — Print view

1. Save Apparatus with gate = No.
2. Open Application Print.
3. Expect pump/hose/list sections hidden.

### TC-READONLY — Read-only session

Verify `Session["ReadOnly"]` bypass unchanged on all three pages.

---

## 9. Files changed

| File | Change type |
|---|---|
| `docs/planning/part-of-project-conditional-sections-plan.md` | New |
| `docs/part-of-project-conditional-sections-implementation-plan.md` | New |
| `Application/PPE.aspx` | JS clear-on-hide |
| `Application/PPE.aspx.cs` | Validation fix + clear on No |
| `Application/CommunicationEquipment.aspx` | Markup wrapper + JS |
| `Application/CommunicationEquipment.aspx.cs` | Validation gating + clear on No |
| `Application/Apparatus.aspx` | Wrapper + JS |
| `Application/Apparatus.aspx.cs` | Clear on No |
| `Application/Reporting/ApplicationPrint.aspx` | tbody/table wrappers |
| `Application/Reporting/ApplicationPrint.aspx.cs` | Visibility logic |
| `Application/Reporting/ApplicationPrint.aspx.designer.cs` | New control declarations |

---

## 10. Out of scope

- Database schema changes
- Client-side required validators (pages use server-side `SaveForm()` only)
- Changes to submission gate logic beyond existing `IsValid` flags
