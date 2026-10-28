using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ElementBindingExpressionSyntax : ExpressionSyntax
{
	public BracketArgumentListSyntax ArgumentList { get; }

	internal ElementBindingExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		BracketArgumentListSyntax argumentList
	) : base(tree, span, SyntaxKind.ElementBindingExpression)
	{
		ArgumentList = argumentList;

		SetParent(argumentList);
	}

	public override string ToString()
	{
		return $"{ArgumentList}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitElementBindingExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitElementBindingExpression(this);
}
