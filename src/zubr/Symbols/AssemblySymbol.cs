namespace Zubr.Symbols;

public sealed class AssemblySymbol : Symbol
{
	public override SymbolKind BaseKind => SymbolKind.Assembly;

	public override Accessibility Accessibility => Accessibility.None;

	public ModuleSymbol Top { get; }

	internal AssemblySymbol(
		string name, 
		ModuleSymbol top
	) : base(name)
	{
		Top = top;

		SetParent(top);
	}
}
