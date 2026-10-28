using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ParameterSyntax : SyntaxNode
{
	public SyntaxList<AttributeSyntax> Attributes { get; }

	public TokenList Modifiers { get; }

	public Token Identifier { get; }

	public TypeSpecifierSyntax TypeSpecifier { get; }

	public EqualsValueClauseSyntax? Default { get; }

	internal ParameterSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token identifier,
		TypeSpecifierSyntax typeSpecifier,
		EqualsValueClauseSyntax? @default
	) : base(tree, span, SyntaxKind.Parameter)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Identifier = identifier;
		TypeSpecifier = typeSpecifier;
		Default = @default;

		SetParent(attributes);
		SetParent(typeSpecifier);
		SetParentIfNotNull(@default);
	}

	public override string ToString()
	{
		return $"{TrailingSpace(Modifiers)}{Identifier}{TypeSpecifier}{LeadingSpace(Default)}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitParameter(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitParameter(this);
}
