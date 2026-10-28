using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Zubr.Load;
using Zubr.Logging;
using Zubr.Text;

namespace Zubr;

[DebuggerDisplay("{Metadata.Name}:{Metadata.Version}")]
public sealed class Project
{
	public ProjectMetadata Metadata { get; }

	public string RootPath { get; }

	public string OutputPath { get; }

	public OutputKind OutputKind { get; }

	public string AssemblyName { get; }

	public ILogger Logger { get; }

	internal Project(
		string rootPath,
		ProjectSettings settings,
		ILogger logger
	)
	{
		RootPath = rootPath;
		Metadata = CreateMetadata(settings);

		OutputKind = settings.Build?.OutputKind ?? default;
		AssemblyName = GetAssemblyName(settings, OutputKind);
		OutputPath = GetOutputPath(settings, rootPath);
		Logger = logger;
	}

	public IEnumerable<string> GetFiles()
	{
		return Directory.EnumerateFiles(RootPath, "*.zr", SearchOption.AllDirectories);
	}

	public Compilation CreateCompilation()
	{
		List<SyntaxTree> syntaxTrees = GetSyntaxTrees();

		Compilation compilation = Compilation.Create(AssemblyName, syntaxTrees, [], new()
		{
			LanguageVersion = LanguageVersion.Latest,
			OutputKind = OutputKind.Console
		});

		return compilation;
	}

	public static Project Load(string path)
	{
		return Load(path, NullLogger.Instance);
	}

	public static Project Load(string path, ILogger logger)
	{
		ProjectLoaderOptions options = ProjectLoaderOptions.Default;
		ProjectLoader loader = new(options, logger);

		ProjectLoadResult result = loader.Load(path);

		if(!result.IsSuccess)
		{
			throw new ProjectException($"Project failed to load at path: {path}");
		}

		string rootPath = Path.GetDirectoryName(result.FilePath)!;

		return new Project(rootPath, result.Settings, logger);
	}

	private List<SyntaxTree> GetSyntaxTrees()
	{
		List<SyntaxTree> syntaxTrees = new();

		foreach (string file in GetFiles())
		{
			string content;

			try
			{
				content = File.ReadAllText(file);
			}
			catch (Exception ex)
			{
				Logger.LogError($"Could not read contents of file '{file}': {ex.Message}");
				continue;
			}

			Logger.LogDebug($"Parsing syntax tree at path: '{file}'");
			SyntaxTree tree = SyntaxTree.Parse(SourceText.FromSource(content, file));
			syntaxTrees.Add(tree);
		}

		return syntaxTrees;
	}

	private static ProjectMetadata CreateMetadata(ProjectSettings settings)
	{
		return new()
		{
			Name = settings.Metadata?.Name ?? Constants.DEFAULT_NAME,
			Authors = settings.Metadata?.Authors ?? [],
			TargetVersion = Enum.TryParse(settings.Build?.Target, out TargetVersion v) ? v : TargetVersion.Alpha,
			Url = settings.Metadata?.Url,
			Version = settings.Metadata?.Version ?? "0.0.0",
			Dependencies = settings.Dependencies ?? []
		};
	}

	private static string GetAssemblyName(ProjectSettings settings, OutputKind outputKind)
	{
		string assemblyName = string.IsNullOrWhiteSpace(settings.Build?.AssemblyName)
			? Constants.DEFAULT_NAME
			: settings.Build.AssemblyName;

		string extension = outputKind switch
		{
			OutputKind.Lib
				=> "dll",

			OutputKind.App or
			OutputKind.Console
				=> "exe",

			_ => throw new UnreachableException()
		};

		return $"{assemblyName}.{extension}";
	}

	private static string GetOutputPath(ProjectSettings settings, string rootPath)
	{
		string targetPath = string.IsNullOrWhiteSpace(settings.Build?.OutputPath)
			? Constants.DEFAULT_OUTPUT_PATH
			: FormatPath(settings.Build.OutputPath);

		string assemblyName = string.IsNullOrWhiteSpace(settings.Build?.AssemblyName)
			? Constants.DEFAULT_NAME
			: settings.Build.AssemblyName;

		return Path.GetFullPath(Path.Combine(rootPath, targetPath, assemblyName));

		static string FormatPath(string template)
		{
			string configuration =
#if RELEASE
				"Release";
#else
				"Debug";
#endif

			return template
				.Replace("/", "\\")
				.Replace("${Configuration}", configuration);
		}
	}
}
