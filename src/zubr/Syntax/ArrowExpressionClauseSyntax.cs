using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ArrowExpressionClauseSyntax : SyntaxNode
{
	public Token Arrow { get; }

	public ExpressionSyntax Expression { get; }

	internal ArrowExpressionClauseSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token arrow,
		ExpressionSyntax expression
	) : base(tree, span, SyntaxKind.ArrowExpressionClause)
	{
		Arrow = arrow;
		Expression = expression;

		SetParent(expression);
	}

	public override string ToString()
	{
		return $"{Arrow} {Expression}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitArrowExpressionClause(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitArrowExpressionClause(this);
}
