using Zubr.Text;

namespace Zubr.Syntax;

public sealed class AttributeSyntax : SyntaxNode
{
	public Token OpenBracket { get; }

	public AttributeTargetSyntax? Target { get; }

	public NameSyntax Name { get; }

	public AttributeArgumentListSyntax? ArgumentList { get; }

	public Token CloseBracket { get; }

	internal AttributeSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openBracket,
		AttributeTargetSyntax? target,
		NameSyntax name,
		AttributeArgumentListSyntax? argumentList,
		Token closeBracket
	) : base(tree, span, SyntaxKind.Attribute)
	{
		OpenBracket = openBracket;
		Target = target;
		Name = name;
		ArgumentList = argumentList;
		CloseBracket = closeBracket;

		SetParentIfNotNull(target);
		SetParent(name);
		SetParentIfNotNull(argumentList);
	}

	public override string ToString()
	{
		return $"{OpenBracket}{TrailingSpace(Target)}{Name}{ArgumentList}{CloseBracket}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAttribute(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAttribute(this);
}
