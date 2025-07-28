namespace Belin.PhpMinifier;

using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// Removes comments and whitespace from a PHP script, by calling a PHP process.
/// </summary>
/// <param name="executable">The path to the PHP executable.</param>
public sealed class SafeTransformer(string executable = "php"): ITransformer {

	/// <summary>
	/// Releases any resources associated with this object.
	/// </summary>
	public void Dispose() {}

	/// <summary>
	/// Processes a PHP script.
	/// </summary>
	/// <param name="file">The path to the PHP script.</param>
	/// <returns>The transformed script.</returns>
	/// <exception cref="ProcessException">An error occurred when starting the PHP process.</exception>
	public async Task<string> Transform(string file) {
		var startInfo = new ProcessStartInfo(executable, ["-w", Path.GetFullPath(file)]) {
			CreateNoWindow = true,
			RedirectStandardOutput = true
		};

		using var process = Process.Start(startInfo) ?? throw new ProcessException(startInfo.FileName);
		var standardOutput = process.StandardOutput.ReadToEnd().Trim();
		await process.WaitForExitAsync();
		if (process.ExitCode != 0) throw new ProcessException(startInfo.FileName, $"The PHP process failed with exit code {process.ExitCode}.");
		return standardOutput;
	}
}
