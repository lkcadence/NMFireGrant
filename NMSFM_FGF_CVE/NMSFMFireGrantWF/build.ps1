# Build NMSFMFireGrantWF.sln from Cursor (no Visual Studio required).
# Run: .\build.ps1   or   .\build.ps1 -Configuration Release

param(
  [string]$Configuration = 'Debug',
  [string]$SolutionPath = $PSScriptRoot,
  [switch]$SkipToolingAudit,
  [switch]$SkipDependencyAudit,
  [ValidateSet('Critical', 'High', 'Moderate', 'Low')]
  [string]$DependencyAuditFailLevel = 'High',
  [string[]]$DependencyAuditSuppressVulnIds = @(),
  [string[]]$ToolAuditSuppressIds = @('CVE-2025-3600', 'CVE-2026-2878'),
  [string]$SecurityScansPath = (Join-Path $PSScriptRoot 'artifacts\security\scans'),
  [string]$SecurityEvidenceLatestPath = (Join-Path $PSScriptRoot 'docs\security\remediation-evidence\latest-scan-artifacts.md')
)

function Get-SeverityLabel {
  param([int]$Rank);

  switch ($Rank) {
    4 { return 'critical'; }
    3 { return 'high'; }
    2 { return 'moderate'; }
    default { return 'low'; }
  }
}

function Get-OsvVulnSeverityRank {
  param([object]$Vuln);

  if ($null -eq $Vuln) {
    return 2;
  }

  $severityHints = New-Object System.Collections.Generic.List[string];

  if ($Vuln.PSObject.Properties['database_specific'] -and
      $Vuln.database_specific.PSObject.Properties['severity']) {
    $severityHints.Add([string]$Vuln.database_specific.severity);
  }

  if ($Vuln.PSObject.Properties['affected']) {
    foreach ($affected in @($Vuln.affected)) {
      if ($affected.PSObject.Properties['database_specific'] -and
          $affected.database_specific.PSObject.Properties['severity']) {
        $severityHints.Add([string]$affected.database_specific.severity);
      }
    }
  }

  foreach ($hint in $severityHints) {
    $value = $hint.ToLowerInvariant();
    if ($value -match 'critical') { return 4; }
    if ($value -match 'high') { return 3; }
    if ($value -match 'moderate|medium') { return 2; }
    if ($value -match 'low') { return 1; }
  }

  if ($Vuln.PSObject.Properties['severity']) {
    foreach ($sev in @($Vuln.severity)) {
      if ($sev.PSObject.Properties['score']) {
        $scoreText = [string]$sev.score;
        if ($scoreText -match '([0-9]+(\.[0-9]+)?)') {
          $numeric = [double]$matches[1];
          if ($numeric -ge 9.0) { return 4; }
          if ($numeric -ge 7.0) { return 3; }
          if ($numeric -ge 4.0) { return 2; }
          return 1;
        }
      }
    }
  }

  $combinedText = @([string]$Vuln.summary, [string]$Vuln.details) -join ' ';
  $combinedText = $combinedText.ToLowerInvariant();
  if ($combinedText -match '\bcritical\b') { return 4; }
  if ($combinedText -match '\bhigh\b') { return 3; }
  if ($combinedText -match '\bmoderate\b|\bmedium\b') { return 2; }
  if ($combinedText -match '\blow\b') { return 1; }

  return 2;
}

