using Zubr.Text;

namespace Zubr.Syntax;

public sealed class PropertyDeclarationSyntax : CommonPropertyDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public Token Identifier { get; }

	public override TypeSpecifierSyntax? TypeSpecifier { get; }

	public override ArrowExpressionClauseSyntax? ExpressionBody { get; }

	public override AccessorListSyntax? AccessorList { get; }

	public EqualsValueClauseSyntax? Initializer { get; }

	public override Token Semicolon { get; }

	internal PropertyDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token identifier,
		TypeSpecifierSyntax? typeSpecifier,
		ArrowExpressionClauseSyntax? expressionBody,
		AccessorListSyntax? accessorList,
		EqualsValueClauseSyntax? initializer,
		Token semicolon
	) : base(tree, span, SyntaxKind.PropertyDeclaration)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		TypeSpecifier = typeSpecifier;
		Identifier = identifier;
		ExpressionBody = expressionBody;
		AccessorList = accessorList;
		Initializer = initializer;
		Semicolon = semicolon;

		SetParent(attributes);
		SetParentIfNotNull(typeSpecifier);
		SetParentIfNotNull(expressionBody);
		SetParentIfNotNull(accessorList);
		SetParentIfNotNull(initializer);
	}

	public override string ToString()
	{
		if (ExpressionBody is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Identifier}{TypeSpecifier} {ExpressionBody}{Semicolon}";
		}

		return $"{TrailingSpace(Modifiers)}{Identifier}{TypeSpecifier}{LeadingSpace(AccessorList)}{LeadingSpace(Initializer)}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitPropertyDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitPropertyDeclaration(this);
}
