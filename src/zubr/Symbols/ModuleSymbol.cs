using System.Collections.Generic;

namespace Zubr.Symbols;

public sealed class ModuleSymbol : ModuleOrTypeSymbol
{
	private readonly List<Symbol> _members = new();

	public override SymbolKind BaseKind => SymbolKind.Module;

	public override Accessibility Accessibility => Accessibility.None;

	public bool IsTop => Parent is AssemblySymbol;

	internal ModuleSymbol(string name) : base(name)
	{
	}

	public override IEnumerable<Symbol> GetMembers()
	{
		return _members;
	}

	internal void AddMember(Symbol symbol)
	{
		_members.Add(symbol);
		SetParent(symbol);
	}
}
