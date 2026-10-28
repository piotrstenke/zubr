using System.Collections.Immutable;

namespace Zubr.Symbols;

public sealed class TypeParameterSymbol : TypeSymbol
{
	public override SymbolKind BaseKind => SymbolKind.TypeParameter;

	public override TypeKind Kind => TypeKind.TypeParameter;

	public override Accessibility Accessibility => Accessibility.None;

	internal TypeParameterSymbol(string name) : base(name)
	{
	}
}
