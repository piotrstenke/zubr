using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
{
	public Token OpenParen { get; }

	public ExpressionSyntax Expression { get; }

	public Token CloseParen { get; }

	internal ParenthesizedExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openParen,
		ExpressionSyntax expression,
		Token closeParen
	) : base(tree, span, SyntaxKind.ParenthesizedExpression)
	{
		OpenParen = openParen;
		Expression = expression;
		CloseParen = closeParen;

		SetParent(expression);
	}

	public override string ToString()
	{
		return $"{OpenParen}{Expression}{CloseParen}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitParenthesizedExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitParenthesizedExpression(this);
}
