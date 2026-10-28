using Zubr.Text;

namespace Zubr.Syntax;

public sealed class CastDeclarationSyntax : CommonFunctionDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public override Token Keyword { get; }

	public override ParameterListSyntax ParameterList { get; }

	public TypeSyntax ReturnType { get; }

	public override BlockSyntax? Body { get; }

	public override ArrowExpressionClauseSyntax? ExpressionBody { get; }

	public override Token Semicolon { get; }

	internal CastDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token keyword,
		ParameterListSyntax parameterList,
		TypeSyntax returnType,
		BlockSyntax? body,
		ArrowExpressionClauseSyntax? expressionBody,
		Token semicolon
	) : base(tree, span, SyntaxKind.CastDeclaration)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Keyword = keyword;
		ParameterList = parameterList;
		ReturnType = returnType;
		Body = body;
		ExpressionBody = expressionBody;
		Semicolon = semicolon;

		SetParent(attributes);
		SetParent(parameterList);
		SetParent(returnType);
		SetParentIfNotNull(body);
		SetParentIfNotNull(expressionBody);
	}

	public override string ToString()
	{
		if (Body is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword}{ParameterList} {ReturnType} {Body}";
		}

		if (ExpressionBody is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword}{ParameterList} {ReturnType} {ExpressionBody}{Semicolon}";
		}

		return $"{TrailingSpace(Modifiers)}{Keyword}{ParameterList} {ReturnType}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitCastDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitCastDeclaration(this);
}
