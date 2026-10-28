using System.Collections.Generic;

namespace Zubr;

public sealed class ExecutableReference : MetadataReference
{
	public override string Display { get; }

	public string? Path { get; }

	internal ExecutableReference(string? path, ReferenceOptions options, string? display) : base(options)
	{
		Display = GetDisplay(display, path);
		Path = path;
	}

	public new Builder<ExecutableReference> AddAliases(string alias)
	{
		return CreateBuilder().AddAliases(alias);
	}

	public new Builder<ExecutableReference> AddAliases(params string[]? aliases)
	{
		return CreateBuilder().AddAliases(aliases);
	}

	public new Builder<ExecutableReference> AddAliases(params IEnumerable<string>? aliases)
	{
		return CreateBuilder().AddAliases(aliases);
	}
	
	public new Builder<ExecutableReference> WithAliases(params string[]? aliases)
	{
		return CreateBuilder().WithAliases(aliases);
	}

	public new Builder<ExecutableReference> WithAliases(params IEnumerable<string>? aliases)
	{
		return CreateBuilder().WithAliases(aliases);
	}

	private Builder<ExecutableReference> CreateBuilder()
	{
		return new(Create, this);
	}

	private protected override ExecutableReference Create(ReferenceOptions options)
	{
		return new ExecutableReference(Path, options, Display);
	}

	private static string GetDisplay(string? custom, string? path)
	{
		if (custom is not null)
		{
			return custom;
		}

		if (path is not null)
		{
			return path;
		}

		return "in-memory";
	}
}
