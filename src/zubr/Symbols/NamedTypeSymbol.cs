using System.Collections.Immutable;

namespace Zubr.Symbols;

public sealed class NamedTypeSymbol : TypeSymbol
{
	public override TypeKind Kind { get; }

	public override Accessibility Accessibility { get; }

	public bool IsGeneric => Arity > 0;

	public int Arity => TypeParameters.Length;

	public ImmutableArray<TypeParameterSymbol> TypeParameters { get; }

	public ImmutableArray<TypeSymbol> TypeArguments { get; }

	public NamedTypeSymbol? ConstructedFrom { get; }

	public NamedTypeSymbol? BaseType { get; }

	public ImmutableArray<NamedTypeSymbol> Implemented { get; }

	internal NamedTypeSymbol(
		TypeKind kind,
		string name,
		Accessibility accessibility,
		ImmutableArray<TypeParameterSymbol> typeParameters,
		ImmutableArray<TypeSymbol> typeArguments,
		NamedTypeSymbol? constructedFrom,
		NamedTypeSymbol? baseType,
		ImmutableArray<NamedTypeSymbol> implemented
	) : base(name)
	{
		Kind = kind;
		Accessibility = accessibility;
		TypeParameters = typeParameters;
		TypeArguments = typeArguments;
		ConstructedFrom = constructedFrom;
		BaseType = baseType;
		Implemented = implemented;

		SetParent(typeParameters);
	}
}
