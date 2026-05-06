# Vulnerability Change Log

Track vulnerability remediation and governance updates.

| Date (UTC) | Change | Owner | Evidence |
| --- | --- | --- | --- |
| 2026-03-25 | Initialized vulnerability workflow artifacts and OSV pre-build audit gate | App Team | `docs/security/remediation-evidence/latest-scan-artifacts.md` |
| 2026-03-25 | Recorded initial findings in vulnerability register and remediation evidence | App Team | `docs/security/remediation-evidence/2026-03-25-initial-osv-audit.md` |
| 2026-03-25 | Remediated OWIN, Identity, Newtonsoft.Json, and jQuery findings; audit now passes with zero findings | App Team | `artifacts/security/scans/20260325_182751_osv_dependency-audit.md` |
| 2026-03-25 | Added tooling and third-party binary audit (Telerik + build environment inventory) to `build.ps1` with suppression governance | App Team | `artifacts/security/scans/*_tooling-audit.md` |
