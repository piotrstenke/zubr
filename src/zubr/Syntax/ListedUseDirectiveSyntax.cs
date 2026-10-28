using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ListedUseDirectiveSyntax : UseDirectiveSyntax
{
	public override Token Top { get; }

	public override Token Use { get; }

	public UseDirectiveElementListSyntax List { get; }

	public Token From { get; }

	public NameSyntax Name { get; }

	public override AliasSpecifierSyntax? Alias { get; }

	public override Token Semicolon { get; }

	internal ListedUseDirectiveSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token top,
		Token use,
		UseDirectiveElementListSyntax list,
		Token from,
		NameSyntax name,
		AliasSpecifierSyntax? alias,
		Token semicolon
	) : base(tree, span, SyntaxKind.ListedUseDirective)
	{
		Top = top;
		Use = use;
		List = list;
		From = from;
		Name = name;
		Alias = alias;
		Semicolon = semicolon;

		SetParent(list);
		SetParent(name);
		SetParentIfNotNull(alias);
	}

	public override string ToString()
	{
		return $"{TrailingSpace(Top)}{Use} {List}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitListedUseDirective(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitListedUseDirective(this);
}