function Write-DependencyAuditEvidence {
  param(
    [string]$ScansPath,
    [string]$LatestEvidencePath,
    [int]$FailAtRank,
    [string[]]$SuppressVulnIds,
    [object[]]$AllFindings,
    [object[]]$SuppressedFindings,
    [object[]]$WarningFindings,
    [object[]]$FailureFindings
  );

  $null = New-Item -ItemType Directory -Path $ScansPath -Force;
  $latestDir = Split-Path -Parent $LatestEvidencePath;
  $null = New-Item -ItemType Directory -Path $latestDir -Force;

  $timestamp = (Get-Date).ToUniversalTime().ToString('yyyyMMdd_HHmmss');
  $thresholdLabel = Get-SeverityLabel -Rank $FailAtRank;
  $result = if (@($FailureFindings).Count -gt 0) { 'fail' } else { 'pass' };

  $jsonObj = [ordered]@{
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString('o');
    threshold = $thresholdLabel;
    suppressedIds = @($SuppressVulnIds | Sort-Object -Unique);
    counts = [ordered]@{
      all = @($AllFindings).Count;
      suppressed = @($SuppressedFindings).Count;
      warnings = @($WarningFindings).Count;
      failures = @($FailureFindings).Count;
    };
    result = $result;
    findings = @($AllFindings);
  };

  $jsonPath = Join-Path $ScansPath "${timestamp}_osv_dependency-audit.json";
  $mdPath = Join-Path $ScansPath "${timestamp}_osv_dependency-audit.md";
  $jsonObj | ConvertTo-Json -Depth 8 | Set-Content -Path $jsonPath -Encoding UTF8;

  $md = New-Object System.Collections.Generic.List[string];
  $md.Add('# OSV Dependency Audit');
  $md.Add('');
  $md.Add("- Generated (UTC): $($jsonObj.generatedAtUtc)");
  $md.Add("- Threshold: $thresholdLabel");
  $md.Add("- Result: $result");
  $md.Add("- Counts: all=$(@($AllFindings).Count), suppressed=$(@($SuppressedFindings).Count), warnings=$(@($WarningFindings).Count), failures=$(@($FailureFindings).Count)");
  $md.Add('');

  if (@($FailureFindings).Count -gt 0) {
    $md.Add('## Failures');
    foreach ($f in @($FailureFindings)) {
      $md.Add("- [$($f.vulnId)]($($f.url)) | $($f.severity) | $($f.packageName) $($f.packageVersion)");
    }
    $md.Add('');
  }

  if (@($WarningFindings).Count -gt 0) {
    $md.Add('## Warnings');
    foreach ($f in @($WarningFindings)) {
      $md.Add("- [$($f.vulnId)]($($f.url)) | $($f.severity) | $($f.packageName) $($f.packageVersion)");
    }
    $md.Add('');
  }

  if (@($SuppressedFindings).Count -gt 0) {
    $md.Add('## Suppressed');
    foreach ($f in @($SuppressedFindings)) {
      $md.Add("- [$($f.vulnId)]($($f.url)) | $($f.severity) | $($f.packageName) $($f.packageVersion)");
    }
    $md.Add('');
  }

  if (@($AllFindings).Count -eq 0) {
    $md.Add('No vulnerabilities found.');
  }

  $md | Set-Content -Path $mdPath -Encoding UTF8;

  $pointer = @(
    '# Latest Scan Artifacts',
    '',
    "- Generated (UTC): $($jsonObj.generatedAtUtc)",
    "- JSON: ``$jsonPath``",
    "- Markdown: ``$mdPath``",
    "- Result: $result",
    "- Threshold: $thresholdLabel"
  );
  $pointer | Set-Content -Path $LatestEvidencePath -Encoding UTF8;
}

