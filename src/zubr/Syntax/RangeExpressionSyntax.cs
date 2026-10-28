using Zubr.Text;

namespace Zubr.Syntax;

public sealed class RangeExpressionSyntax : ExpressionSyntax
{
	public ExpressionSyntax? Left { get; }

	public Token Range { get; }

	public Token Comparison { get; }

	public ExpressionSyntax? Right { get; }

	internal RangeExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		ExpressionSyntax? left,
		Token range,
		Token comparison,
		ExpressionSyntax? right
	) : base(tree, span, SyntaxKind.RangeExpression)
	{
		Left = left;
		Range = range;
		Comparison = comparison;
		Right = right;

		SetParentIfNotNull(left);
		SetParentIfNotNull(right);
	}

	public override string ToString()
	{
		return $"{Left}{Range}{Comparison}{Right}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitRangeExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitRangeExpression(this);
}
