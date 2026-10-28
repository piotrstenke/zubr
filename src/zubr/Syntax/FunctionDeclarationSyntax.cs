using Zubr.Text;

namespace Zubr.Syntax;

public sealed class FunctionDeclarationSyntax : CommonFunctionDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public override Token Keyword { get; }

	public Token Identifier { get; }

	public TypeParameterListSyntax? TypeParameterList { get; }

	public override ParameterListSyntax ParameterList { get; }

	public TypeSpecifierSyntax? ReturnType { get; }

	public SyntaxList<TypeParameterConstraintListSyntax> ConstraintList { get; }

	public override BlockSyntax? Body { get; }

	public override ArrowExpressionClauseSyntax? ExpressionBody { get; }

	public override Token Semicolon { get; }

	public int Arity => TypeParameterList?.Parameters.Count ?? 0;

	internal FunctionDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token keyword,
		Token identifier,
		TypeParameterListSyntax? typeParameterList,
		ParameterListSyntax parameterList,
		TypeSpecifierSyntax? returnType,
		SyntaxList<TypeParameterConstraintListSyntax> constraintList,
		BlockSyntax? body,
		ArrowExpressionClauseSyntax? expressionBody,
		Token semicolon
	) : base(tree, span, SyntaxKind.FunctionDeclaration)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Keyword = keyword;
		Identifier = identifier;
		TypeParameterList = typeParameterList;
		ParameterList = parameterList;
		ReturnType = returnType;
		ConstraintList = constraintList;
		Body = body;
		ExpressionBody = expressionBody;
		Semicolon = semicolon;

		SetParent(attributes);
		SetParent(parameterList);
		SetParentIfNotNull(returnType);
		SetParentIfNotNull(typeParameterList);
		SetParent(constraintList);
		SetParentIfNotNull(body);
		SetParentIfNotNull(expressionBody);
	}

	public override string ToString()
	{
		if (Body is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword} {Identifier}{TypeParameterList}{ParameterList}{LeadingSpace(ReturnType)}{LeadingSpace(ConstraintList)} {Body}";
		}

		if (ExpressionBody is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword} {Identifier}{TypeParameterList}{ParameterList}{LeadingSpace(ReturnType)}{LeadingSpace(ConstraintList)} {ExpressionBody}{Semicolon}";
		}

		return $"{TrailingSpace(Modifiers)}{Keyword} {Identifier}{TypeParameterList}{ParameterList}{LeadingSpace(ReturnType)}{LeadingSpace(ConstraintList)}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitFunctionDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitFunctionDeclaration(this);
}
