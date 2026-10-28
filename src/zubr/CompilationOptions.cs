namespace Zubr;

public sealed record CompilationOptions
{
	public static CompilationOptions Default { get; } = new()
	{
		LanguageVersion = LanguageVersion.Default,
		OutputKind = OutputKind.Lib
	};

	public LanguageVersion LanguageVersion { get; init; }

	public OutputKind OutputKind { get; init; }
}
