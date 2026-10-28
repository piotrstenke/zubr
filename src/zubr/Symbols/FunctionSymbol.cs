using System.Collections.Immutable;

namespace Zubr.Symbols;

public sealed class FunctionSymbol : Symbol
{
	public override SymbolKind BaseKind => SymbolKind.Function;

	public override Accessibility Accessibility { get; }

	public FunctionKind Kind { get; }

	public TypeSymbol? ReturnType { get; }

	public bool IsGeneric => Arity > 0;

	public int Arity => TypeParameters.Length;

	public ImmutableArray<TypeParameterSymbol> TypeParameters { get; }

	public ImmutableArray<TypeSymbol> TypeArguments { get; }

	public ImmutableArray<ParameterSymbol> Parameters { get; }

	public FunctionSymbol? Overridden { get; }

	public FunctionSymbol? ConstructedFrom { get; }

	public new Symbol Parent => base.Parent!;

	internal FunctionSymbol(
		FunctionKind kind,
		string name,
		TypeSymbol? returnType,
		Accessibility accessibility,
		ImmutableArray<ParameterSymbol> parameters,
		ImmutableArray<TypeParameterSymbol> typeParameters,
		ImmutableArray<TypeSymbol> typeArguments,
		FunctionSymbol? overridden,
		FunctionSymbol? constructedFrom
	) : base(name)
	{
		Kind = kind;
		ReturnType = returnType;
		Accessibility = accessibility;
		Parameters = parameters;
		TypeParameters = typeParameters;
		TypeArguments = typeArguments;
		Overridden = overridden;
		ConstructedFrom = constructedFrom;

		SetParent(typeParameters);
		SetParent(parameters);
	}

	//public FunctionSymbol Construct(params TypeSymbol[] typeArguments)
	//{
	//	if (!IsGeneric)
	//	{
	//		throw new SymbolConstuctionException(ErrorMessages.NON_GENERIC_CONSTRUCT);
	//	}

	//	if (typeArguments is null || TypeParameters.Length != typeArguments.Length)
	//	{
	//		throw new SymbolConstuctionException(ErrorMessages.WRONG_ARGS_COUNT);
	//	}

	//	if (ConstructedFrom is not null)
	//	{
	//		throw new SymbolConstuctionException(ErrorMessages.NOT_ORIGINAL_CONSTRUCT);
	//	}

	//	return new(Kind, Name, ReturnType, Parameters, TypeParameters, ImmutableArray.Create(typeArguments), Overridden, this);
	//}
}
