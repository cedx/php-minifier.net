namespace Belin.PhpMinifier;

/// <summary>
/// Removes comments and whitespace from a PHP script, by calling a Web service.
/// </summary>
/// <param name="executable">The path to the PHP executable.</param>
public sealed class FastTransformer(string executable = "php"): ITransformer {

	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public ValueTask DisposeAsync() {
		return ValueTask.CompletedTask;
	}

	/// <summary>
	/// Processes a PHP script.
	/// </summary>
	/// <param name="file">The path to the PHP script.</param>
	/// <returns>The transformed script.</returns>
	public Task<string> Transform(string file) {
		return Task.FromResult(string.Empty);
	}
}
