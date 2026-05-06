# Risk Acceptance

Every suppression in `build.ps1` must have a matching row here.

| Vulnerability ID | Component | Owner | Reason | Review Date | Exit Criteria | Status |
| --- | --- | --- | --- | --- | --- | --- |
| CVE-2025-3600 | Telerik.Web.UI | App Team | Commercial Telerik upgrade path is pending vendor package acquisition and compatibility test cycle | 2026-04-30 | Upgrade Telerik UI for ASP.NET AJAX to a version that includes vendor fix (>= 2025.1.416) and remove suppression from `build.ps1` | Active |
| CVE-2026-2878 | Telerik.Web.UI | App Team | Current app depends on legacy Telerik line; upgrade requires coordinated web UI regression testing | 2026-04-30 | Upgrade Telerik UI for ASP.NET AJAX to a version that includes vendor fix (>= 2026.1.225) and remove suppression from `build.ps1` | Active |

## Rules

- Suppressions are temporary and must be reviewed regularly.
- Each row needs owner, reason, review date, and explicit exit criteria.
- Remove suppression from `build.ps1` once exit criteria are met.
