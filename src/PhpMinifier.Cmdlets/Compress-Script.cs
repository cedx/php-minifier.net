namespace Belin.PhpMinifier.Cmdlets;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Minifies PHP source code by removing comments and whitespace.
/// </summary>
[Cmdlet(VerbsData.Compress, "Script", DefaultParameterSetName = nameof(Path)), OutputType(typeof(void), typeof(string))]
public class CompressScriptCommand: PSCmdlet {

	/// <summary>
	/// The path to the PHP executable.
	/// </summary>
	[Parameter]
	public string Command { get; set; } = "php";

	/// <summary>
	/// The path to the output directory.
	/// </summary>
	[Parameter(Position = 1)]
	public required string DestinationPath { get; set; }

	/// <summary>
	/// A pattern used to filter the list of files to be processed.
	/// </summary>
	[Parameter]
	public string Filter { get; set; } = "*.php";

	/// <summary>
	/// The path to the PHP script to compress.
	/// </summary>
	[Parameter(Mandatory = true, ParameterSetName = nameof(LiteralPath))]
	public required string[] LiteralPath { get; set; }

	/// <summary>
	/// The operation mode of the minifier.
	/// </summary>
	[Parameter]
	public TransformMode Mode { get; set; } = TransformMode.Safe;

	/// <summary>
	/// The path to the PHP script to compress.
	/// </summary>
	[Parameter(Mandatory = true, ParameterSetName = nameof(Path), Position = 0, ValueFromPipeline = true), SupportsWildcards]
	public required string[] Path { get; set; }

	/// <summary>
	/// Value indicating whether to silence the minifier output.
	/// </summary>
	[Parameter]
	public SwitchParameter Quiet { get; set; }

	/// <summary>
	/// Value indicating whether to process the input path recursively.
	/// </summary>
	[Parameter]
	public SwitchParameter Recurse { get; set; }

	/// <summary>
	/// The instance used to process the PHP code.
	/// </summary>
	private ITransformer? transformer;

	/// <summary>
	/// Performs initialization of command execution.
	/// </summary>
	[MemberNotNull(nameof(transformer))]
	protected override void BeginProcessing() =>
		transformer = Mode == TransformMode.Fast ? new FastTransformer(Command) : new SafeTransformer(Command);

	/// <summary>
	/// Performs clean-up after the command execution.
	/// </summary>
	protected override void EndProcessing() => transformer?.Dispose();

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() {
		using var script = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand("Get-ChildItem");
		if (ParameterSetName == nameof(LiteralPath)) script.AddParameter(nameof(LiteralPath), LiteralPath);
		else script.AddParameter(nameof(Path), Path);

		script.AddParameter("File");
		if (Filter.Length > 0) script.AddParameter(nameof(Filter), Filter);
		if (Recurse) script.AddParameter(nameof(Recurse));

		var files = script.Invoke<FileInfo>();
		if (script.HadErrors) {
			ThrowTerminatingError(script.Streams.Error.First());
			return;
		}

		foreach (var file in files) {

		}
	}
}
