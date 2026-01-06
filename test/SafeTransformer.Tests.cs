namespace Belin.PhpMinifier;

using System.Threading.Tasks;

/// <summary>
/// Tests the features of the <see cref="SafeTransformer"/> class.
/// </summary>
/// <param name="testContext">The test context.</param>
[TestClass]
public sealed class SafeTransformerTests(TestContext testContext) {

	[TestMethod]
	public async Task TransformAsync() {
		var patterns = new[] {
			"<?= 'Hello World!' ?>", // It should remove the inline comments.
			"namespace dummy; class Dummy", // It should remove the multi-line comments.
			"$className = get_class($this); return $className;", // It should remove the single-line comments.
			"__construct() { $this->property" // It should remove the whitespace.
		};

		var file = Path.Join(AppContext.BaseDirectory, "../res/Sample.php");
		using var transformer = new SafeTransformer();
		foreach (var pattern in patterns) Contains(pattern, await transformer.TransformAsync(file, testContext.CancellationToken));
	}
}
