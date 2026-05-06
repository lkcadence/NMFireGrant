param(
  [int]$Port = 52945
)

$iisExpress = "${env:ProgramFiles(x86)}\IIS Express\iisexpress.exe";
$sitePath = Join-Path $PSScriptRoot 'NMSFMFireGrantWF';

if (-not (Test-Path $iisExpress)) {
  Write-Error "IIS Express not found at: $iisExpress";
  exit 1;
}

if (-not (Test-Path $sitePath)) {
  Write-Error "Site path not found: $sitePath";
  exit 1;
}

# Restart behavior: stop any existing IIS Express workers first.
$running = Get-Process -Name 'iisexpress' -ErrorAction SilentlyContinue;
if ($running) {
  Write-Host 'Stopping existing IIS Express process(es)...' -ForegroundColor Yellow;
  $running | Stop-Process -Force -ErrorAction Stop;
  Start-Sleep -Seconds 1;
}

Write-Host "Starting IIS Express at http://localhost:$Port/" -ForegroundColor Cyan;
& $iisExpress /path:"$sitePath" /port:$Port;
