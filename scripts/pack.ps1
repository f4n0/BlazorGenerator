$ErrorActionPreference = "Stop"
$ProjectRoot = Resolve-Path "$PSScriptRoot\.."
$spacer = "".PadRight(25, "#")

$OutputFolder = "$env:Build_ArtifactStagingDirectory"
if (-not $OutputFolder) {
    $OutputFolder = "$ProjectRoot"
}
$OutputFolder = "$OutputFolder\_artifacts"
Write-Host "Output folder is '$OutputFolder'"

if (Test-Path $OutputFolder) { Remove-Item $OutputFolder -Force -Recurse }

Write-Host "$spacer Restoring solution $spacer"
dotnet restore "$ProjectRoot\Eos.BlazorGenerator.sln" --configfile nuget.config 
if ($LASTEXITCODE -ne 0) { throw "Failed" }

Write-Host "$spacer Building solution $spacer"
dotnet build "$ProjectRoot\Eos.BlazorGenerator.sln" -c Release
if ($LASTEXITCODE -ne 0) { throw "Failed" }

Write-Host "$spacer Packing solution $spacer" 
dotnet pack "$ProjectRoot" -c Release --output $OutputFolder
if ($LASTEXITCODE -ne 0) { throw "Failed" }