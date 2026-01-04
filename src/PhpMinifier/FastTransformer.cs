namespace Belin.PhpMinifier;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
		httpClient = null;
		process?.Kill();
		process?.Dispose();
		process = null;
	}
	
	/// <summary>
	/// Processes a PHP script.
	/// </summary>
	/// <param name="file">The path to the PHP script.</param>
	/// <returns>The transformed script.</returns>
	public string Transform(string file) => TransformAsync(file, CancellationToken.None).GetAwaiter().GetResult();

	/// <summary>
	/// Processes a PHP script.
	/// </summary>
	/// <param name="file">The path to the PHP script.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>The transformed script.</returns>
	public async Task<string> TransformAsync(string file, CancellationToken cancellationToken = default) {
		await Listen(cancellationToken);
		return await httpClient.GetStringAsync($"PhpMinifier.php?file={Uri.EscapeDataString(Path.GetFullPath(file))}", cancellationToken);
	}

	/// <summary>
	/// Starts the underlying PHP process and begins accepting connections.
	/// </summary>
	/// <returns>The TCP port used by the PHP process.</returns>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <exception cref="ProcessException">An error occurred when starting the PHP process.</exception>
	[MemberNotNull(nameof(httpClient), nameof(process))]
	internal async Task<int> Listen(CancellationToken cancellationToken = default) {
		if (process is not null) return httpClient!.BaseAddress!.Port;

		var port = GetPort();
		var arguments = new[] { "-S", $"{IPAddress.Loopback}:{port}", "-t", Path.GetDirectoryName(GetType().Assembly.Location)! };
		var startInfo = new ProcessStartInfo(executable, arguments) { CreateNoWindow = true };

		httpClient = new HttpClient { BaseAddress = new($"http://{IPAddress.Loopback}:{port}/") };
		process = Process.Start(startInfo) ?? throw new ProcessException(startInfo.FileName);
		await Task.Delay(1_000, cancellationToken);
		return port;
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
