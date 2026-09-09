$dir = (Resolve-Path "bin\Debug").Path

# Pre-load all dependency DLLs in bin\Debug
Get-ChildItem -Path $dir -Filter "*.dll" | ForEach-Object {
    try {
        $bytes = [System.IO.File]::ReadAllBytes($_.FullName)
        [System.Reflection.Assembly]::Load($bytes) | Out-Null
    } catch {
        # ignore native or already loaded
    }
}

$target = Join-Path $dir "SolutionComponents.dll"
$bytes = [System.IO.File]::ReadAllBytes($target)
$asm = [System.Reflection.Assembly]::Load($bytes)
$type = $asm.GetType("SolutionComponents.Tests.SolutionAuditServiceTests")
$method = $type.GetMethod("RunAllTests")
$method.Invoke($null, $null)
