namespace Belin.PhpMinifier;

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Removes comments and whitespace from a PHP script, by calling a Web service.
/// </summary>
/// <param name="executable">The path to the PHP executable.</param>
public sealed class FastTransformer(string executable = "php"): ITransformer {

	/// <summary>
	/// The HTTP client used to query the PHP process.
	/// </summary>
	private HttpClient? httpClient;

	/// <summary>
	/// The underlying PHP process.
	/// </summary>
	private Process? process;

	/// <summary>
	/// Releases any resources associated with this object.
	/// </summary>
	public void Dispose() {
		httpClient?.Dispose();
		process?.Kill();
		process?.Dispose();
		process = null;
	}

	/// <summary>
	/// Starts the underlying PHP process and begins accepting connections.
	/// </summary>
	/// <returns>The TCP port used by the PHP process.</returns>
	/// <exception cref="ProcessException">An error occurred when starting the PHP process.</exception>
	public async Task<int> Listen() {
		if (process is not null) return httpClient!.BaseAddress!.Port;

		var port = GetPort();
		var args = new[] { "-S", $"{IPAddress.Loopback}:{port}", "-t", Path.Join(AppContext.BaseDirectory, "../www") };
		var startInfo = new ProcessStartInfo(executable, args) { CreateNoWindow = true };

		process = Process.Start(startInfo) ?? throw new ProcessException(startInfo.FileName);
		httpClient = new HttpClient { BaseAddress = new($"http://{IPAddress.Loopback}:{port}/") };
		await Task.Delay(1_000);
		return port;
	}

	/// <summary>
	/// Processes a PHP script.
	/// </summary>
	/// <param name="file">The path to the PHP script.</param>
	/// <returns>The transformed script.</returns>
	public async Task<string> Transform(string file) {
		await Listen();
		return await httpClient!.GetStringAsync($"index.php?file={Uri.EscapeDataString(file)}");
	}

	/// <summary>
	/// Gets an ephemeral TCP port chosen by the system.
	/// </summary>
	/// <returns>The TCP port chosen by the system.</returns>
	private static int GetPort() {
		using var tcpListener = new TcpListener(IPAddress.Loopback, 0);
		tcpListener.Start();
		return ((IPEndPoint) tcpListener.LocalEndpoint).Port;
	}
}
