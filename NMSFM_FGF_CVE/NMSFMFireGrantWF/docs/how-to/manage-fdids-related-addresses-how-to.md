# Manage FDIDs / Related Addresses — Admin How-To

**Project:** NMSFM Fire Grant Web Application  
**Document version:** 1.0  
**Date:** June 28, 2026  
**Audience:** Fire Grant web administrators  
**Page:** Admin → **Manage FDIDs / Related Addresses** (`/Admin/ManageFDIDs`)

**Related technical docs (developers):**

- [`fdid-modal-address-sync-implementation-plan.md`](../fdid-modal-address-sync-implementation-plan.md)
- [`fdid-modal-address-edit-udf-implementation-plan.md`](../fdid-modal-address-edit-udf-implementation-plan.md)
- [`manage-fdid-list-filter-sort-implementation-plan.md`](../manage-fdid-list-filter-sort-implementation-plan.md)

---

## 1. What this page is for

The **Manage FDIDs / Related Addresses** page maintains the master list of fire department **NERIS IDs** (stored as `FDID` in the database) and their department names. Each row in this list is used across the Fire Grant application—for example:

- **General Information** — NERIS ID prefill and department lookup
- **Edit User** — department dropdown labels
- **Award, denial, and print reports** — department name and address on documents

After the NERIS migration, department names in this master list may no longer match the **Codepal fire department address** records. This page lets you:

1. Add or update NERIS IDs and department names in the master list.
2. **Link** an existing Codepal address to the department name, or **create / edit** the physical address.
3. Update **department information** (ISO rating, station counts) stored as Codepal user-defined fields (UDFs).

Keeping the master list and related addresses aligned prevents missing NERIS IDs, wrong department names, and blank or incorrect addresses on grant documents.

---

## 2. Who can use this page

Only users who are signed in as a **Web Admin** can open this page. External users and non-admin staff are redirected to **Unauthorized**.

**Navigation:** From the admin menu bar, open **Admin** → **Manage FDIDs / Related Addresses**.

**Page help:** Click the help icon (upper right) to expand administrator help text configured under **Manage Help Text** (page key: *FDIDs (Admin)*).

---

## 3. Fire Department ID list

The main grid shows all NERIS IDs from the master table.

| Column | Description |
|--------|-------------|
| **Edit** | **View/Edit** link — opens the Fire Department ID modal for that row |
| **NERIS ID** | The department’s NERIS identifier (up to 20 characters; stored in uppercase) |
| **Fire Department** | Department name associated with the NERIS ID |
| **Inactive** | When checked on the record, the department is treated as inactive in this list |

The grid supports **paging** (25 rows per page) and **sorting** by NERIS ID or Fire Department (click column headers).

### Search and filter

Use the toolbar above the grid:

| Control | What it does |
|---------|----------------|
| **Search NERIS ID** | Partial match on NERIS ID (not case-sensitive) |
| **Search Fire Department** | Partial match on department name |
| **Hide inactive departments** | When checked (default), inactive rows are hidden |
| **Apply** | Runs the search/filter and returns to page 1 |
| **Clear** | Clears search boxes, re-checks *Hide inactive*, resets sort to NERIS ID ascending |

Filters apply to the in-memory list loaded for the session; click **Apply** after changing search text.

---

## 4. Add a new NERIS ID

1. Click **Add New NERIS ID**.
2. In the modal, complete the required fields:
   - **NERIS ID** * (required)
   - **Department Name** * (required)
3. Optionally configure fire department address and department information (see sections 5 and 6).
4. Set **Inactive** if the department should not appear when *Hide inactive departments* is checked.
5. Click **Save NERIS ID**.

On success, the page reloads with a green message: *NERIS ID saved successfully.*

**Validation:**

- NERIS ID and Department Name cannot be blank.
- NERIS ID must be unique; duplicates show an error in the modal.
- NERIS ID is normalized to **uppercase** as you type.

---

## 5. View or edit an existing NERIS ID

1. In the grid, click **View/Edit** (or **View/Edit** followed by the NERIS ID).
2. The modal opens with NERIS ID, department name, and inactive flag filled in.
3. If address sync is enabled (see section 8), the system loads matching addresses and, when possible, the address already linked to that department name.
4. Make changes and click **Save NERIS ID**.

**Changing the NERIS ID:** You may edit the NERIS ID field. Saving replaces the old ID with the new one in the master list. The new ID must not already exist.

**Changing the department name:** Update **Department Name** and save. If you link or create an address (section 6), the linked Codepal address **Address Code** is updated to match the new department name.

---

## 6. Fire department address (link vs create / edit)

When address sync is enabled, the modal includes a **Fire department address** section below the department name.

> Link an existing Codepal fire department address or create a new one. Use **Full Address** in the dropdown to distinguish departments with the same name.

Choose one of two actions:

### Option A — Link existing address

1. Select **Link existing address** (default).
2. Open **Link to address** and choose a row from the dropdown.

Each option is shown as:

```text
{Department code} — {Full street address} (Apps: {count}, Users: {count})
```

- **Apps** — number of Fire Grant applications tied to that address
- **Users** — number of active user/address party links

Use the full address and counts to pick the correct record when several departments share a similar name (for example, multiple “Clovis Fire Department” entries).

3. When you select an address, the form may switch to **Create / Edit Address** mode with that address loaded so you can review or adjust fields before saving.
4. Click **Save NERIS ID**.

**What “link” does:** Updates the selected Codepal address so its **Address Code** matches the **Department Name** you entered. It does not create a new address row.

