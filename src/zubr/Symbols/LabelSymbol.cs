namespace Zubr.Symbols;

public sealed class LabelSymbol : Symbol
{
	public override SymbolKind BaseKind => SymbolKind.Label;

	public override Accessibility Accessibility => Accessibility.None;

	internal LabelSymbol(string name) : base(name)
	{
	}
}
