using Zubr.Text;

namespace Zubr.Syntax;

public sealed class DiscardExpressionSyntax : ExpressionSyntax
{
	public Token Discard { get; }

	internal DiscardExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token discard
	) : base(tree, span, SyntaxKind.DiscardExpression)
	{
		Discard = discard;
	}

	public override string ToString()
	{
		return $"{Discard}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitDiscardExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitDiscardExpression(this);
}
