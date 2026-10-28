namespace Zubr;

public sealed record class Dependecy
{
	public required string Name { get; init; }

	public required string Version { get; init; }

	public override string ToString()
	{
		return $"{Name}:{Version}";
	}
}
