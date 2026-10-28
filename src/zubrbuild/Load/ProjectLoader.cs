using System;
using System.IO;
using System.Linq;
using Zubr.Diagnostics;
using Zubr.Emit;
using Zubr.Logging;
using Zubr.Text;

namespace Zubr.Load;

public sealed class ProjectLoader
{
	public ILogger Logger { get; }

	public ProjectLoaderOptions Options { get; }

	public ProjectLoader() : this(ProjectLoaderOptions.Default)
	{
	}

	public ProjectLoader(ProjectLoaderOptions options) : this(options, NullLogger.Instance)
	{
	}

	public ProjectLoader(ProjectLoaderOptions options, ILogger logger)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentNullException.ThrowIfNull(logger);

		Options = options;
		Logger = logger;
	}

	public ProjectLoadResult Load(string path)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(path);

		if (!Path.Exists(path))
		{
			throw new ProjectException($"Zubr package not found at path: {path}");
		}

		FileAttributes attr;

		try
		{
			attr = File.GetAttributes(path);
		}
		catch (Exception ex)
		{
			throw new ProjectException($"Loading package script file failed with error: {ex.Message} at path: {path}", ex);
		}

		string targetPath = path;

		if (attr != default && attr.HasFlag(FileAttributes.Directory))
		{
			if (Options.PackageFileStrategy == PackageFileStrategy.Require)
			{
				throw new ProjectException($"Target path does not point to a Zubr package script file: {path}");
			}

			targetPath = Path.Combine(targetPath, Options.DefaultScriptFileName);
		}

		ProjectSettings? settings = LoadSettingsFromFile(targetPath, out ErrorMessage[]? errors);

		return new(settings, targetPath, errors);
	}

	private ProjectSettings? LoadSettingsFromFile(string path, out ErrorMessage[]? errors)
	{
		SourceText content;

		try
		{
			content = SourceText.FromFile(path);
		}
		catch (Exception ex)
		{
			throw new ProjectException($"Loading package script file failed with error: {ex.Message} at path: {path}", ex);
		}

		ProjectSettings? settings = TryCompileSettings(content, out errors);

		return settings;
	}

	private ProjectSettings? TryCompileSettings(SourceText source, out ErrorMessage[]? errors)
	{
		SyntaxTree syntaxTree = SyntaxTree.Parse(source);
		Compilation compilation = Compilation.Create("zubrbuild", [syntaxTree], []);

		EmitResult result = compilation.Emit();

		if(result.HasDiagnostics)
		{
			LogDiagnostics(result);

			errors = result.Diagnostics
				.Where(x => x.Severity >= DiagnosticSeverity.Warning)
				.Select(x => new ErrorMessage()
				{
					Message = x.Message,
					Level = GetErrorLevel(x.Severity),
					Column = x.Location.Column,
					Line = x.Location.Line
				})
				.ToArray();

			return null;
		}

		errors = null;
		return CreateSettings();
	}

	private void LogDiagnostics(EmitResult result)
	{
		foreach (DiagnosticMessage diag in result.Diagnostics)
		{
			switch (diag.Severity)
			{
				case DiagnosticSeverity.Info:
					Logger.LogInfo($"{diag.Code}: {diag.Message}");
					break;

				case DiagnosticSeverity.Warning:
					Logger.LogWarning($"{diag.Code}: {diag.Message}");
					break;

				case DiagnosticSeverity.Error:
					Logger.LogError($"{diag.Code}: {diag.Message}");
					break;
			}
		}
	}

	private static ProjectSettings CreateSettings()
	{
		return new()
		{
			Metadata = new()
			{
				Name = "zubr"
			},
			Build = new()
			{
				AssemblyName = "zubr",
				Target = "alpha"
			}
		};
	}

	private static ErrorLevel GetErrorLevel(DiagnosticSeverity severity)
	{
		return severity switch
		{
			DiagnosticSeverity.Warning => ErrorLevel.Warning,
			DiagnosticSeverity.Error => ErrorLevel.Error,
			_ => ErrorLevel.None
		};
	}
}
