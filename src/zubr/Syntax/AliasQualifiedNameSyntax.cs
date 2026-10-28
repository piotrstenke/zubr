using Zubr.Text;

namespace Zubr.Syntax;

public sealed class AliasQualifiedNameSyntax : NameSyntax
{
	public IdentifierNameSyntax Alias { get; }

	public Token ColonColon { get; }

	public SimpleNameSyntax Name { get; }

	internal AliasQualifiedNameSyntax(
		SyntaxTree tree,
		TextSpan span,
		IdentifierNameSyntax alias,
		Token colonColon,
		SimpleNameSyntax name
	) : base(tree, span, SyntaxKind.AliasQualifiedName)
	{
		Alias = alias;
		ColonColon = colonColon;
		Name = name;

		SetParent(alias);
		SetParent(name);
	}

	public override string ToString()
	{
		return $"{Alias}{ColonColon}{Name}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAliasQualifiedName(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAliasQualifiedName(this);

	internal override SimpleNameSyntax GetUnqualifiedName()
	{
		return Name;
	}
}
