using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ExpressionStatementSyntax : StatementSyntax
{
	public ExpressionSyntax Expression { get; }

	public Token Semicolon { get; }

	internal ExpressionStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		ExpressionSyntax expression,
		Token semicolon
	) : base(tree, span, SyntaxKind.ExpressionStatement)
	{
		Expression = expression;
		Semicolon = semicolon;

		SetParent(expression);
	}

	public override string ToString()
	{
		return $"{Expression}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitExpressionStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitExpressionStatement(this);
}
