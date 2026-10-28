using System.Collections.Immutable;

namespace Zubr.Symbols;

public sealed class PropertySymbol : Symbol
{
	public override SymbolKind BaseKind => SymbolKind.Property;

	public override Accessibility Accessibility { get; }

	public bool IsIndexer { get; }

	public bool IsRequired { get; }

	public TypeSymbol Type { get; }

	public ImmutableArray<ParameterSymbol> Parameters { get; }

	public FunctionSymbol? Get { get; }

	public FunctionSymbol? Set { get; }

	public PropertySymbol? Overridden { get; }

	internal PropertySymbol(
		string name,
		TypeSymbol type,
		Accessibility accesibility,
		FunctionSymbol? get,
		FunctionSymbol? set,
		PropertySymbol? overridden,
		bool isIndexer,
		ImmutableArray<ParameterSymbol> parameters,
		bool isRequired
	) : base(name)
	{
		Type = type;
		Accessibility = accesibility;
		Parameters = parameters;
		Get = get;
		Set = set;
		Overridden = overridden;
		IsIndexer = isIndexer;
		IsRequired = isRequired;

		SetParentIfNotNull(get);
		SetParentIfNotNull(set);
		SetParent(parameters);
	}
}
