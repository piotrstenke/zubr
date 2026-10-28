using Zubr.Text;

namespace Zubr.Syntax;

public sealed class InvocationExpressionSyntax : ExpressionSyntax
{
	public ExpressionSyntax Expression { get; }

	public ArgumentListSyntax ArgumentList { get; }

	internal InvocationExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		ExpressionSyntax expression,
		ArgumentListSyntax argumentList
	) : base(tree, span, SyntaxKind.InvocationExpression)
	{
		Expression = expression;
		ArgumentList = argumentList;

		SetParent(expression);
		SetParent(argumentList);
	}

	public override string ToString()
	{
		return $"{Expression}{ArgumentList}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitInvocationExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitInvocationExpression(this);
}
