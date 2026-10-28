using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ConditionalAccessExpressionSyntax : ExpressionSyntax
{
	public ExpressionSyntax Left { get; }

	public Token Operator { get; }

	public ExpressionSyntax Right { get; }

	internal ConditionalAccessExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		ExpressionSyntax left,
		Token @operator,
		ExpressionSyntax right
	) : base(tree, span, SyntaxKind.ConditionalAccessExpression)
	{
		Left = left;
		Operator = @operator;
		Right = right;

		SetParent(left);
		SetParent(right);
	}

	public override string ToString()
	{
		return $"{Left}{Operator}{Right}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitConditionalAccessExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitConditionalAccessExpression(this);
}
