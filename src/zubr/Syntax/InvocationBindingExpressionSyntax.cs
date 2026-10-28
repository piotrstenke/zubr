using Zubr.Text;

namespace Zubr.Syntax;

public sealed class InvocationBindingExpressionSyntax : ExpressionSyntax
{
	public ArgumentListSyntax ArgumentList { get; }

	internal InvocationBindingExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		ArgumentListSyntax argumentList
	) : base(tree, span, SyntaxKind.InvocationBindingExpression)
	{
		ArgumentList = argumentList;

		SetParent(argumentList);
	}

	public override string ToString()
	{
		return $"{ArgumentList}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitInvocationBindingExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitInvocationBindingExpression(this);
}
