using namespace System.Diagnostics.CodeAnalysis
using module ./FastTransformer.psm1
using module ./SafeTransformer.psm1

<#
.SYNOPSIS
	Minifies PHP source code by removing comments and whitespace.
.PARAMETER Path
	The path to the input file or directory.
.PARAMETER DestinationPath
	The path to the output directory.
.PARAMETER Binary
	The path to the PHP executable.
.PARAMETER Extension
	The extension of the PHP files to process.
.PARAMETER Mode
	The operation mode of the minifier.
.PARAMETER Quiet
	Value indicating whether to silence the minifier output.
.PARAMETER Recurse
	Value indicating whether to process the input path recursively.
#>
function Compress-Php {
	[CmdletBinding()]
	[OutputType([void])]
	param (
		[Parameter(Mandatory, Position = 0)]
		[ValidateScript({ Test-Path $_ }, ErrorMessage = "The input file or directory does not exist.")]
		[string] $Path,

		[Parameter(Mandatory, Position = 1)]
		[ValidateScript({ Test-Path $_ -IsValid }, ErrorMessage = "The output path is invalid.")]
		[string] $DestinationPath,

		[Parameter()]
		[ValidateNotNullOrWhiteSpace()]
		[string] $Binary = "php",

		[Parameter()]
		[ValidateNotNullOrWhiteSpace()]
		[string] $Extension = "php",

		[Parameter()]
		[TransformMode] $Mode = [TransformMode]::Safe,

		[Parameter()]
		[switch] $Quiet,

		[Parameter()]
		[switch] $Recurse
	)

	begin {
		$transformer = $Mode -eq [TransformMode]::Fast ? [FastTransformer] $Binary : [SafeTransformer] $Binary
	}
	process {
		$isFilePath = Test-Path $Path -PathType Leaf
		if (-not $DestinationPath) { $DestinationPath = $isFilePath ? (Split-Path $Path) : $Path }

		$files = $isFilePath ? @($Path) : (Get-ChildItem "$Path/*.$Extension" -Recurse:$Recurse)
		foreach ($file in $files) {
			$relativePath = Resolve-Path $file -Relative -RelativeBasePath $Path
		}
	}
	clean {
		$transformer.Dispose()
	}
}
