using ConsoleAppFramework;
using System;
using System.Diagnostics;
using System.IO;
using Zubr;
using Zubr.Diagnostics;
using Zubr.Emit;
using Zubr.Logging;

var app = ConsoleApp.Create();

app.Add("build", Commands.Build);

await app.RunAsync(args);

Console.ReadKey();

static class Commands
{
	/// <summary>
	/// Builds the package.
	/// </summary>
	/// <param name="path">-p, Path to the package root directory or the package manifest file.</param>
	/// <param name="debug">-d, Show debug and trace logs.</param>
	public static void Build(string? path = null, bool debug = false)
	{
		string targetPath = string.IsNullOrWhiteSpace(path)
			? Environment.CurrentDirectory
			: Path.GetFullPath(path);

		Project project = Project.Load(targetPath, Logger.Console(debug ? LogLevel.Trace : LogLevel.Info));

		Console.WriteLine("Building zubr package...");
		Console.WriteLine();

		Stopwatch watch = Stopwatch.StartNew();

		Compilation compilation = project.CreateCompilation();

		watch.Stop();

		if (compilation.HasDiagnostics)
		{
			Console.WriteLine();
			Console.WriteLine("Build diagnostics:");

			WriteDiagnostics(compilation.GetDiagnostics());
		}

		Console.WriteLine();
		Console.WriteLine($"Build took {watch.ElapsedMilliseconds}ms");

		watch.Restart();

		EmitResult result = compilation.Emit();

		watch.Stop();

		if (result.HasDiagnostics)
		{
			Console.WriteLine();
			Console.WriteLine("Emit diagnostics:");

			WriteDiagnostics(result.Diagnostics);
		}

		Console.WriteLine();
		Console.WriteLine($"Emit took {watch.ElapsedMilliseconds}ms");

		if (!result.IsSuccess)
		{
			Console.WriteLine();
			Console.WriteLine("Build failed. No output generated.");

			return;
		}

		string outputPath = Path.Combine(project.OutputPath, compilation.AssemblyName);

		try
		{
			Directory.CreateDirectory(project.OutputPath);
			File.WriteAllBytes(outputPath, result.Data);
		}
		catch (Exception ex)
		{
			Console.WriteLine();
			Console.WriteLine($"Failed to write compilation result to file at path '{outputPath}' with error: {ex.Message}");
		}

		Console.WriteLine();
		Console.WriteLine($"Data written to file '{outputPath}'");
	}

	private static void WriteDiagnostics(Diagnostic[] diagnostics)
	{
		Console.WriteLine();

		foreach (Diagnostic diagnostic in diagnostics)
		{
			if (diagnostic.Location.IsDefault)
			{
				Console.WriteLine($"[{diagnostic.Severity}]: {diagnostic.Message}");
			}
			else
			{
				Console.WriteLine($"[{diagnostic.Severity}]: {diagnostic.Message} at {diagnostic.Location}");
			}
		}
	}
}
