# Manage Agency Information — Admin How-To

**Project:** NMSFM Fire Grant Web Application  
**Document version:** 1.0  
**Date:** June 28, 2026  
**Audience:** Fire Grant web administrators  
**Page:** Admin → **Manage Agency Information** (`/Admin/ManageAgencyInformation`)

**Related technical docs (developers):**

- [`manage-agency-information-implementation-plan.md`](../manage-agency-information-implementation-plan.md)
- [`support-email-admin-tab-implementation-plan.md`](../support-email-admin-tab-implementation-plan.md)

---

## 1. What this page is for

The **Manage Agency Information** page lets web administrators view and edit the
**agency record** associated with their login session. This is the same agency
contact data maintained in the LegacyApp **Agency** form, now available in the
Fire Grant web application.

Agency information is used across the application—for example:

- **Grant reports** — agency name, address, and report image on printed documents
  (denial letters, award letters, and similar outputs)
- **Support menu** — technical and fire services support email recipients
- **Agency-level settings** — user-defined fields (UDFs) configured in Codepal

The page opens an **Agency Information** modal automatically when you navigate to
it. You edit fields across three tabs and click **Save** to persist changes to
the Codepal database.

---

## 2. Who can use this page

Only users who are signed in as a **Web Admin** can open this page. External
users and non-admin staff are redirected to **Unauthorized**.

**Navigation:** From the admin menu bar, open **Admin** → **Manage Agency
Information**.

The menu item is also available from the application sidebar when you are
editing a grant application (application master navigation).

**Page help:** Click the help icon (upper right) to expand administrator help
text configured under **Manage Help Text** (page key: *Manage Agency Information
(Admin)*).

**Agency scope:** You can edit only the agency tied to your login session
(`AgencyId` set at sign-in). This page does not list or switch between multiple
agencies.

---

## 3. Opening the Agency Information modal

1. Open **Admin** → **Manage Agency Information**.
2. The **Agency Information** modal opens automatically with current data loaded.
3. Use the **General**, **Advanced**, and **Support Email** tabs to review or
   edit fields.
4. Click **Save** to persist changes, or **Close** to return to **Current Apps**
   (`/Admin/Home`) without saving.

The modal stays on screen while you work. After a successful save, a green
success message appears above the page title and the modal reopens with updated
values.

If validation fails, a **red alert** appears at the top of the modal and the
modal stays open so you can correct the issue.

---

## 4. Inactive checkbox

At the top right of the modal, the **Inactive** checkbox marks the agency
record as inactive in Codepal (stored in the `ExternalId` field: `0` = active,
`1` = inactive).

Use this only when directed by your agency administrator or when retiring an
agency record. In most day-to-day Fire Grant administration, leave **Inactive**
unchecked.

---

## 5. General tab

The **General** tab contains agency contact information and the report image.

### Contact fields

| Field | Max length | Notes |
|-------|------------|-------|
| **Name** | 50 | Official agency name (`AgencyName`) |
| **Sub Name** | 50 | Secondary name or subtitle (`AgencySubName`) |
| **Address** | 50 | Street address |
| **City** | 50 | |
| **State / Zip** | — | State dropdown plus zip text (zip max 20 characters) |
| **Country** | — | Country dropdown |
| **Phone** | 25 | |
| **Fax** | 25 | |
| **E-mail** | 100 | Validated as an email address format |

Layout: contact fields appear in the left column; the report image panel is on
the right.

### Report image

The **Report Image** appears on printed grant reports. It is stored in the
agency record as a binary image.

| Action | How |
|--------|-----|
| **View current image** | A 150×150 thumbnail preview appears when an image is saved |
| **Upload new image** | Use **Choose File** (file upload control), then click **Save** |
| **Clear image** | Click **Clear Report Image**, then click **Save** |

**Allowed file types:** `.bmp`, `.jpg`, `.jpeg`, `.gif`, `.png`

Uploading an invalid file type shows an error in the modal and blocks save until
you choose a supported image or clear the upload.

**Tip:** Click **Clear Report Image** before **Save** to remove the image from
the database. Clearing only hides the preview until you save.

### Created and Last Updated

Below the tabs, the modal footer area shows:

- **Created** — date and time the agency record was first inserted
- **Last Updated** — date and time of the most recent save

---

## 6. Advanced tab

The **Advanced** tab shows **agency-level user-defined fields (UDFs)** configured
in Codepal. Fields are grouped under category headings and rendered dynamically
based on your agency's UDF configuration.

### Field types you may see

| Control type | How it works |
|--------------|--------------|
| **Text box** | Free-text entry (default for most field types) |
| **Drop-down list** | Choose one option from a predefined list |
| **Check box list** | Select one or more options from a predefined list |

### Empty state

If no agency UDF categories are configured, the tab displays:

*There are no additional fields defined for this record.*

This matches the LegacyApp behavior. Contact your Codepal administrator if you
expect fields here that are not shown.

### Required fields

Some UDF fields may be marked **required** in Codepal. If a required field is
blank when you click **Save**, the modal shows an error such as:

