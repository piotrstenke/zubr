using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ElementAccessExpressionSyntax : ExpressionSyntax
{
	public ExpressionSyntax Expression { get; }

	public BracketArgumentListSyntax ArgumentList { get; }

	internal ElementAccessExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		ExpressionSyntax expression,
		BracketArgumentListSyntax argumentList
	) : base(tree, span, SyntaxKind.ElementAccessExpression)
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

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitElementAccessExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitElementAccessExpression(this);
}