function Invoke-NMSFMFireGrantWFToolingAudit {
  param(
    [string]$RepoRoot,
    [int]$FailAtRank,
    [string[]]$SuppressIds,
    [string]$ScansPath
  );

  $null = New-Item -ItemType Directory -Path $ScansPath -Force;
  $timestamp = (Get-Date).ToUniversalTime().ToString('yyyyMMdd_HHmmss');

  $suppressionSet = @{};
  foreach ($id in @($SuppressIds)) {
    $suppressionSet[$id.ToUpperInvariant()] = $true;
  }

  $findings = New-Object System.Collections.Generic.List[object];
  $rules = @(
    @{
      id = 'CVE-2025-3600';
      description = 'Telerik Web Forms unsafe reflection vulnerability.';
      minSafeVersion = [version]'2025.1.416.0';
      severityRank = 3;
      url = 'https://www.telerik.com/products/aspnet-ajax/documentation/knowledge-base/kb-security-unsafe-reflection-cve-2025-3600';
    },
    @{
      id = 'CVE-2026-2878';
      description = 'Telerik Web Forms insufficient entropy vulnerability.';
      minSafeVersion = [version]'2026.1.225.0';
      severityRank = 3;
      url = 'https://www.telerik.com/products/aspnet-ajax/documentation/knowledge-base/kb-security-insufficient-entropy-cve-2026-2878';
    }
  );

  $csproj = Join-Path $RepoRoot 'NMSFMFireGrantWF\NMSFMFireGrantWF.csproj';
  if (Test-Path $csproj) {
    $content = Get-Content -Path $csproj -Raw;
    $match = [regex]::Match($content, 'Telerik\.Web\.UI,\s*Version=([0-9]+\.[0-9]+\.[0-9]+\.[0-9]+)');
    if ($match.Success) {
      $telerikVersion = [version]$match.Groups[1].Value;
      foreach ($rule in $rules) {
        if ($telerikVersion -lt $rule.minSafeVersion) {
          $status = if ($suppressionSet.ContainsKey($rule.id.ToUpperInvariant())) {
            'suppressed';
          } elseif ($rule.severityRank -ge $FailAtRank) {
            'failure';
          } else {
            'warning';
          }
          $findings.Add([ordered]@{
              vulnId = $rule.id;
              component = 'Telerik.Web.UI';
              installedVersion = $telerikVersion.ToString();
              minimumFixedVersion = $rule.minSafeVersion.ToString();
              severity = (Get-SeverityLabel -Rank $rule.severityRank);
              severityRank = $rule.severityRank;
              summary = $rule.description;
              url = $rule.url;
              status = $status;
            });
        }
      }
    }
  }

  $dotnetVersion = '';
  try { $dotnetVersion = (& dotnet --version).Trim() } catch { $dotnetVersion = 'unavailable' }
  $msbuildVersion = '';
  try {
    $msbuildPath = Get-Command msbuild -ErrorAction Stop;
    $msbuildVersion = (Get-Item $msbuildPath.Source).VersionInfo.FileVersion;
  } catch {
    $msbuildVersion = 'unavailable';
  }

  $all = @($findings.ToArray());
  $suppressed = @($all | Where-Object { $_.status -eq 'suppressed' });
  $warnings = @($all | Where-Object { $_.status -eq 'warning' });
  $failures = @($all | Where-Object { $_.status -eq 'failure' });
  $result = if ($failures.Count -gt 0) { 'fail' } else { 'pass' };

  Write-Host '';
  Write-Host '=== Tooling And Binary Audit ===' -ForegroundColor Cyan;
  Write-Host "Threshold: $(Get-SeverityLabel -Rank $FailAtRank)";
  Write-Host "Environment: dotnet=$dotnetVersion, msbuild=$msbuildVersion";
  Write-Host "Findings: all=$($all.Count), suppressed=$($suppressed.Count), warnings=$($warnings.Count), failures=$($failures.Count)";

  if ($failures.Count -gt 0) {
    Write-Host 'Failures:' -ForegroundColor Red;
    foreach ($f in $failures) {
      Write-Host "  - $($f.vulnId) | $($f.component) | $($f.installedVersion) < fixed $($f.minimumFixedVersion)";
    }
  }

  if ($suppressed.Count -gt 0) {
    Write-Host 'Suppressed:' -ForegroundColor DarkYellow;
    foreach ($f in $suppressed) {
      Write-Host "  - $($f.vulnId) | $($f.component) | $($f.installedVersion)";
    }
  }

  $jsonObj = [ordered]@{
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString('o');
    threshold = (Get-SeverityLabel -Rank $FailAtRank);
    suppressedIds = @($SuppressIds | Sort-Object -Unique);
    environment = [ordered]@{
      dotnet = $dotnetVersion;
      msbuild = $msbuildVersion;
    };
    counts = [ordered]@{
      all = $all.Count;
      suppressed = $suppressed.Count;
      warnings = $warnings.Count;
      failures = $failures.Count;
    };
    result = $result;
    findings = $all;
  };

  $jsonPath = Join-Path $ScansPath "${timestamp}_tooling-audit.json";
  $mdPath = Join-Path $ScansPath "${timestamp}_tooling-audit.md";
  $jsonObj | ConvertTo-Json -Depth 8 | Set-Content -Path $jsonPath -Encoding UTF8;

  $md = New-Object System.Collections.Generic.List[string];
  $md.Add('# Tooling And Binary Audit');
  $md.Add('');
  $md.Add("- Generated (UTC): $($jsonObj.generatedAtUtc)");
  $md.Add("- Threshold: $($jsonObj.threshold)");
  $md.Add("- Result: $result");
  $md.Add("- Environment: dotnet=$dotnetVersion, msbuild=$msbuildVersion");
  $md.Add('');
  foreach ($f in $all) {
    $md.Add("- [$($f.vulnId)]($($f.url)) | $($f.component) | installed=$($f.installedVersion) | fixed=$($f.minimumFixedVersion) | $($f.status)");
  }
  if ($all.Count -eq 0) {
    $md.Add('No tooling/binary findings for configured rules.');
  }
  $md | Set-Content -Path $mdPath -Encoding UTF8;

  return ($failures.Count -eq 0);
}

