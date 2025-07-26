namespace Belin.PhpMinifier;

/// <summary>
/// Removes comments and whitespace from a PHP script.
/// </summary>
public interface ITransformer: IAsyncDisposable {

	/// <summary>
	/// Processes a PHP script.
	/// </summary>
	/// <param name="file">The path to the PHP script.</param>
	/// <returns>The transformed script.</returns>
	Task<string> Transform(string file);
}
