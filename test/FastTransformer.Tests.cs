namespace Belin.PhpMinifier;

using System.Threading.Tasks;

/// <summary>
/// Tests the features of the <see cref="FastTransformer"/> class.
/// </summary>
/// <param name="testContext">The test context.</param>
[TestClass]
public sealed class FastTransformerTests(TestContext testContext) {

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
	public async Task Listen() {
		// It should not throw, even if called several times.
		using var transformer = new FastTransformer();
		await That.DoesNotThrowAsync<Exception>(() => transformer.Listen(testContext.CancellationToken));
		await That.DoesNotThrowAsync<Exception>(() => transformer.Listen(testContext.CancellationToken));
	}

	[TestMethod]
	public async Task TransformAsync() {
		var file = Path.Join(AppContext.BaseDirectory, "../res/Sample.php");
		using var transformer = new FastTransformer();
		foreach (var pattern in patterns) Contains(pattern, await transformer.TransformAsync(file, testContext.CancellationToken));
	}
}
