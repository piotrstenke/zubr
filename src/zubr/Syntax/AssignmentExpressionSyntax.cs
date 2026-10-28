using Zubr.Text;

namespace Zubr.Syntax;

public sealed class AssignmentExpressionSyntax : ExpressionSyntax
{
	public ExpressionSyntax Left { get; }

	public Token Operator { get; }

	public ExpressionSyntax Right { get; }

	internal AssignmentExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		ExpressionSyntax left,
		Token @operator,
		ExpressionSyntax right
	) : base(tree, span, SyntaxKind.AssignmentExpression)
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

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAssignmentExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAssignmentExpression(this);
}
