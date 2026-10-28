using Zubr.Text;

namespace Zubr.Syntax;

public sealed class AssociatedTypeDeclarationSyntax : MemberDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public Token Keyword { get; }

	public Token Identifier { get; }

	public EqualsTypeClauseSyntax? EqualsClause { get; }

	public Token Semicolon { get; }

	public AssociatedTypeDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token keyword,
		Token identifier,
		EqualsTypeClauseSyntax? equalsClause,
		Token semicolon
	) : base(tree, span, SyntaxKind.AssociatedTypeDeclaration)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Keyword = keyword;
		Identifier = identifier;
		EqualsClause = equalsClause;
		Semicolon = semicolon;

		SetParent(attributes);
		SetParentIfNotNull(equalsClause);
	}

	public override string ToString()
	{
		return $"{TrailingSpace(Modifiers)}{Keyword} {Identifier}{LeadingSpace(EqualsClause)}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAssociatedTypeDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAssociatedTypeDeclaration(this);
}
