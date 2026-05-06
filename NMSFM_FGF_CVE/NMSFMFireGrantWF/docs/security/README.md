# Security Workflow

This repository runs dependency vulnerability checks as part of `.\build.ps1`.

## What build does

- Queries OSV for discovered NuGet package versions.
- Audits selected third-party binaries and tooling rules (including Telerik checks).
- Applies the configured fail threshold from `DependencyAuditFailLevel`.
- Supports temporary suppressions through `DependencyAuditSuppressVulnIds`.
- Supports temporary tool/binary suppressions through `ToolAuditSuppressIds`.
- Writes machine-readable and human-readable scan outputs.

## Required governance files

- `docs/security/vulnerability-register.md` is the canonical status register.
- `docs/security/risk-acceptance.md` tracks approved temporary suppressions.
- `docs/security/change-log.md` records remediation activity.
- `docs/security/remediation-evidence/` stores implementation proof.

## Standard run sequence

1. Run `.\build.ps1`.
2. Review failures, warnings, and suppressed findings.
3. Update `vulnerability-register.md`.
4. Add remediation proof under `remediation-evidence/`.
5. Update `risk-acceptance.md` if suppressions are used.
6. Append `change-log.md`.
