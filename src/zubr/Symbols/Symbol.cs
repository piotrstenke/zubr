using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Zubr.Symbols;

public abstract class Symbol
{
	/// <summary>
	/// Kind of symbol represented by this instance.
	/// </summary>
	public abstract SymbolKind BaseKind { get; }

	/// <summary>
	/// Name of the symbol.
	/// </summary>
	public string Name { get; }

	public abstract Accessibility Accessibility { get; }

	/// <summary>
	/// Gets the containing symbol of this symbol, or null if there is no parent.
	/// </summary>
	public Symbol? Parent { get; private set; }

	internal Symbol(string name)
	{
		Name = name;
	}

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>A string that represents the current object.
	public override string ToString()
	{
		return Name;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected void SetParent(Symbol symbol)
	{
		symbol.Parent = this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected void SetParent<T>(ImmutableArray<T> symbols) where T : Symbol
	{
		foreach (Symbol symbol in symbols)
		{
			symbol.Parent = this;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected void SetParentIfNotNull(Symbol? symbol)
	{
		symbol?.Parent = this;
	}
}
