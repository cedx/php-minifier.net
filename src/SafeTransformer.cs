namespace Belin.PhpMinifier;

using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// Removes comments and whitespace from a PHP script, by calling a PHP process.
/// </summary>
/// <param name="executable">The path to the PHP executable.</param>
public sealed class SafeTransformer(string executable = "php"): ITransformer {

	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public ValueTask DisposeAsync() => ValueTask.CompletedTask;

	/// <summary>
	/// Processes a PHP script.
	/// </summary>
	/// <param name="file">The path to the PHP script.</param>
	/// <returns>The transformed script.</returns>
	public async Task<string> Transform(string file) {
		var process = Process.Start(executable);
		await process.WaitForExitAsync();
		return string.Empty;
	}
}
