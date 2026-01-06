namespace Belin.PhpMinifier;

using System.Threading.Tasks;

/// <summary>
/// Tests the features of the <see cref="SafeTransformer"/> class.
/// </summary>
/// <param name="testContext">The test context.</param>
[TestClass]
public sealed class SafeTransformerTests(TestContext testContext) {

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
	public async Task TransformAsync() {
		var file = Path.Join(AppContext.BaseDirectory, "../res/Sample.php");
		using var transformer = new SafeTransformer();
		foreach (var pattern in patterns) Contains(pattern, await transformer.TransformAsync(file, testContext.CancellationToken));
	}
}
