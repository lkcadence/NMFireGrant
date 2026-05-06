# Build NMSFMFireGrantWF.sln in Release and create a date-stamped deploy .zip
# Example zip name: NMSFMFireGrantWF_Release_02272026.zip
# Usage from solution folder:
#   .\build-release.ps1
#   .\build-release.ps1 -Configuration Release

param(
  [string]$Configuration = 'Release',
  [string]$SolutionPath = $PSScriptRoot,
  [string]$OutputFolderName = 'publish'
)

$ErrorActionPreference = 'Stop'

# 1) Locate solution
$sln = Join-Path $SolutionPath 'NMSFMFireGrantWF.sln'
if (-not (Test-Path $sln)) {
  Write-Error "Solution not found: $sln"
  exit 1
}

# 2) Find MSBuild
$msbuild = $null

# Preferred standard paths for VS 2022 / 2019
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

# Fallback to vswhere if needed
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
MSBuild not found.

Install ONE of:
  - Visual Studio 2019/2022 (any edition) with ".NET desktop build tools"
  - Build Tools for Visual Studio 2022:
    https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2022

Then run this script again.
"@
  exit 1
}

# 3) Build solution (prefer VS Developer environment, fallback to dotnet msbuild on -2146233082)
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
    $webProjectPath = Join-Path $SolutionPath 'NMSFMFireGrantWF\NMSFMFireGrantWF.csproj'
    & dotnet msbuild $webProjectPath "/p:DotNetBuild=true" "/p:VSToolsPath=$vstoolsPath" "/p:VisualStudioVersion=$env:VisualStudioVersion" "/p:TargetFrameworkRootPath=$refRoot" "/p:FrameworkPathOverride=$frameworkPath" /p:Configuration=$Configuration /t:Build /v:minimal 2>&1
    $exitCode = $LASTEXITCODE
    Remove-Item Env:VSToolsPath -ErrorAction SilentlyContinue
    Remove-Item Env:VisualStudioVersion -ErrorAction SilentlyContinue
  }
}
if ($exitCode -ne 0) {
  Write-Host ""
  Write-Host "Build failed (exit code $exitCode). Fix errors above, then run .\build-release.ps1 again." -ForegroundColor Red
  if ($exitCode -eq -2146233082) {
    Write-Host "MSBuild could not start. Try: run from 'Developer PowerShell for VS 2022' (Start menu), or repair Visual Studio." -ForegroundColor Yellow
  }
  if ($exitCode -eq 1) {
    Write-Host "If you see MSB3644 or 'mscorlib.dll could not be found': install .NET Framework 4.7.1 Developer Pack (https://dotnet.microsoft.com/download/dotnet-framework/net471)." -ForegroundColor Yellow
  }
  exit $exitCode
}

Write-Host "Build succeeded." -ForegroundColor Green

# 4) Publish web output, then create date-stamped zip from publish contents
$deployRoot = Join-Path $SolutionPath $OutputFolderName
if (Test-Path $deployRoot) {
  Write-Host "Cleaning existing deploy folder: $deployRoot"
  Remove-Item -Recurse -Force $deployRoot
}

# Publish to file-system output so deploy zip contains only runtime assets.
$webProjectPath = Join-Path $SolutionPath 'NMSFMFireGrantWF\NMSFMFireGrantWF.csproj'
if (-not (Test-Path $webProjectPath)) {
  Write-Error "Web project not found: $webProjectPath"
  exit 1
}

