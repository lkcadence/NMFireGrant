# Swap PPE and Hazards/Threats — Implementation Plan

## Goal

In the fire department application Step 1 sidebar, place **PPE** before **Hazards/Threats** (currently the reverse). Also update **Previous/Next** buttons so the wizard flow matches the new order:

```text
... → Communication Equipment → PPE → Hazards/Threats → Equipment Needs → ...
```

---

## Current vs target

| Area | Current | Target |
|------|---------|--------|
| Sidebar (after Communication Equipment) | Hazards/Threats, then PPE | **PPE**, then **Hazards/Threats** |
| Communication Equipment → Next | HazardsThreats | **PPE** |
| PPE → Back | HazardsThreats | **CommunicationEquipment** |
| PPE → Next | EquipmentNeeds | **HazardsThreats** |
| Hazards/Threats → Back | CommunicationEquipment | **PPE** |
| Hazards/Threats → Next | PPE | **EquipmentNeeds** |
| Equipment Needs → Back | PPE | **HazardsThreats** |

Sidebar menu clicks use `case "PPE"` / `case "Hazards/Threats"` in each page's `rmStep1_Click` — **no change needed** there (order-independent).

---

## Files changed

### 1. Sidebar menu markup

**File:** `NMSFMFireGrantWF/Application/ApplicationMstr.Master`

Swap the two `RadMenuItem` lines so PPE appears before Hazards/Threats.

### 2. Validation status icons

**File:** `NMSFMFireGrantWF/Application/ApplicationMstr.Master.cs`

Status ticks/crosses use fixed indices on `rmStep1`. After the menu swap:

- `GetFGApplicationPPE` → `rmStep1.Items[9]`
- `GetFGApplicationHazardsThreats` → `rmStep1.Items[10]`

### 3. Wizard navigation

| File | Method | Redirect |
|------|--------|----------|
| `CommunicationEquipment.aspx.cs` | `btnNext_Click` | `~/Application/PPE` |
| `PPE.aspx.cs` | `btnBack_Click` | `~/Application/CommunicationEquipment` |
| `PPE.aspx.cs` | `btnNext_Click` | `~/Application/HazardsThreats` |
| `HazardsThreats.aspx.cs` | `btnBack_Click` | `~/Application/PPE` |
| `HazardsThreats.aspx.cs` | `btnNext_Click` | `~/Application/EquipmentNeeds` |
| `EquipmentNeeds.aspx.cs` | `btnBack_Click` | `~/Application/HazardsThreats` |

---

## Out of scope

- `publish/` and backup project copies
- Application print/report section order
- Database or validation rule changes
- Renaming menu text

---

## Test plan

1. Open any FY2027 application — sidebar shows **PPE** above **Hazards/Threats**.
2. **Communication Equipment** → Next → **PPE**.
3. **PPE** → Previous → Communication Equipment; Next → Hazards/Threats.
4. **Hazards/Threats** → Previous → PPE; Next → Equipment Needs.
5. **Equipment Needs** → Previous → Hazards/Threats.
6. Sidebar clicks load the correct page.
7. Status icons appear on the correct menu items.
