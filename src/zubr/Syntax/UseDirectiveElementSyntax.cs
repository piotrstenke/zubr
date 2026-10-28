using Zubr.Text;

namespace Zubr.Syntax;

public sealed class UseDirectiveElementSyntax : SyntaxNode
{
	public SimpleNameSyntax Name { get; }

	public AliasSpecifierSyntax? Alias { get; }

	internal UseDirectiveElementSyntax(
		SyntaxTree tree,
		TextSpan span,
		SimpleNameSyntax name,
		AliasSpecifierSyntax? alias
	) : base(tree, span, SyntaxKind.UseDirectiveElement)
	{
		Name = name;
		Alias = alias;

		SetParent(name);
		SetParentIfNotNull(alias);
	}

	public override string ToString()
	{
		return $"{Name}{LeadingSpace(Alias)}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitUseDirectiveElement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitUseDirectiveElement(this);
}
