namespace Belin.PhpMinifier;

/// <summary>
/// Exception thrown when an error has occurred during the execution of a process.
/// </summary>
/// <param name="fileName">The name of the document or application file that has been started.</param>
/// <param name="message">The message that describes the error.</param>
/// <param name="innerException">The exception that is the cause of the current exception.</param>
public class ProcessException(string fileName, string message = "", Exception? innerException = null):
	Exception(message.Length > 0 ? message : $"The \"{fileName}\" process could not be started.", innerException) {

	/// <summary>
	/// The name of the document or application file that has been started.
	/// </summary>
	public string FileName => fileName;
}
