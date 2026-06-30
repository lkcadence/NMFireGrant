# NMSFM Fire Grant Application
## Web.config Email Configuration — Instructions for NM DoIT

**Document purpose:** Update the deployed site's `Web.config` for the new outbound email configuration.

**Scope:** Email settings only. All other existing `Web.config` settings (connection strings, `ApplicationUrl`, etc.) should remain unchanged.

**Reference:** Same email relay pattern as the NMSFM Fire Fund application.

---

## 1. Replace the SMTP section

Remove any SendGrid, Office 365, or other third-party SMTP settings. Replace the `<system.net>` mail section with:

```xml
<system.net>
  <mailSettings>
    <smtp from="donotreply@fireservicesgrant.dhsem.nm.gov">
      <network defaultCredentials="true"
               host="webmail.state.nm.us"
               port="25"
               enableSsl="true" />
    </smtp>
  </mailSettings>
</system.net>
```

**Notes:**

- Do not add `userName` or `password` attributes.
- The IIS application pool identity must be permitted to relay mail through `webmail.state.nm.us` (port 25), consistent with Fire Fund.

---

## 2. Update required app settings

Add or update these keys in `<appSettings>`:

```xml
<add key="DefaultEmailSender" value="donotreply@fireservicesgrant.dhsem.nm.gov" />
<add key="EnableEmailSendLogging" value="true" />
<add key="EmailSendLogRetentionDays" value="90" />
```

| Setting | Purpose |
|---------|---------|
| `DefaultEmailSender` | From address on all application emails |
| `EnableEmailSendLogging` | Records each outbound email for the admin Email Send Log |
| `EmailSendLogRetentionDays` | Retention period used when purging old log entries (days) |

These logging settings are **required**, not optional. If `EnableEmailSendLogging` is not `true`, new sends will not appear in **Admin → Email Send Log**.

---

## 3. Post-deploy verification

1. Sign in as an application administrator.
2. Open **Admin → Email Send Log**.
3. Send a test email to a valid address.
4. Confirm the log shows status **Sent** (or **Failed** with an error message if relay access needs adjustment).

---

## Summary

| Item | Required value / action |
|------|-------------------------|
| SMTP host | `webmail.state.nm.us` |
| SMTP port | `25` |
| SSL | `true` |
| Credentials | `defaultCredentials="true"` (no username/password in config) |
| From address | `donotreply@fireservicesgrant.dhsem.nm.gov` |
| `DefaultEmailSender` | `donotreply@fireservicesgrant.dhsem.nm.gov` |
| `EnableEmailSendLogging` | `true` |
| `EmailSendLogRetentionDays` | `90` |
| Remove | SendGrid or other third-party SMTP configuration |

No other `Web.config` changes are required for outbound email in this release.
