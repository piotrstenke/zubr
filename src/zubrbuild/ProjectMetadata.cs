using System.Collections.Generic;

namespace Zubr;

public sealed class ProjectMetadata
{
	public required string Name { get; init; }

	public required string Version { get; init; }

	public string? Url { get; init; }

	public required IReadOnlyList<string> Authors { get; init; }

	public required TargetVersion TargetVersion { get; init; }

	public required IReadOnlyList<Dependecy> Dependencies { get; init; }
}
