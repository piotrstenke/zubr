using Zubr.Text;

namespace Zubr.Syntax;

public sealed class InitializerExpressionSyntax : ExpressionSyntax
{
	public Token OpenBrace { get; }

	public SeparatedSyntaxList<ExpressionSyntax> Expressions { get; }

	public Token CloseBrace { get; }

	internal InitializerExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openBrace,
		SeparatedSyntaxList<ExpressionSyntax> expressions,
		Token closeBrace
	) : base(tree, span, SyntaxKind.InitializerExpression)
	{
		OpenBrace = openBrace;
		Expressions = expressions;
		CloseBrace = closeBrace;

		SetParent(expressions);
	}

	public override string ToString()
	{
		return $"{OpenBrace} ... {CloseBrace}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitInitializerExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitInitializerExpression(this);
}
