namespace Belin.PhpMinifier;

/// <summary>
/// Provides extensions methods for test assertions.
/// </summary>
public static class AssertExtensions {

	/// <summary>
	/// Asserts that the specified delegate does not throw an exception of type <typeparamref name="T"/> (or derived type).
	/// </summary>
	/// <typeparam name="T">The type of exception that should not be thrown.</typeparam>
	/// <param name="_">The instance of the assertion functionality.</param>
	/// <param name="action">The delegate to be tested and which should not throw an exception.</param>
	/// <param name="message">The message to include in the error when the delegate throws an exception of type <typeparamref name="T"/>.</param>
	/// <param name="parameters">An array of parameters to use when formatting the message.</param>
	/// <exception cref="AssertFailedException">The specified delegate threw an exception of type <typeparamref name="T"/>.</exception>
	public static void DoesNotThrow<T>(this Assert _, Action action, string message = "", params object[] parameters) where T : Exception {
		try { action(); }
		catch (T exception) { Fail(message.Length > 0 ? message : exception.Message, parameters); }
		catch {}
	}

	/// <summary>
	/// Asserts that the specified delegate does not throw an exception of type <typeparamref name="T"/> (or derived type).
	/// </summary>
	/// <typeparam name="T">The type of exception that should not be thrown.</typeparam>
	/// <param name="_">The instance of the assertion functionality.</param>
	/// <param name="action">The delegate to be tested and which should not throw an exception.</param>
	/// <param name="message">The message to include in the error when the delegate throws an exception of type <typeparamref name="T"/>.</param>
	/// <param name="parameters">An array of parameters to use when formatting the message.</param>
	/// <returns>Completes when the delegate has been tested.</returns>
	/// <exception cref="AssertFailedException">The specified delegate threw an exception of type <typeparamref name="T"/>.</exception>
	public static async Task DoesNotThrowAsync<T>(this Assert _, Func<Task> action, string message = "", params object[] parameters) where T: Exception {
		try { await action(); }
		catch (T exception) { Fail(message.Length > 0 ? message : exception.Message, parameters); }
		catch {}
	}
}
