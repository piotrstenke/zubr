namespace Zubr.Load;

public sealed class ProjectLoaderOptions
{
	public static ProjectLoaderOptions Default { get; } = new()
	{
		PackageFileStrategy = PackageFileStrategy.Default,
		DefaultScriptFileName = "_package.zr"
	};

	public required PackageFileStrategy PackageFileStrategy { get; set; }

	public required string DefaultScriptFileName { get; set; }
}
