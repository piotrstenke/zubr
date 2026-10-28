using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ConstructorInvocationExpressionSyntax : ExpressionSyntax
{
	public Token Keyword { get; }

	public ArgumentListSyntax ArgumentList { get; }

	internal ConstructorInvocationExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxKind kind,
		Token keyword,
		ArgumentListSyntax argumentList
	) : base(tree, span, kind)
	{
		Keyword = keyword;
		ArgumentList = argumentList;

		SetParent(argumentList);
	}

	public override string ToString()
	{
		return $"{Keyword}{ArgumentList}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitConstructorInvocationExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitConstructorInvocationExpression(this);
}
