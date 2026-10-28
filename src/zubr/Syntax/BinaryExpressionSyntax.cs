using Zubr.Text;

namespace Zubr.Syntax;

public sealed class BinaryExpressionSyntax : ExpressionSyntax
{
	public ExpressionSyntax Left { get; }

	public Token Operator { get; }

	public ExpressionSyntax Right { get; }

	internal BinaryExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxKind kind,
		ExpressionSyntax left,
		Token @operator,
		ExpressionSyntax right
	) : base(tree, span, kind)
	{
		Left = left;
		Operator = @operator;
		Right = right;

		SetParent(left);
		SetParent(right);
	}

	public override string ToString()
	{
		return $"{Left} {Operator} {Right}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitBinaryExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitBinaryExpression(this);
}
