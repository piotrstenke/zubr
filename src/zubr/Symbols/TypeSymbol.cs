using System.Collections.Generic;

namespace Zubr.Symbols;

public abstract class TypeSymbol : ModuleOrTypeSymbol
{
	public override SymbolKind BaseKind => SymbolKind.Type;

	public abstract TypeKind Kind { get; }

	internal TypeSymbol(string name) : base(name)
	{
	}

	public override IEnumerable<Symbol> GetMembers()
	{
		return [];
	}
}
