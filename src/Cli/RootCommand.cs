namespace Belin.PhpMinifier.Cli;

using System.CommandLine;

/// <summary>
/// Minifies PHP source code by removing comments and whitespace.
/// </summary>
internal class RootCommand: System.CommandLine.RootCommand {

	/// <summary>
	/// Creates a new root command.
	/// </summary>
	public RootCommand(): base("Minify PHP source code by removing comments and whitespace.") {
		SetAction(Invoke);
	}

	/// <summary>
	/// Invokes this command.
	/// </summary>
	/// <param name="parseResult">The results of parsing the command line input.</param>
	/// <returns>The exit code.</returns>
	public int Invoke(ParseResult parseResult) {
		return 0;
	}
}
