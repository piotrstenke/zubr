using System.Collections.Generic;
using Zubr.Symbols;

namespace Zubr.Binding;

internal sealed class Scope
{
	private readonly Dictionary<string, Symbol> _symbols = new();

	public Scope? Parent { get; }

	public Scope(Scope? parent)
	{
		Parent = parent;
	}

	public bool AddSymbol(Symbol symbol)
	{
		return _symbols.TryAdd(symbol.Name, symbol);
	}

	public Symbol? Lookup(string name)
	{
		Scope? current = this;

		do
		{
			if(current._symbols.TryGetValue(name, out Symbol? symbol))
			{
				return symbol;
			}

			current = current.Parent;
		}
		while (current is not null);

		return null;
	}

	public TSymbol? Lookup<TSymbol>(string name) where TSymbol : Symbol
	{
		Symbol? symbol = Lookup(name);
		return symbol as TSymbol;
	}
}
