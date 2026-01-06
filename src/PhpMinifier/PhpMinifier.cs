namespace Belin.PhpMinifier;

/// <summary>
/// Removes comments and whitespace from PHP scripts.
/// </summary>
/// <param name="mode">The operation mode of the minifier.</param>
/// <param name="executable">The path to the PHP executable.</param>
public class PhpMinifier(TransformMode mode = TransformMode.Safe, string executable = "php"): IDisposable {

	/// <summary>
	/// TODO
	/// </summary>
	private readonly ITransformer transformer = mode == TransformMode.Fast ? new FastTransformer(executable) : new SafeTransformer(executable);

	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="path">The path to the input file or directory.</param>
	/// <param name="destinationPath">The path to the output directory.</param>
	/// <param name="filter">A pattern used to filter the list of files to be processed.</param>
	/// <param name="recurse">Value indicating whether to process the input path recursively.</param>
	public void Compress(string path, string? destinationPath = null, string filter = "*.php", bool recurse = false) =>
		CompressAsync(path, destinationPath, filter, recurse, CancellationToken.None).GetAwaiter().GetResult();

	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="path">The path to the input file or directory.</param>
	/// <param name="destinationPath">The path to the output directory.</param>
	/// <param name="filter">A pattern used to filter the list of files to be processed.</param>
	/// <param name="recurse">Value indicating whether to process the input path recursively.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <exception cref="InvalidOperationException">The input file or directory could not be found.</exception>
	/// <exception cref="DirectoryNotFoundException">The output directory could not be determined.</exception>
	public async Task CompressAsync(string path, string? destinationPath = null, string filter = "*.php", bool recurse = false, CancellationToken cancellationToken = default) {
		FileSystemInfo input = true switch {
			true when Directory.Exists(path) => new DirectoryInfo(path),
			true when File.Exists(path) => new FileInfo(path),
			_ => throw new InvalidOperationException("The input file or directory could not be found.")
		};

		var output = (!string.IsNullOrEmpty(destinationPath) ? new DirectoryInfo(destinationPath) : (input is FileInfo fileInfo ? fileInfo.Directory : input as DirectoryInfo))
			?? throw new DirectoryNotFoundException("The output directory could not be determined.");

		var files = input switch {
			DirectoryInfo directory => directory.EnumerateFiles(filter, recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly),
			FileInfo file => [file],
			_ => []
		};

		foreach (var file in files) {
			var script = await transformer.TransformAsync(file.FullName, cancellationToken);
			var target = Path.Join(output.FullName, input is FileInfo ? file.Name : Path.GetRelativePath(input.FullName, file.FullName));
			if (Path.GetDirectoryName(target) is string folder) Directory.CreateDirectory(folder);
			await File.WriteAllTextAsync(target, script, cancellationToken);
		}
	}

	/// <summary>
	/// Releases any resources associated with this object.
	/// </summary>
	public void Dispose() {
		transformer.Dispose();
		GC.SuppressFinalize(this);
	}
}
