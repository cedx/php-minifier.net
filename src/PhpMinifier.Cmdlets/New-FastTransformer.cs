namespace Belin.PhpMinifier.Cmdlets;

/// <summary>
/// Creates a new fast transformer.
/// </summary>
[Cmdlet(VerbsCommon.New, "FastTransformer"), OutputType(typeof(FastTransformer))]
public class NewFastTransformerCommand: Cmdlet {

	/// <summary>
	/// The path to the PHP executable.
	/// </summary>
	[Parameter(Position = 0, ValueFromPipeline = true)]
	public string Executable { get; set; } = "php";

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() => WriteObject(new FastTransformer(Executable));
}
