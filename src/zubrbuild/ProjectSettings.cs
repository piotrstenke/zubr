namespace Zubr;

public sealed class ProjectSettings
{
	public MetadataSettings? Metadata { get; set; }

	public BuildSettings? Build { get; set; }

	public Dependecy[]? Dependencies { get; set; }

	public sealed class MetadataSettings
	{
		public string? Name { get; set; }

		public string? Description { get; set; }

		public string[]? Tags { get; set; }

		public string? Version { get; set; }

		public string[]? Authors { get; set; }

		public string? Url { get; set; }

		public string? License { get; set; }
	}

	public sealed class BuildSettings
	{
		public OutputKind OutputKind { get; set; }

		public string? OutputPath { get; set; }

		public string? AssemblyName { get; set; }

		public string? Target { get; set; }
	}
}
