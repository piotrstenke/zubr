using Zubr.Text;

namespace Zubr.Syntax;

public sealed class AttributeTargetSyntax : SyntaxNode
{
	public Token Keyword { get; }

	public Token Colon { get; }

	internal AttributeTargetSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token keyword,
		Token colon
	) : base(tree, span, SyntaxKind.AttributeTarget)
	{
		Keyword = keyword;
		Colon = colon;
	}

	public override string ToString()
	{
		return $"{Keyword}{Colon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAttributeTarget(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAttributeTarget(this);
}
