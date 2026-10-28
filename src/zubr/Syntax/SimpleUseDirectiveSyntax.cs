using Zubr.Text;

namespace Zubr.Syntax;

public sealed class SimpleUseDirectiveSyntax : UseDirectiveSyntax
{
	public override Token Top { get; }

	public override Token Use { get; }

	public NameSyntax Name { get; }

	public override AliasSpecifierSyntax? Alias { get; }

	public override Token Semicolon { get; }

	public SimpleUseDirectiveSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token top,
		Token use,
		NameSyntax name,
		AliasSpecifierSyntax? alias,
		Token semicolon
	) : base(tree, span, SyntaxKind.SimpleUseDirective)
	{
		Top = top;
		Use = use;
		Name = name;
		Alias = alias;
		Semicolon = semicolon;

		SetParent(name);
		SetParentIfNotNull(alias);
	}

	public override string ToString()
	{
		return $"{TrailingSpace(Top)}{Use} {Name}{LeadingSpace(Alias)}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitSimpleUseDirective(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitSimpleUseDirective(this);
}
