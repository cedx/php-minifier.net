if ($Release) {
	& "$PSScriptRoot/Clean.ps1"
	& "$PSScriptRoot/Version.ps1"
}
else {
	"The ""-Release"" switch must be set!"
	exit 1
}

"Publishing the package..."
$module = Import-PowerShellDataFile PhpMinifier.psd1
$version = $module.ModuleVersion
git tag "v$version"
git push origin "v$version"

$output = "var/NuGet"
dotnet pack --output $output
Get-Item "$output/*.nupkg" | ForEach-Object {
	dotnet nuget push $_ --api-key $Env:NUGET_API_KEY --source https://api.nuget.org/v3/index.json
}

$output = "var/PSModule"
New-Item $output/bin -ItemType Directory | Out-Null
Copy-Item PhpMinifier.psd1 $output
Copy-Item *.md $output
Copy-Item $module.RootModule $output/bin
$module.RequiredAssemblies | Copy-Item -Destination $output/bin

$output = "var/PSGallery"
New-Item $output -ItemType Directory | Out-Null
Compress-PSResource var/PSModule $output
Get-Item "$output/*.nupkg" | ForEach-Object { Publish-PSResource -ApiKey $Env:PSGALLERY_API_KEY -NupkgPath $_ }
