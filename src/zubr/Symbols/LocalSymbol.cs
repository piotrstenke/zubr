namespace Zubr.Symbols;

public sealed class LocalSymbol : Symbol
{
	public override SymbolKind BaseKind => SymbolKind.Local;

	public override Accessibility Accessibility => Accessibility.None;

	public TypeSymbol Type { get; }

	internal LocalSymbol(
		string name,
		TypeSymbol type
	) : base(name)
	{
		Type = type;
	}
}
