dotnet publish PlaneAlerter\PlaneAlerter.csproj -p:PublishProfile=Publish-x64 -c Release -f net7.0-windows -r win-x64
dotnet publish PlaneAlerter\PlaneAlerter.csproj -p:PublishProfile=Publish-x86 -c Release -f net7.0-windows -r win-x86

$version = (Get-Item PlaneAlerter\bin\Release\publish\win-x64\PlaneAlerter.exe).VersionInfo.FileVersionRaw
$versionString = "$($version.Major).$($version.Minor).$($version.Build)"
$x64OutputZip = "Releases\PlaneAlerter_${versionString}_x64.zip"
$x86OutputZip = "Releases\PlaneAlerter_${versionString}_x86.zip"

if ((Test-Path $x64OutputZip) -or (Test-Path $x86OutputZip)) {
    Write-Warning "${x64OutputZip} or ${x86OutputZip} already exists"
}
$confirmation = Read-Host "Version ${versionString}, continue? y/n"
if ($confirmation -eq 'y') {
    Compress-Archive PlaneAlerter\bin\Release\publish\win-x64\* $x64OutputZip -Update
    Compress-Archive PlaneAlerter\bin\Release\publish\win-x86\* $x86OutputZip -Update
}