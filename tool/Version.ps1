"Updating the version number in the sources..."
$version = (Import-PowerShellDataFile "PhpMinifier.psd1").ModuleVersion
foreach ($item in Get-ChildItem "*/*.csproj" -Recurse) {
	(Get-Content $item) -replace "<Version>\d+(\.\d+){2}</Version>", "<Version>$version</Version>" | Out-File $item
}