function Invoke-NMSFMFireGrantWFDependencyAudit {
  param(
    [string]$RepoRoot,
    [int]$FailAtRank,
    [string[]]$SuppressVulnIds,
    [string]$ScansPath,
    [string]$LatestEvidencePath
  );

  $packages = @{};
  $isAuditablePath = {
    param([string]$Path);

    if (-not $Path) {
      return $false;
    }

    if ($Path -match '\\NMSFMFireGrantWF_Backup_') {
      return $false;
    }

    return $true;
  };

  $packageConfigFiles = Get-ChildItem -Path $RepoRoot -Recurse -Filter 'packages.config' -File `
    -ErrorAction SilentlyContinue | Where-Object { & $isAuditablePath $_.FullName };
  foreach ($file in $packageConfigFiles) {
    try {
      [xml]$xml = Get-Content -Path $file.FullName -Raw;
      foreach ($pkg in @($xml.packages.package)) {
        $name = [string]$pkg.id;
        $version = [string]$pkg.version;
        if ($name -and $version) {
          $packages["$name|$version"] = [ordered]@{ name = $name; version = $version };
        }
      }
    } catch {
      Write-Warning "Could not parse packages.config: $($file.FullName)";
    }
  }

  $csprojFiles = Get-ChildItem -Path $RepoRoot -Recurse -Filter '*.csproj' -File `
    -ErrorAction SilentlyContinue | Where-Object { & $isAuditablePath $_.FullName };
  foreach ($file in $csprojFiles) {
    try {
      [xml]$xml = Get-Content -Path $file.FullName -Raw;
      $nodes = $xml.SelectNodes("//*[local-name()='PackageReference']");
      foreach ($node in @($nodes)) {
        $name = [string]$node.Include;
        $version = [string]$node.Version;
        if (-not $version -and $node.PSObject.Properties['InnerText']) {
          $version = [string]$node.InnerText;
        }
        if ($name -and $version) {
          $packages["$name|$version"] = [ordered]@{ name = $name; version = $version };
        }
      }
    } catch {
      Write-Warning "Could not parse csproj: $($file.FullName)";
    }
  }

  $packageList = @($packages.Values);
  if ($packageList.Count -eq 0) {
    Write-Host 'No NuGet package manifests discovered for dependency audit.' -ForegroundColor Yellow;
    Write-DependencyAuditEvidence `
      -ScansPath $ScansPath `
      -LatestEvidencePath $LatestEvidencePath `
      -FailAtRank $FailAtRank `
      -SuppressVulnIds $SuppressVulnIds `
      -AllFindings @() `
      -SuppressedFindings @() `
      -WarningFindings @() `
      -FailureFindings @();
    return $true;
  }

  $queryBody = @{
    queries = @(
      foreach ($p in $packageList) {
        @{
          package = @{
            name = $p.name;
            ecosystem = 'NuGet';
          };
          version = $p.version;
        }
      }
    );
  } | ConvertTo-Json -Depth 8;

  $batchUrl = 'https://api.osv.dev/v1/querybatch';
  try {
    $batchResponse = Invoke-RestMethod -Method Post -Uri $batchUrl -Body $queryBody `
      -ContentType 'application/json';
  } catch {
    Write-Error "Dependency audit failed: unable to query OSV batch API. $($_.Exception.Message)";
    return $false;
  }

  $suppressionSet = @{};
  foreach ($id in @($SuppressVulnIds)) {
    $suppressionSet[$id.ToUpperInvariant()] = $true;
  }

  $allFindings = New-Object System.Collections.Generic.List[object];
  $suppressedFindings = New-Object System.Collections.Generic.List[object];
  $warningFindings = New-Object System.Collections.Generic.List[object];
  $failureFindings = New-Object System.Collections.Generic.List[object];

  for ($i = 0; $i -lt $packageList.Count; $i++) {
    $pkg = $packageList[$i];
    $result = $batchResponse.results[$i];
    if ($null -eq $result -or -not $result.vulns) {
      continue;
    }

    foreach ($vuln in @($result.vulns)) {
      $vulnId = [string]$vuln.id;
      if (-not $vulnId) { continue; }

      $detail = $null;
      $detailUrl = "https://api.osv.dev/v1/vulns/$vulnId";
      try {
        $detail = Invoke-RestMethod -Method Get -Uri $detailUrl;
      } catch {
        $detail = $vuln;
      }

      $rank = Get-OsvVulnSeverityRank -Vuln $detail;
      $severity = Get-SeverityLabel -Rank $rank;
      $isSuppressed = $suppressionSet.ContainsKey($vulnId.ToUpperInvariant());
      $status = if ($isSuppressed) {
        'suppressed';
      } elseif ($rank -ge $FailAtRank) {
        'failure';
      } else {
        'warning';
      }

      $finding = [ordered]@{
        vulnId = $vulnId;
        packageName = $pkg.name;
        packageVersion = $pkg.version;
        severity = $severity;
        severityRank = $rank;
        summary = [string]$detail.summary;
        url = $detailUrl;
        status = $status;
      };
      $allFindings.Add($finding);

      switch ($status) {
        'suppressed' { $suppressedFindings.Add($finding); }
        'failure' { $failureFindings.Add($finding); }
        default { $warningFindings.Add($finding); }
      }
    }
  }

  Write-Host '';
  Write-Host '=== Dependency Vulnerability Audit (OSV) ===' -ForegroundColor Cyan;
  Write-Host "Threshold: $(Get-SeverityLabel -Rank $FailAtRank)";
  Write-Host "Findings: all=$($allFindings.Count), suppressed=$($suppressedFindings.Count), warnings=$($warningFindings.Count), failures=$($failureFindings.Count)";

  if ($failureFindings.Count -gt 0) {
    Write-Host '';
    Write-Host 'Failures:' -ForegroundColor Red;
    foreach ($f in $failureFindings) {
      Write-Host "  - $($f.vulnId) | $($f.severity) | $($f.packageName) $($f.packageVersion)";
    }
  }

  if ($warningFindings.Count -gt 0) {
    Write-Host '';
    Write-Host 'Warnings:' -ForegroundColor Yellow;
    foreach ($f in $warningFindings) {
      Write-Host "  - $($f.vulnId) | $($f.severity) | $($f.packageName) $($f.packageVersion)";
    }
  }

  if ($suppressedFindings.Count -gt 0) {
    Write-Host '';
    Write-Host 'Suppressed:' -ForegroundColor DarkYellow;
    foreach ($f in $suppressedFindings) {
      Write-Host "  - $($f.vulnId) | $($f.severity) | $($f.packageName) $($f.packageVersion)";
    }
  }

  Write-DependencyAuditEvidence `
    -ScansPath $ScansPath `
    -LatestEvidencePath $LatestEvidencePath `
    -FailAtRank $FailAtRank `
    -SuppressVulnIds $SuppressVulnIds `
    -AllFindings $allFindings `
    -SuppressedFindings $suppressedFindings `
    -WarningFindings $warningFindings `
    -FailureFindings $failureFindings;

  return ($failureFindings.Count -eq 0);
}

$sln = Join-Path $SolutionPath 'NMSFMFireGrantWF.sln'
if (-not (Test-Path $sln)) {
  Write-Error "Solution not found: $sln"
  exit 1
}

if (-not $SkipDependencyAudit) {
  $failRank = switch ($DependencyAuditFailLevel) {
    'Critical' { 4 }
    'High' { 3 }
    'Moderate' { 2 }
    'Low' { 1 }
    default { 3 }
  }

  if (-not $SkipToolingAudit) {
    $toolAuditOk = Invoke-NMSFMFireGrantWFToolingAudit `
      -RepoRoot $SolutionPath `
      -FailAtRank $failRank `
      -SuppressIds $ToolAuditSuppressIds `
      -ScansPath $SecurityScansPath;
    if (-not $toolAuditOk) {
      Write-Host '';
      Write-Host 'Tooling and binary audit failed the configured threshold. Build stopped.' -ForegroundColor Red;
      exit 1;
    }
  }

  $auditOk = Invoke-NMSFMFireGrantWFDependencyAudit `
    -RepoRoot $SolutionPath `
    -FailAtRank $failRank `
    -SuppressVulnIds $DependencyAuditSuppressVulnIds `
    -ScansPath $SecurityScansPath `
    -LatestEvidencePath $SecurityEvidenceLatestPath;

  if (-not $auditOk) {
    Write-Host '';
    Write-Host 'Dependency audit failed the configured threshold. Build stopped.' -ForegroundColor Red;
    exit 1;
  }
}

$msbuild = $null
$standardPaths = @(
  "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
  "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
  "${env:ProgramFiles}\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
  "${env:ProgramFiles}\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe",
  "${env:ProgramFiles}\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
  "${env:ProgramFiles}\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
)
foreach ($path in $standardPaths) {
  if (Test-Path $path) { $msbuild = $path; break }
}
if (-not $msbuild) {
  $vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
  if (Test-Path $vswhere) {
    $installPath = & $vswhere -latest -requires Microsoft.Component.MSBuild -property installationPath 2>$null
    if ($installPath) {
      $candidate = Join-Path $installPath "MSBuild\Current\Bin\MSBuild.exe"
      if (Test-Path $candidate) { $msbuild = $candidate }
    }
  }
}

if (-not $msbuild) {
  Write-Error @"
MSBuild not found. Install one of:
  - Visual Studio 2019/2022 (any edition) with .NET desktop build tools
  - Build Tools for Visual Studio (https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2022)
Then run this script again.
"@
  exit 1
}

# Prefer running under VS Developer environment so MSBuild loads correctly (avoids exit -2146233082).
$vsDevCmd = $null
$vsRoot = Split-Path (Split-Path (Split-Path (Split-Path $msbuild)))
$vsDevCmd = Join-Path $vsRoot "Common7\Tools\VsDevCmd.bat"
$useVsDev = (Test-Path $vsDevCmd)

if ($useVsDev) {
  Write-Host "Using: VS Developer environment + MSBuild" -ForegroundColor Cyan
  Write-Host "Building: $sln (Configuration=$Configuration)"
  $buildCmd = "call `"$vsDevCmd`" -no_logo >nul && cd /d `"$SolutionPath`" && `"$msbuild`" `"$sln`" /p:Configuration=$Configuration /t:Build /v:normal"
  cmd /c $buildCmd
} else {
  Write-Host "Using: $msbuild"
  Write-Host "Building: $sln (Configuration=$Configuration)"
  & $msbuild $sln /p:Configuration=$Configuration /t:Build /v:normal 2>&1
}
$exitCode = $LASTEXITCODE
if ($exitCode -eq -2146233082 -and $useVsDev) {
  $vstoolsPath = Join-Path $vsRoot "MSBuild\Microsoft\VisualStudio\v17.0"
  if (-not (Test-Path $vstoolsPath)) { $vstoolsPath = Join-Path $vsRoot "MSBuild\Microsoft\VisualStudio\v16.0" }
  $refRoot = "${env:ProgramFiles(x86)}\Reference Assemblies\Microsoft\Framework\.NETFramework"
  if ((Test-Path $vstoolsPath) -and (Test-Path $refRoot)) {
    Write-Host ""
    Write-Host "Retrying with dotnet msbuild (VS MSBuild failed to start)..." -ForegroundColor Cyan
    $env:VSToolsPath = $vstoolsPath
    $env:VisualStudioVersion = if (Test-Path (Join-Path $vsRoot "MSBuild\Microsoft\VisualStudio\v17.0")) { "17.0" } else { "16.0" }
    $refRoot = "${env:ProgramFiles(x86)}\Reference Assemblies\Microsoft\Framework"
    $frameworkPath = "$refRoot\.NETFramework\v4.7.1"
    $webProject = Join-Path $SolutionPath 'NMSFMFireGrantWF\NMSFMFireGrantWF.csproj'
    & dotnet msbuild $webProject "/p:DotNetBuild=true" "/p:VSToolsPath=$vstoolsPath" "/p:VisualStudioVersion=$env:VisualStudioVersion" "/p:TargetFrameworkRootPath=$refRoot" "/p:FrameworkPathOverride=$frameworkPath" /p:Configuration=$Configuration /t:Build /v:minimal 2>&1
    $exitCode = $LASTEXITCODE
    Remove-Item Env:VSToolsPath -ErrorAction SilentlyContinue
    Remove-Item Env:VisualStudioVersion -ErrorAction SilentlyContinue
  }
}
if ($exitCode -ne 0) {
  Write-Host ""
  Write-Host "Build failed (exit code $exitCode). Fix errors above, then run .\build.ps1 again." -ForegroundColor Red
  if ($exitCode -eq -2146233082) {
    Write-Host "MSBuild could not start. Try: run from 'Developer PowerShell for VS 2022' (Start menu), or repair Visual Studio." -ForegroundColor Yellow
  }
  if ($exitCode -eq 1) {
    Write-Host "If you see MSB3644 or 'mscorlib.dll could not be found': install .NET Framework 4.7.1 Developer Pack (https://dotnet.microsoft.com/download/dotnet-framework/net471)." -ForegroundColor Yellow
  }
  exit $exitCode
}
Write-Host "Build succeeded." -ForegroundColor Green
