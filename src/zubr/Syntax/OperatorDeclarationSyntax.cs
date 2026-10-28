using Zubr.Text;

namespace Zubr.Syntax;

public sealed class OperatorDeclarationSyntax : CommonFunctionDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public override Token Keyword { get; }

	public Token Operator { get; }

	public override ParameterListSyntax ParameterList { get; }

	public TypeSyntax? ReturnType { get; }

	public override BlockSyntax? Body { get; }

	public override ArrowExpressionClauseSyntax? ExpressionBody { get; }

	public override Token Semicolon { get; }

	internal OperatorDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token keyword,
		Token @operator,
		ParameterListSyntax parameterList,
		TypeSyntax? returnType,
		BlockSyntax? body,
		ArrowExpressionClauseSyntax? expressionBody,
		Token semicolon
	) : base(tree, span, SyntaxKind.OperatorDeclaration)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Keyword = keyword;
		Operator = @operator;
		ParameterList = parameterList;
		ReturnType = returnType;
		Body = body;
		ExpressionBody = expressionBody;
		Semicolon = semicolon;

		SetParent(attributes);
		SetParent(parameterList);
		SetParentIfNotNull(returnType);
		SetParentIfNotNull(body);
		SetParentIfNotNull(expressionBody);
	}

	public override string ToString()
	{
		if (Body is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword} {Operator}{ParameterList}{LeadingSpace(ReturnType)} {Body}";
		}

		if (ExpressionBody is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword} {Operator}{ParameterList}{LeadingSpace(ReturnType)} {ExpressionBody}{Semicolon}";
		}

		return $"{TrailingSpace(Modifiers)}{Keyword} {Operator}{ParameterList}{LeadingSpace(ReturnType)}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitOperatorDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitOperatorDeclaration(this);
}
