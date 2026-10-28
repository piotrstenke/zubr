using Zubr.Text;

namespace Zubr.Syntax;

public sealed class AliasSpecifierSyntax : SyntaxNode
{
	public Token As { get; }

	public SimpleNameSyntax Name { get; }

	public AliasSpecifierSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token @as,
		SimpleNameSyntax name
	) : base(tree, span, SyntaxKind.AliasSpecifier)
	{
		As = @as;
		Name = name;

		SetParent(name);
	}

	public override string ToString()
	{
		return $"{As} {Name}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAliasSpecifier(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAliasSpecifier(this);
}
