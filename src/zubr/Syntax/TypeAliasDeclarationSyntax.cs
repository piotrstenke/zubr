using Zubr.Text;

namespace Zubr.Syntax;

public sealed class TypeAliasDeclarationSyntax : MemberDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public Token Alias { get; }

	public NameSyntax Name { get; }

	public EqualsTypeClauseSyntax EqualsClause { get; }

	public Token Semicolon { get; }

	internal TypeAliasDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token alias,
		NameSyntax name,
		EqualsTypeClauseSyntax equalsClause,
		Token semicolon
	) : base(tree, span, SyntaxKind.TypeAliasDeclaration)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Alias = alias;
		Name = name;
		EqualsClause = equalsClause;
		Semicolon = semicolon;

		SetParent(attributes);
		SetParent(name);
		SetParent(equalsClause);
	}

	public override string ToString()
	{
		return $"{Alias} {Name} {EqualsClause}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAliasDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAliasDeclaration(this);
}
