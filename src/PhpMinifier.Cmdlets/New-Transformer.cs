namespace Belin.PhpMinifier.Cmdlets;

/// <summary>
/// Creates a new transformer.
/// </summary>
[Cmdlet(VerbsCommon.New, "Transformer"), OutputType(typeof(ITransformer))]
public class NewTransformerCommand: Cmdlet {

	/// <summary>
	/// The path to the PHP executable.
	/// </summary>
	[Parameter]
	public string Executable { get; set; } = "php";

	/// <summary>
	/// The operation mode of the minifier.
	/// </summary>
	[Parameter(Position = 0)]
	public TransformMode Type { get; set; } = TransformMode.Safe;

	/// <summary>
	/// Performs execution of this command.
	/// </summary>
	protected override void ProcessRecord() =>
		WriteObject(Type == TransformMode.Fast ? new FastTransformer(Executable) : new SafeTransformer(Executable));
}
