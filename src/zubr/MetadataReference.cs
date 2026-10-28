using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Zubr;

[DebuggerDisplay("{Display,nq}")]
public abstract class MetadataReference
{
	public abstract string Display { get; }

	public ImmutableArray<string> Aliases => Options.Aliases;

	internal ReferenceOptions Options { get; }

	private protected MetadataReference(ReferenceOptions options)
	{
		Options = options;
	}

	public sealed override string ToString()
	{
		return Display;
	}

	public static Builder<ExecutableReference> FromStream(Stream stream, string? display = null)
	{
		ArgumentNullException.ThrowIfNull(stream);

		return new(options => new ExecutableReference(null, options, display));
	}

	public static Builder<ExecutableReference> FromData(byte[] data, string? display = null)
	{
		ArgumentNullException.ThrowIfNull(data);

		return new(options => new ExecutableReference(null, options, display));
	}

	public static Builder<ExecutableReference> FromFile(string path, string? display = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(path);

		return new(options => new ExecutableReference(path, options, display));
	}

	public Builder<MetadataReference> AddAliases(string alias)
	{
		return CreateBuilder().AddAliases(alias);
	}

	public Builder<MetadataReference> AddAliases(params string[]? aliases)
	{
		return CreateBuilder().AddAliases(aliases);
	}

	public Builder<MetadataReference> AddAliases(params IEnumerable<string>? aliases)
	{
		return CreateBuilder().AddAliases(aliases);
	}

	public Builder<MetadataReference> WithAliases(params string[]? aliases)
	{
		return CreateBuilder().WithAliases(aliases);
	}

	public Builder<MetadataReference> WithAliases(params IEnumerable<string>? aliases)
	{
		return CreateBuilder().WithAliases(aliases);
	}

	private Builder<MetadataReference> CreateBuilder()
	{
		return new(Create, this);
	}

	private protected abstract MetadataReference Create(ReferenceOptions options);

	internal readonly record struct ReferenceOptions
	{
		public ImmutableArray<string> Aliases { get; }

		public ReferenceOptions(ImmutableArray<string> aliases)
		{
			Aliases = aliases;
		}
	}

	public sealed class Builder<T> : IBuilder<T> where T : MetadataReference
	{
		private readonly Func<ReferenceOptions, T> _ctor;

		private List<string>? _aliases;

		internal Builder(Func<ReferenceOptions, T> ctor)
		{
			_ctor = ctor;
		}

		internal Builder(Func<ReferenceOptions, T> ctor, T original)
		{
			_ctor = ctor;

			_aliases = original.Aliases.ToList();
		}

		public Builder<T> AddAliases(string alias)
		{
			_aliases ??= new();
			AddAliases_Internal(alias);

			return this;
		}

		public Builder<T> AddAliases(params string[]? aliases)
		{
			if (aliases is not null)
			{
				_aliases ??= new(aliases.Length);

				foreach (string alias in aliases)
				{
					AddAliases_Internal(alias);
				}
			}

			return this;
		}

		public Builder<T> AddAliases(params IEnumerable<string>? aliases)
		{
			if (aliases is not null)
			{
				CollectionHelpers.InitWithCapacity(ref _aliases, aliases);

				foreach (string alias in aliases)
				{
					AddAliases_Internal(alias);
				}
			}

			return this;
		}

		public Builder<T> WithAliases(params string[]? aliases)
		{
			if (aliases is null || aliases.Length == 0)
			{
				_aliases?.Clear();
			}
			else
			{
				CollectionHelpers.NewOrClear(ref _aliases, aliases.Length);

				foreach (string alias in aliases)
				{
					AddAliases(alias);
				}
			}

			return this;
		}

		public Builder<T> WithAliases(params IEnumerable<string>? aliases)
		{
			if (aliases is null)
			{
				_aliases?.Clear();
			}
			else
			{
				CollectionHelpers.NewOrClear(ref _aliases);

				foreach (string alias in aliases)
				{
					AddAliases(alias);
				}
			}

			return this;
		}

		public T Build()
		{
			return _ctor(new(_aliases?.ToImmutableArray() ?? []));
		}
		
		public static implicit operator T(Builder<T> self)
		{
			return self.Build();
		}

		private void AddAliases_Internal(string alias)
		{
			ValidateAlias(alias);
			_aliases!.Add(alias);
		}

		private void ValidateAlias(string alias)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(alias);

			if(!SyntaxFacts.IsValidIdentifier(alias))
			{
				throw new ArgumentException($"Alias '{alias}' is not a valid identifier.");
			}

			int length = _aliases!.Count;

			for (int i = 0; i < length; i++)
			{
				if (_aliases[i].Equals(alias, StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException($"Alias '{alias}' already added to the metadata.");
				}
			}
		}
	}
}
