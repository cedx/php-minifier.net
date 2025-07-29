namespace Belin.PhpMinifier.Cli;

using System.CommandLine;

/// <summary>
/// Minifies PHP source code by removing comments and whitespace.
/// </summary>
internal class RootCommand: System.CommandLine.RootCommand {

	/// <summary>
	/// The path to the input file or directory.
	/// </summary>
	private readonly Argument<FileSystemInfo> inputArgument = new Argument<FileSystemInfo>("input") {
		Description = "The path to the input file or directory."
	}.AcceptExistingOnly();

	/// <summary>
	/// The path to the output directory.
	/// </summary>
	private readonly Argument<DirectoryInfo?> outputArgument = new Argument<DirectoryInfo?>("output") {
		DefaultValueFactory = _ => null,
		Description = "The path to the output directory."
	}.AcceptLegalFilePathsOnly();

	/// <summary>
	/// The path to the PHP executable.
	/// </summary>
	private readonly Option<string> binaryOption = new("--binary", ["-b"]) {
		DefaultValueFactory = _ => "php",
		Description = "The path to the PHP executable."
	};

	/// <summary>
	/// The extension of the PHP files to process.
	/// </summary>
	private readonly Option<string> extensionOption = new("--extension", ["-e"]) {
		DefaultValueFactory = _ => "php",
		Description = "The extension of the PHP files to process."
	};

	/// <summary>
	/// The operation mode of the minifier.
	/// </summary>
	private readonly Option<string> modeOption = new Option<string>("--mode", ["-m"]) {
		DefaultValueFactory = _ => "safe",
		Description = "The operation mode of the minifier."
	}.AcceptOnlyFromAmong(["fast", "safe"]);

	/// <summary>
	/// Value indicating whether to process the input directory recursively.
	/// </summary>
	private readonly Option<bool> recursiveOption = new("--recursive", ["-r"]) {
		Description = "Whether to process the input directory recursively."
	};

	/// <summary>
	/// Value indicating whether to silence the minifier output.
	/// </summary>
	private readonly Option<bool> silentOption = new("--silent", ["-s"]) {
		Description = "Whether to silence the minifier output."
	};

	/// <summary>
	/// Creates a new root command.
	/// </summary>
	public RootCommand(): base("Minify PHP source code by removing comments and whitespace.") {
		Arguments.Add(inputArgument);
		Arguments.Add(outputArgument);
		Options.Add(binaryOption);
		Options.Add(extensionOption);
		Options.Add(modeOption);
		Options.Add(recursiveOption);
		Options.Add(silentOption);
		SetAction(InvokeAsync);
	}

	/// <summary>
	/// Invokes this command.
	/// </summary>
	/// <param name="parseResult">The results of parsing the command line input.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The exit code.</returns>
	public async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken) {
		var binary = parseResult.GetRequiredValue(binaryOption);
		using ITransformer transformer = parseResult.GetRequiredValue(modeOption) == "fast" ? new FastTransformer(binary) : new SafeTransformer(binary);

		var input = parseResult.GetRequiredValue(inputArgument);
		var searchOption = parseResult.GetValue(recursiveOption) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
		var files = input switch {
			DirectoryInfo directory => directory.EnumerateFiles($"*.{parseResult.GetRequiredValue(extensionOption)}", searchOption),
			FileInfo file => [file],
			_ => []
		};

		var output = parseResult.GetValue(outputArgument) ?? (input is FileInfo fileInfo ? fileInfo.Directory! : input);
		var silent = parseResult.GetValue(silentOption);

		foreach (var file in files) {
			var relativePath = input is FileInfo ? file.Name : Path.GetRelativePath(input.FullName, file.FullName);
			if (!silent) Console.WriteLine("Minifying: {0}", relativePath);

			var script = await transformer.Transform(file.FullName);
			var target = Path.Join(output.FullName, relativePath);
			if (Path.GetDirectoryName(target) is string folder) Directory.CreateDirectory(folder);
			await File.WriteAllTextAsync(target, script, cancellationToken);
		}

		return 0;
	}
}
