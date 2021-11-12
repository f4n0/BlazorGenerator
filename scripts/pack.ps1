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
dotnet restore "$ProjectRoot\Eos.BlazorGenerator.sln" --configfile "$ProjectRoot\nuget.config"
if ($LASTEXITCODE -ne 0) { throw "Failed" }

Write-Host "$spacer Increase Build Number $spacer"
[xml]$csprojcontents = Get-Content -Path "$ProjectRoot\Eos.Blazor.Generator\Eos.Blazor.Generator.csproj"
$versionNo = [version] $csprojcontents.Project.PropertyGroup[0].Version
$newVersion = "{0}.{1}.{2}" -f $versionNo.Major, $versionNo.Minor, ($versionNo.Build+1)
Write-Host $newVersion
$csprojcontents.Project.PropertyGroup[0].Version = $newVersion
$csprojcontents.Save("$ProjectRoot\Eos.Blazor.Generator\Eos.Blazor.Generator.csproj")
if ($LASTEXITCODE -ne 0) { throw "Failed" }


Write-Host "$spacer Building solution $spacer"
dotnet build "$ProjectRoot\Eos.BlazorGenerator.sln" -c Release
if ($LASTEXITCODE -ne 0) { throw "Failed" }

Write-Host "$spacer Packing solution $spacer" 
dotnet pack "$ProjectRoot" -c Release --output $OutputFolder
if ($LASTEXITCODE -ne 0) { throw "Failed" }