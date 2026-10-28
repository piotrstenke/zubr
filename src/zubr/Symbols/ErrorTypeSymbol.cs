using System.Collections.Immutable;

namespace Zubr.Symbols;

public sealed class ErrorTypeSymbol : TypeSymbol
{
	public override SymbolKind BaseKind => SymbolKind.Error;

	public override TypeKind Kind => TypeKind.Error;

	public override Accessibility Accessibility => Accessibility.None;

	public ImmutableArray<Symbol> Candidates { get; }

	internal ErrorTypeSymbol(
		string name,
		ImmutableArray<Symbol> candidates
	) : base(name)
	{
		Candidates = candidates;
	}
}
