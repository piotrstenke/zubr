using System;
using System.Collections.Generic;

namespace Zubr;

public sealed class CompilationReference : MetadataReference, IEquatable<CompilationReference>
{
	public override string Display => Compilation.AssemblyName;

	public Compilation Compilation { get; }

	internal CompilationReference(Compilation compilation, ReferenceOptions options) : base(options)
	{
		Compilation = compilation;
	}

	public bool Equals(CompilationReference? other)
	{
		if(other is null)
		{
			return false;
		}

		return other.Compilation == Compilation && other.Options == Options;
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as CompilationReference);
	}

	public override int GetHashCode()
	{
		return Compilation.GetHashCode();
	}

	public new Builder<CompilationReference> AddAliases(string alias)
	{
		return CreateBuilder().AddAliases(alias);
	}

	public new Builder<CompilationReference> AddAliases(params string[]? aliases)
	{
		return CreateBuilder().AddAliases(aliases);
	}

	public new Builder<CompilationReference> AddAliases(params IEnumerable<string>? aliases)
	{
		return CreateBuilder().AddAliases(aliases);
	}

	public new Builder<CompilationReference> WithAliases(params string[]? aliases)
	{
		return CreateBuilder().WithAliases(aliases);
	}

	public new Builder<CompilationReference> WithAliases(params IEnumerable<string>? aliases)
	{
		return CreateBuilder().WithAliases(aliases);
	}

	private Builder<CompilationReference> CreateBuilder()
	{
		return new(Create, this);
	}

	private protected override CompilationReference Create(ReferenceOptions options)
	{
		return new CompilationReference(Compilation, options);
	}

	public static bool operator ==(CompilationReference? left, CompilationReference? right)
	{
		return left?.Equals(right) ?? false;
	}

	public static bool operator !=(CompilationReference? left, CompilationReference? right)
	{
		return !(left == right);
	}
}
