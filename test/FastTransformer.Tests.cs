namespace Belin.PhpMinifier;

using System.Threading.Tasks;

/// <summary>
/// Tests the features of the <see cref="FastTransformer"/> class.
/// </summary>
[TestClass]
public sealed class FastTransformerTests {

	[TestMethod]
	public async Task Listen() {
		// It should not throw, even if called several times.
		using var transformer = new FastTransformer();
		await That.DoesNotThrowAsync<Exception>(transformer.Listen);
		await That.DoesNotThrowAsync<Exception>(transformer.Listen);
	}

	[TestMethod]
	public async Task Transform() {
		var patterns = new[] {
			"<?= 'Hello World!' ?>", // It should remove the inline comments.
			"namespace dummy; class Dummy", // It should remove the multi-line comments.
			"$className = get_class($this); return $className;", // It should remove the single-line comments.
			"__construct() { $this->property" // It should remove the whitespace.
		};

		var file = Path.Join(AppContext.BaseDirectory, "../res/Sample.php");
		using var transformer = new FastTransformer();
		foreach (var pattern in patterns) Contains(pattern, await transformer.Transform(file));
	}
}
