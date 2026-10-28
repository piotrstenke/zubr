namespace Zubr.Symbols;

public sealed class ParameterSymbol : Symbol
{
	public override SymbolKind BaseKind => SymbolKind.Parameter;

	public override Accessibility Accessibility { get; }

	public TypeSymbol Type { get; }

	public int Ordinal { get; }

	public bool IsOptional => DefaultValue is not null;

	public object? DefaultValue { get; }

	internal ParameterSymbol(
		string name,
		TypeSymbol type,
		Accessibility accessibility,
		int ordinal,
		object? defaultValue
	) : base(name)
	{
		Type = type;
		DefaultValue = defaultValue;
		Ordinal = ordinal;
		Accessibility = accessibility;
	}
}
