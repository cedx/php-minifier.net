namespace Belin.PhpMinifier;

using System.Threading;

/// <summary>
/// Removes comments and whitespace from a PHP script.
/// </summary>
public interface ITransformer: IDisposable {
	
	/// <summary>
	/// Processes a PHP script.
	/// </summary>
	/// <param name="file">The path to the PHP script.</param>
	/// <returns>The transformed script.</returns>
	string Transform(string file);

	/// <summary>
	/// Processes a PHP script.
	/// </summary>
	/// <param name="file">The path to the PHP script.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The transformed script.</returns>
	Task<string> TransformAsync(string file, CancellationToken cancellationToken = default);
}