**Tip:** If no suitable address appears, pick **— Create / Edit Address (new) —** at the bottom of the dropdown to switch to create mode.

### Option B — Create / Edit Address

1. Select **Create / Edit Address**, or choose **— Create / Edit Address (new) —** from the link dropdown.
2. Complete the address fields.

| Field | Required | Notes |
|-------|----------|-------|
| **Address type** | Yes | Defaults to *FS Fire Department* when available |
| **Street number** | No | |
| **Direction** | No | e.g. N, S, E, W |
| **Street name** | No | |
| **Suffix** | No | e.g. St, Ave, Blvd |
| **City** | Yes | |
| **State** | Yes | Defaults to New Mexico when configured |
| **County** | Yes | |
| **Zip** | Yes | e.g. `88101`; system may create zip if missing |

The help text notes that a full address is **required for invoices and legal documents**—provide complete city, state, county, and zip at minimum.

3. Click **Save NERIS ID**.

**What “create” does:** Inserts a new Codepal fire department address with the physical fields you entered and **Address Code** set to the department name.

**What “edit” does:** If an existing address was loaded (from **View/Edit** or from the link dropdown), saving updates that address record instead of creating a new one.

---

## 7. Department information (Codepal UDFs)

When address sync is enabled, the modal includes **Department information (Codepal UDFs)**:

| Field | Description |
|-------|-------------|
| **ISO Rating** | ISO classification for the department |
| **Main Stations** | Count of main stations |
| **Substations** | Count of substations |
| **Admin Buildings** | Count of administrative buildings |

These values are stored on the linked or created address and are the same fields shown on **General Information** for applicants.

**Validation:** Each count field must be a **non-negative whole number** (blank is treated as zero). Invalid values show an error in the modal.

UDFs are saved when an address is successfully linked, created, or updated. If you save a NERIS ID without linking or creating an address, UDF values are not written.

---

## 8. Inactive departments

Check **Inactive** in the modal to mark a NERIS ID as inactive in the master list.

- Inactive rows show **Inactive = True** in the grid.
- With **Hide inactive departments** checked (default), inactive rows are hidden until you clear filters or uncheck the box and click **Apply**.

Use inactive for departments that should remain in history but should not appear in normal admin workflows.

---

## 9. Modal tips

- **Drag the modal** by its title bar to move it on screen.
- **Close** (footer or ×) dismisses the modal without saving.
- Errors appear in a **red alert** at the top of the modal; fix the issue and save again.
- After a successful save, the modal closes and the list refreshes.

---

## 10. Common errors and what to do

| Message / situation | Likely cause | What to do |
|---------------------|--------------|------------|
| *NERIS ID cannot be blank* | Empty NERIS ID | Enter a valid NERIS ID |
| *Department Name cannot be blank* | Empty name | Enter the official department name |
| *NERIS ID exists in the list* | Duplicate ID | Use a unique NERIS ID or edit the existing row |
| *Another active address already uses this department name* | Another fire dept address has the same Address Code | Pick a different department name, link the correct existing address, or resolve the duplicate in Codepal |
| *Selected address was not found* | Address was deleted after the dropdown loaded | Close modal, reopen **View/Edit**, and select again |
| *City / State / County / Zip / Address type is required…* | Create/Edit mode with missing required fields | Fill required address fields before saving |
| *{Field} must be a non-negative whole number* | Invalid ISO or station count | Enter 0 or a positive integer |
| *Unable to resolve or create zip code* | Zip not in system and could not be created | Verify zip format and county |
| Multiple addresses match department name (on open) | More than one active address shares the same Address Code | Use **Link to address** and choose the correct row using full address and App/User counts |

If address sections are **not visible** in the modal, address sync may be turned off in application configuration (`EnableFdidAddressSync`). Contact your application administrator or developer; NERIS ID and department name can still be saved without address linking.

---

## 11. What this page does *not* do

Understanding these limits avoids unexpected results:

- Does **not** automatically create user accounts or link users to addresses
- Does **not** update applicant **General Information** department name text already saved on open applications
- Does **not** create remittance address types used for payment routing
- Does **not** bulk-sync every department in one action—you work one NERIS ID at a time through the modal

---

## 12. Recommended workflow (NERIS migration cleanup)

For a department that is missing NERIS prefill or shows the wrong name on reports:

1. Search the grid by department name or NERIS ID.
2. Open **View/Edit** (or **Add New NERIS ID** if the department is missing from the master list).
3. Confirm **NERIS ID** and **Department Name** match official NERIS records.
4. Under **Fire department address**, link the correct Codepal address **or** create/edit the physical address.
5. Enter **ISO Rating** and station counts if known.
6. Save and confirm the green success message.
7. Verify in the applicant flow: open **General Information** for a test application tied to that department and confirm NERIS ID and department details appear correctly.

---

## 13. Quick reference

| Task | Steps |
|------|--------|
| Find a department | Search NERIS ID or Fire Department → **Apply** |
| Add NERIS ID | **Add New NERIS ID** → fill form → **Save NERIS ID** |
| Edit NERIS ID | **View/Edit** → change fields → **Save NERIS ID** |
| Link to Codepal address | **Link existing address** → choose dropdown row → **Save NERIS ID** |
| New physical address | **Create / Edit Address** → fill address → **Save NERIS ID** |
| Hide inactive | Keep **Hide inactive departments** checked → **Apply** |
| Dismiss modal without saving | **Close** in the modal footer (or ×) |
| Leave page | Use the admin menu (e.g. **Current Apps**) or browser back |

---

*For technical implementation details, database tables, and rollback procedures, see the related developer documents linked at the top of this guide.*
