using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ConstructorDeclarationSyntax : CommonFunctionDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public override Token Keyword { get; }

	public override ParameterListSyntax ParameterList { get; }

	public override BlockSyntax? Body { get; }

	public override ArrowExpressionClauseSyntax? ExpressionBody { get; }

	public override Token Semicolon { get; }

	internal ConstructorDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token keyword,
		ParameterListSyntax parameterList,
		BlockSyntax? body,
		ArrowExpressionClauseSyntax? expressionBody,
		Token semicolon
	) : base(tree, span, SyntaxKind.ConstructorDeclaration)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Keyword = keyword;
		ParameterList = parameterList;
		Body = body;
		ExpressionBody = expressionBody;
		Semicolon = semicolon;

		SetParent(attributes);
		SetParent(parameterList);
		SetParentIfNotNull(body);
		SetParentIfNotNull(expressionBody);
	}

	public override string ToString()
	{
		if (Body is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword}{ParameterList} {Body}";
		}

		if (ExpressionBody is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword}{ParameterList} {ExpressionBody}{Semicolon}";
		}

		return $"{TrailingSpace(Modifiers)}{Keyword}{ParameterList}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitConstructorDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitConstructorDeclaration(this);
}
