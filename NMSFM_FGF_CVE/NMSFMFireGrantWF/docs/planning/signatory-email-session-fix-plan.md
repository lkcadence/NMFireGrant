# Signatory Email Session Fix — Planning Document

> **Detailed implementation guide:**
> [`../signatory-email-session-fix-implementation-plan.md`](../signatory-email-session-fix-implementation-plan.md)

**Project:** NMSFM Fire Grant Web Application  
**Document version:** 1.0  
**Date:** June 30, 2026  
**Status:** Implemented

**Related artifacts:**

- Page: [`SignaturesDocs.aspx.cs`](../../NMSFMFireGrantWF/Application/SignaturesDocs.aspx.cs)
- Master: [`ApplicationMstr.Master.cs`](../../NMSFMFireGrantWF/Application/ApplicationMstr.Master.cs)
- Email context: [`EmailSendContextHelper.cs`](../../../NMSFM.Services/FireGrant/EmailSendContextHelper.cs)
- Prior email work: [`email-send-logging-plan.md`](email-send-logging-plan.md)

---

## Problem

External users clicking **Save & Send Emails** on Signatures and Supporting Docs were redirected to the login page after signatory emails were sent. This was discovered during a production test after the email-send reliability fix (awaiting SMTP before redirect).

The user is **not** logged out during SMTP; the failure occurs on the **redirect GET** immediately afterward when `Session["WebUserId"]` is missing and `ApplicationMstr.Master` calls `Session.Abandon()`.

---

## Decisions

| Topic | Decision |
|-------|----------|
| Save & Send Emails | Show success/error **inline on postback** — no redirect |
| Submit Application | Redirect to AppConf only if session intact after email |
| Async lifecycle | `RegisterAsyncTask` instead of `async void` click handler |
| Email context | Snapshot session values before any `await`; pass explicit `EmailSendContext` |
| Master auth guard | Redirect to login on missing session; **do not** `Abandon`/`SignOut` on guard |
| Scope | Bug fix; no schema or config changes |

---

## Success criteria

1. External user stays on Signatures page after **Save & Send Emails** with success message.
2. User is not prompted to log in unless session truly expired or user logged out.
3. Signatory emails still send and log correctly (`SignatoryRequest`).
4. Submit Application still redirects to AppConf when session is valid.
5. Explicit logout still clears session.
6. `.\build.ps1` succeeds.

---

## Out of scope

- Signatory link auto-login replacing applicant session in same browser
- Background/queued email sending
- Duplicate-email prevention on browser refresh after inline save

---

## Rollback

Revert changes to `SignaturesDocs.aspx.cs`, `EmailSendContextHelper.cs`, `ApplicationMstr.Master.cs`, and `Site.Master.cs`. No database or config rollback.
