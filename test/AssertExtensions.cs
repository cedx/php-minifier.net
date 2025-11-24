namespace Belin.PhpMinifier;

/// <summary>
/// Provides extensions members for test assertions.
/// </summary>
public static class AssertExtensions {
	extension(Assert _) {

		/// <summary>
		/// Asserts that the specified action does not throw an exception of type <typeparamref name="T"/> (or derived type).
		/// </summary>
		/// <typeparam name="T">The type of exception that should not be thrown.</typeparam>
		/// <param name="action">The action to be tested and which should not throw an exception.</param>
		/// <param name="message">The message to include in the error when the action throws an exception of type <typeparamref name="T"/>.</param>
		/// <exception cref="AssertFailedException">The specified action threw an exception of type <typeparamref name="T"/>.</exception>
		public void DoesNotThrow<T>(Action action, string message = "") where T: Exception {
			try { action(); }
			catch (T exception) { Fail(message.Length > 0 ? message : exception.Message); }
			catch {}
		}

		/// <summary>
		/// Asserts that the specified action does not throw an exception of type <typeparamref name="T"/> (or derived type).
		/// </summary>
		/// <typeparam name="T">The type of exception that should not be thrown.</typeparam>
		/// <param name="action">The action to be tested and which should not throw an exception.</param>
		/// <param name="message">The message to include in the error when the action throws an exception of type <typeparamref name="T"/>.</param>
		/// <returns>Completes when the action has been tested.</returns>
		/// <exception cref="AssertFailedException">The specified action threw an exception of type <typeparamref name="T"/>.</exception>
		public async Task DoesNotThrowAsync<T>(Func<Task> action, string message = "") where T: Exception {
			try { await action(); }
			catch (T exception) { Fail(message.Length > 0 ? message : exception.Message); }
			catch {}
		}
	}
}
