using Zubr.Text;

namespace Zubr.Syntax;

public sealed class PostfixUnaryExpressionSyntax : ExpressionSyntax
{
	public ExpressionSyntax Operand { get; }

	public Token Operator { get; }

	internal PostfixUnaryExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxKind kind,
		ExpressionSyntax operand,
		Token @operator
	) : base(tree, span, kind)
	{
		Operand = operand;
		Operator = @operator;

		SetParent(operand);
	}

	public override string ToString()
	{
		return $"{Operand}{Operator}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitPostfixUnaryExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitPostfixUnaryExpression(this);
}
