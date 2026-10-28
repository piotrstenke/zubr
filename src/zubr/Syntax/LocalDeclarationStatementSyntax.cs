using Zubr.Text;

namespace Zubr.Syntax;

public sealed class LocalDeclarationStatementSyntax : StatementSyntax
{
	public TokenList Modifiers { get; }

	public Token Identifier { get; }

	public TypeSpecifierSyntax? TypeSpecifier { get; }

	public EqualsValueClauseSyntax? Initializer { get; }

	public Token Semicolon { get; }

	internal LocalDeclarationStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		TokenList modifiers,
		Token identifier,
		TypeSpecifierSyntax? typeSpecifier,
		EqualsValueClauseSyntax? initializer,
		Token semicolon
	) : base(tree, span, SyntaxKind.LocalDeclarationStatement)
	{
		Modifiers = modifiers;
		Identifier = identifier;
		TypeSpecifier = typeSpecifier;
		Initializer = initializer;
		Semicolon = semicolon;

		SetParentIfNotNull(typeSpecifier);
		SetParentIfNotNull(initializer);
	}

	public override string ToString()
	{
		return $"{TrailingSpace(Modifiers)}{Identifier}{TypeSpecifier}{LeadingSpace(Initializer)}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitLocalDeclarationStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitLocalDeclarationStatement(this);
}
