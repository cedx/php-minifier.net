using Belin.PhpMinifier;
using System;
using System.IO;

// Choose an appropriate transformer.
using ITransformer transformer = Environment.GetEnvironmentVariable("PHPMINIFIER_MODE") == "fast"
	? new FastTransformer()
	: new SafeTransformer();

// Scan the input directory for PHP files.
var input = new DirectoryInfo("path/to/source/folder");
var files = input.EnumerateFiles("*.php", SearchOption.AllDirectories);

// Write the transformed files to the output directory.
var output = new DirectoryInfo("path/to/destination/folder");
foreach (var file in files) {
	var relativePath = Path.GetRelativePath(input.FullName, file.FullName);
	Console.WriteLine("Minifying: {0}", relativePath);

	var script = await transformer.Transform(file.FullName);
	var target = Path.Join(output.FullName, relativePath);
	if (Path.GetDirectoryName(target) is string folder) Directory.CreateDirectory(folder);
	await File.WriteAllTextAsync(target, script);
}
