namespace Belin.PhpMinifier.Cmdlets;

/// <summary>
/// Creates a new safe transformer.
/// </summary>
[Cmdlet(VerbsCommon.New, "SafeTransformer"), OutputType(typeof(SafeTransformer))]
public class NewSafeTransformerCommand: Cmdlet {

	/// <summary>
	/// The path to the PHP executable.
	/// </summary>
	[Parameter(Position = 0, ValueFromPipeline = true)]
	public string Executable { get; set; } = "php";

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() => WriteObject(new SafeTransformer(Executable));
}
