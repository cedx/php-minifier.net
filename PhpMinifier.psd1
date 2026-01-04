@{
	DefaultCommandPrefix = "Php"
	ModuleVersion = "2.0.0"
	PowerShellVersion = "7.5"
	RootModule = "bin/Belin.PhpMinifier.Cmdlets.dll"

	Author = "Cédric Belin <cedx@outlook.com>"
	CompanyName = "Cedric-Belin.fr"
	Copyright = "© Cédric Belin"
	Description = "Minify PHP source code by removing comments and whitespace."
	GUID = "bcbf1848-7f0f-4eac-83c7-c83390f4265c"

	AliasesToExport = @()
	FunctionsToExport = @()
	VariablesToExport = @()

	CmdletsToExport = @(
		"Compress-Script"
		"New-Transformer"
	)

	RequiredAssemblies = @(
		"bin/Belin.PhpMinifier.dll"
	)

	PrivateData = @{
		PSData = @{
			LicenseUri = "https://github.com/cedx/php-minifier.net/blob/main/License.md"
			ProjectUri = "https://github.com/cedx/php-minifier.net"
			ReleaseNotes = "https://github.com/cedx/php-minifier.net/releases"
			Tags = "ci", "compress", "minify", "php"
		}
	}
}
