using System.Collections.Generic;

namespace Zubr.Symbols;

public abstract class ModuleOrTypeSymbol : Symbol
{
	internal ModuleOrTypeSymbol(string name) : base(name)
	{
	}

	public abstract IEnumerable<Symbol> GetMembers();
}
