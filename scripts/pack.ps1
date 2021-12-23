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
dotnet restore "$ProjectRoot\BlazorGenerator.sln" --configfile "$ProjectRoot\nuget.config"
if ($LASTEXITCODE -ne 0) { throw "Failed" }

Write-Host "$spacer Increase Build Number $spacer"
$doc = [Xml.XmlDocument]::new()
$doc.Load("$ProjectRoot\BlazorGenerator\BlazorGenerator.csproj")
$VersionNode = $doc.SelectSingleNode('Project/PropertyGroup/Version')
if (-not $VersionNode) {
    throw "'Version' node not found in project file"
}
$versionNo = [version] $VersionNode.InnerText
$newVersion = "{0}.{1}.{2}" -f $versionNo.Major, $versionNo.Minor, ($versionNo.Build+1)
Write-Host $newVersion
$VersionNode.InnerText = "$newVersion"
$doc.Save("$ProjectRoot\BlazorGenerator\BlazorGenerator.csproj")

Write-Host "$spacer Building solution $spacer"
dotnet build "$ProjectRoot\BlazorGenerator.sln" -c Release
if ($LASTEXITCODE -ne 0) { throw "Failed" }

Write-Host "$spacer Packing solution $spacer" 
dotnet pack "$ProjectRoot" -c Release --output $OutputFolder
if ($LASTEXITCODE -ne 0) { throw "Failed" }