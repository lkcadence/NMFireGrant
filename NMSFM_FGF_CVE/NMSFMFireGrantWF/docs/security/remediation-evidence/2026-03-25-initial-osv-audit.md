# Remediation Evidence - 2026-03-25 Initial OSV Audit

## Context

- Command: `.\build.ps1`
- Threshold: `High`
- Result: `Fail`
- Source artifacts:
  - `artifacts/security/scans/20260325_181737_osv_dependency-audit.json`
  - `artifacts/security/scans/20260325_181737_osv_dependency-audit.md`

## Findings Snapshot

- Failures (`High`):
  - `GHSA-25c8-p796-jg6r` - `Microsoft.AspNet.Identity.Owin 2.2.3`
  - `GHSA-5crp-9r3c-p9vr` - `Newtonsoft.Json 12.0.2`
  - `GHSA-3rq8-h3gj-r5c6` - `Microsoft.Owin.Security.Cookies 4.0.1`
  - `GHSA-3rq8-h3gj-r5c6` - `Microsoft.Owin 4.0.1`
  - `GHSA-hxrm-9w7p-39cc` - `Microsoft.Owin 4.0.1`
- Warnings (`Moderate`):
  - `GHSA-jpcq-cgw6-v4j6` - `jQuery 3.4.1`

## Decision and Next Action

- Build gate remains enforced with no suppressions.
- Remediation is tracked in `docs/security/vulnerability-register.md`.
- If temporary suppression is required, add an approved row in
  `docs/security/risk-acceptance.md` first.

## Remediation Outcome

- Upgraded packages in `NMSFMFireGrantWF/packages.config`:
  - OWIN family from `4.0.1` to `4.2.2`
  - `Microsoft.AspNet.Identity.*` from `2.2.3` to `2.2.4`
  - `Newtonsoft.Json` from `12.0.2` to `13.0.3`
  - `jQuery` and `AspNet.ScriptManager.jQuery` from `3.4.1` to `3.7.1`
- Updated audit scope in `build.ps1` to exclude backup project paths during
  manifest discovery to keep scan results aligned to active code.
- Verification run:
  - `artifacts/security/scans/20260325_182751_osv_dependency-audit.json`
  - `artifacts/security/scans/20260325_182751_osv_dependency-audit.md`
  - Result: `pass` with `all=0, suppressed=0, warnings=0, failures=0`
