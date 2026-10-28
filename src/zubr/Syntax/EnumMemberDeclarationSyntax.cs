using Zubr.Text;

namespace Zubr.Syntax;

public sealed class EnumMemberDeclarationSyntax : MemberDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public Token Identifier { get; }

	public EqualsValueClauseSyntax? Initializer { get; }

	internal EnumMemberDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token identifier,
		EqualsValueClauseSyntax? initializer
	) : base(tree, span, SyntaxKind.EnumMemberDeclaration)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Identifier = identifier;
		Initializer = initializer;

		SetParent(attributes);
		SetParentIfNotNull(initializer);
	}

	public override string ToString()
	{
		return $"{TrailingSpace(Modifiers)}{Identifier}{LeadingSpace(Initializer)}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitEnumMemberDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitEnumMemberDeclaration(this);
}
