using Zubr.Text;

namespace Zubr.Syntax;

public sealed class IndexerDeclarationSyntax : CommonPropertyDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public Token Self { get; }

	public BracketParameterListSyntax ParameterList { get; }

	public override TypeSpecifierSyntax? TypeSpecifier { get; }

	public override ArrowExpressionClauseSyntax? ExpressionBody { get; }

	public override AccessorListSyntax? AccessorList { get; }

	public override Token Semicolon { get; }

	internal IndexerDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token self,
		BracketParameterListSyntax parameterList,
		TypeSpecifierSyntax? typeSpecifier,
		ArrowExpressionClauseSyntax? expressionBody,
		AccessorListSyntax? accessorList,
		Token semicolon
	) : base(tree, span, SyntaxKind.IndexerDeclaration)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Self = self;
		ParameterList = parameterList;
		TypeSpecifier = typeSpecifier;
		ExpressionBody = expressionBody;
		AccessorList = accessorList;
		Semicolon = semicolon;

		SetParent(attributes);
		SetParent(parameterList);
		SetParentIfNotNull(typeSpecifier);
		SetParentIfNotNull(expressionBody);
		SetParentIfNotNull(accessorList);
	}

	public override string ToString()
	{
		if (ExpressionBody is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Self}{ParameterList}{TypeSpecifier} {ExpressionBody}{Semicolon}";
		}

		return $"{TrailingSpace(Modifiers)}{Self}{ParameterList}{TypeSpecifier}{LeadingSpace(AccessorList)}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitIndexerDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitIndexerDeclaration(this);
}