*Required field '{field name}' must be completed.*

Complete all required fields on the **Advanced** tab before saving.

---

## 7. Support Email tab

The **Support Email** tab configures who receives email when users submit the
**Technical Support** and **Fire Services Support** forms from the Support menu
in the navigation bar.

| Field | Purpose |
|-------|---------|
| **Technical Support Email** | Recipients for technical / IT support requests |
| **Fire Services Support Email** | Recipients for fire services program questions |

### Format

- Enter one or more email addresses.
- Separate multiple addresses with **semicolons** (`;`).
- Example: `admin@example.gov; helpdesk@example.gov`

### Blank fields

Leave a field blank to use the **web.config fallback** address configured by
your application deploy team. Blank does not disable support—it defers to the
default in application configuration.

### Validation

Each semicolon-separated address is validated when you save. Invalid addresses
show an error in the modal, for example:

*Technical Support Email: invalid email 'not-an-email'.*

---

## 8. Saving changes

1. Edit fields on any tab (**General**, **Advanced**, or **Support Email**).
2. Click **Save** in the modal footer.
3. On success:
   - Green message above the page title: *Agency information has been saved.*
   - Modal reopens with refreshed data (including updated **Last Updated** time).
4. On error:
   - Red alert at the top of the modal describes the problem.
   - Modal stays open; fix the issue and click **Save** again.

**Save writes all tabs in one action** — agency contact fields, report image
(upload or clear), UDF values, and support email settings are persisted together.

---

## 9. Closing without saving

Click **Close** in the modal footer to navigate to **Current Apps**
(`/Admin/Home`).

**Close does not save** changes made since the last successful save. If you need
to keep edits, click **Save** first.

You can also dismiss the modal with the **×** button in the header; this closes
the modal visually but leaves you on the Manage Agency Information page. Use
**Close** to return to the admin home page.

---

## 10. Common errors and what to do

| Message / situation | Likely cause | What to do |
|---------------------|--------------|------------|
| *Agency session is missing. Please log in again.* | Session expired or `AgencyId` not set | Sign out and sign in again |
| *Agency record was not found.* | Database record missing for session agency | Contact your application administrator |
| *Report image must be a .bmp, .jpg, .jpeg, .gif, or .png file.* | Unsupported upload type | Choose a supported image format |
| *Required field '…' must be completed.* | Blank required UDF on Advanced tab | Fill in the named field |
| *Technical Support Email: invalid email '…'* | Malformed address in support list | Correct the email; use semicolons between multiple addresses |
| *Fire Services Support Email: invalid email '…'* | Malformed address in support list | Same as above |
| *Unable to save agency information.* | Database or service error on agency update | Retry; contact administrator if it persists |
| *Unable to save support email settings.* | Settings table update failed | Retry; contact administrator if it persists |
| Redirect to **Unauthorized** | Not a web admin or external user | Sign in with an admin account |

---

## 11. What this page does not do

Understanding these limits avoids unexpected results:

- Does **not** provide a list of agencies or let you switch agencies—you edit
  only the agency tied to your login
- Does **not** include the LegacyApp **Certifications** tab
- Does **not** show audit history or change logs
- Does **not** manage county fields (omitted from the web form, consistent with
  legacy behavior)
- Does **not** update fire department (FDID) or applicant records—use **Manage
  FDIDs / Related Addresses** for department master data

---

## 12. Recommended workflows

### Update agency contact information for reports

1. Open **Admin** → **Manage Agency Information**.
2. On the **General** tab, update **Name**, address, phone, and **E-mail**.
3. Click **Save**.
4. Open a sample grant report (denial letter or award letter) and confirm the
   agency block reflects the new information.

### Replace the report logo/image

1. Open the modal → **General** tab.
2. Under **Report Image**, choose a `.png` or `.jpg` file (150×150 or similar
   works well for thumbnails).
3. Click **Save**.
4. Preview a printed report to confirm the image appears correctly.

### Update support email recipients

1. Open the modal → **Support Email** tab.
2. Enter semicolon-separated addresses for **Technical Support Email** and/or
   **Fire Services Support Email**.
3. Click **Save**.
4. Submit a test message from the **Support** menu to confirm delivery.

### Configure agency UDFs

1. Open the modal → **Advanced** tab.
2. Complete fields under each category heading.
3. Click **Save**.
4. Verify values appear where agency UDFs are consumed (reports or other
   Codepal integrations as configured).

---

## 13. Quick reference

| Task | Steps |
|------|--------|
| Open agency editor | Admin → Manage Agency Information |
| Edit contact info | General tab → edit fields → Save |
| Upload report image | General tab → Choose File → Save |
| Remove report image | General tab → Clear Report Image → Save |
| Edit UDFs | Advanced tab → edit fields → Save |
| Set support recipients | Support Email tab → enter emails → Save |
| Return to admin home | Close |
| Get help | Click help icon (upper right) |

---

*For technical implementation details, service methods, and database tables, see
the related developer documents linked at the top of this guide.*
