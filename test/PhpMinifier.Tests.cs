namespace Belin.PhpMinifier;

/// <summary>
/// Tests the features of the <see cref="PhpMinifier"/> class.
/// </summary>
/// <param name="testContext">The test context.</param>
[TestClass]
public sealed class PhpMinifierTests(TestContext testContext) {

	/// <summary>
	/// The patterns tested to determine if minification is OK.
	/// </summary>
	private readonly string[] patterns = [
		"<?= 'Hello World!' ?>",
		"namespace dummy; class Dummy",
		"$className = get_class($this); return $className;",
		"__construct() { $this->property"
	];

	[TestMethod]
	public async Task CompressAsync() {
		var inputDirectory = Path.Join(AppContext.BaseDirectory, "../res");
		var outputDirectory = Path.Join(AppContext.BaseDirectory, "../var");
		var outputFile = Path.Join(outputDirectory, "Sample.php");

		using (var fastMinifier = new PhpMinifier(TransformMode.Fast)) {
			await fastMinifier.CompressAsync(Path.Join(inputDirectory, "Sample.php"), outputDirectory, cancellationToken: testContext.CancellationToken);
			var script = await File.ReadAllTextAsync(outputFile, testContext.CancellationToken);
			foreach (var pattern in patterns) Contains(pattern, script);
			File.Delete(outputFile);
		}
		using (var safeMinifier = new PhpMinifier(TransformMode.Safe)) {
			await safeMinifier.CompressAsync(inputDirectory, outputDirectory, recurse: true, cancellationToken: testContext.CancellationToken);
			var script = await File.ReadAllTextAsync(outputFile, testContext.CancellationToken);
			foreach (var pattern in patterns) Contains(pattern, script);
			File.Delete(outputFile);
		}
	}
}