Write-Host "Publishing web project to: $deployRoot" -ForegroundColor Cyan
if ($useVsDev) {
  $publishCmd = "call `"$vsDevCmd`" -no_logo >nul && cd /d `"$SolutionPath`" && `"$msbuild`" `"$webProjectPath`" /p:Configuration=$Configuration /p:DeployOnBuild=true /p:PublishProfile=FolderProfile /p:publishUrl=`"$deployRoot`" /p:DeleteExistingFiles=true /t:Build /v:minimal"
  cmd /c $publishCmd
} else {
  & $msbuild $webProjectPath /p:Configuration=$Configuration /p:DeployOnBuild=true /p:PublishProfile=FolderProfile /p:publishUrl="$deployRoot" /p:DeleteExistingFiles=true /t:Build /v:minimal 2>&1
}
$publishExitCode = $LASTEXITCODE
if ($publishExitCode -eq -2146233082 -and $useVsDev) {
  $publishVSToolsPath = Join-Path $vsRoot "MSBuild\Microsoft\VisualStudio\v17.0"
  if (-not (Test-Path $publishVSToolsPath)) { $publishVSToolsPath = Join-Path $vsRoot "MSBuild\Microsoft\VisualStudio\v16.0" }
  $publishVsVersion = if (Test-Path (Join-Path $vsRoot "MSBuild\Microsoft\VisualStudio\v17.0")) { "17.0" } else { "16.0" }
  $publishRefRoot = "${env:ProgramFiles(x86)}\Reference Assemblies\Microsoft\Framework"
  $publishFrameworkPath = "$publishRefRoot\.NETFramework\v4.7.1"
  if (Test-Path $publishVSToolsPath) {
    Write-Host "Retrying publish with dotnet msbuild..." -ForegroundColor Cyan
    & dotnet msbuild $webProjectPath "/p:DotNetBuild=true" "/p:VSToolsPath=$publishVSToolsPath" "/p:VisualStudioVersion=$publishVsVersion" "/p:TargetFrameworkRootPath=$publishRefRoot" "/p:FrameworkPathOverride=$publishFrameworkPath" /p:Configuration=$Configuration /p:DeployOnBuild=true /p:PublishProfile=FolderProfile /p:publishUrl="$deployRoot" /p:DeleteExistingFiles=true /t:Build /v:minimal 2>&1
    $publishExitCode = $LASTEXITCODE
  }
}
if ($publishExitCode -ne 0) {
  Write-Host ""
  Write-Host "Publish failed (exit code $publishExitCode)." -ForegroundColor Red
  exit $publishExitCode
}

# 5) Ensure Roslyn runtime compiler files are present in publish output.
$publishRoslynPath = Join-Path $deployRoot 'bin\roslyn'
if (-not (Test-Path $publishRoslynPath)) {
  New-Item -Path $publishRoslynPath -ItemType Directory -Force | Out-Null
}

$roslynSourcePath = $null
$roslynSourceCandidates = @(
  (Join-Path $vsRoot 'MSBuild\Current\Bin\Roslyn'),
  (Join-Path $vsRoot 'MSBuild\Current\Bin\amd64\Roslyn'),
  (Join-Path $vsRoot 'MSBuild\Current\Bin\Roslyn\Roslyn45'),
  (Join-Path $vsRoot 'MSBuild\Current\Bin\Roslyn\RoslynLatest'),
  (Join-Path $vsRoot 'MSBuild\15.0\Bin\Roslyn')
)
foreach ($candidate in $roslynSourceCandidates) {
  if (Test-Path (Join-Path $candidate 'csc.exe')) {
    $roslynSourcePath = $candidate
    break
  }
}

if (-not $roslynSourcePath) {
  Write-Host ""
  Write-Host "Roslyn compiler binaries were not found on this build machine." -ForegroundColor Red
  Write-Host "Expected csc.exe under MSBuild Roslyn folders; cannot create IIS-safe release package." -ForegroundColor Red
  exit 1
}

Write-Host "Copying Roslyn compiler files from: $roslynSourcePath" -ForegroundColor Cyan
Copy-Item (Join-Path $roslynSourcePath '*') $publishRoslynPath -Recurse -Force

$requiredRoslynFiles = @(
  'csc.exe',
  'vbc.exe',
  'csi.exe',
  'VBCSCompiler.exe'
)
$missingRoslynFiles = @()
foreach ($requiredFile in $requiredRoslynFiles) {
  if (-not (Test-Path (Join-Path $publishRoslynPath $requiredFile))) {
    $missingRoslynFiles += $requiredFile
  }
}

if ($missingRoslynFiles.Count -gt 0) {
  Write-Host ""
  Write-Host ("Publish output missing Roslyn files: " + ($missingRoslynFiles -join ', ')) -ForegroundColor Red
  Write-Host "Release zip not created to avoid IIS startup failure." -ForegroundColor Red
  exit 1
}

# Date-stamped zip name: NMSFMFireGrantWF_Release_MMddyyyy.zip
$dateStamp = (Get-Date).ToString('MMddyyyy')
$zipName = "NMSFMFireGrantWF_Release_{0}.zip" -f $dateStamp
$zipPath = Join-Path $SolutionPath $zipName

if (Test-Path $zipPath) {
  Remove-Item $zipPath -Force
}

Write-Host "Creating zip: $zipPath"
Compress-Archive -Path (Join-Path $deployRoot '*') -DestinationPath $zipPath

Write-Host "Deploy zip ready: $zipPath" -ForegroundColor Cyan

