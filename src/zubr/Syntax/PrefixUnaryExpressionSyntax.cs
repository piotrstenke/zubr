using Zubr.Text;

namespace Zubr.Syntax;

public sealed class PrefixUnaryExpressionSyntax : ExpressionSyntax
{
	public Token Operator { get; }

	public ExpressionSyntax Operand { get; }

	internal PrefixUnaryExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxKind kind,
		Token @operator,
		ExpressionSyntax operand
	) : base(tree, span, kind)
	{
		Operator = @operator;
		Operand = operand;

		SetParent(operand);
	}

	public override string ToString()
	{
		return $"{Operator}{Operand}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitPrefixUnaryExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitPrefixUnaryExpression(this);
}
