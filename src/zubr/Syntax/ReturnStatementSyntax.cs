using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ReturnStatementSyntax : StatementSyntax
{
	public Token Return { get; }

	public ExpressionSyntax? Expression { get; }

	public Token Semicolon { get; }

	internal ReturnStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token @return,
		ExpressionSyntax? expression,
		Token semicolon
	) : base(tree, span, SyntaxKind.ReturnStatement)
	{
		Return = @return;
		Expression = expression;
		Semicolon = semicolon;

		SetParentIfNotNull(expression);
	}

	public override string ToString()
	{
		return $"{Return}{TrailingSpace(Expression)}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitReturnStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitReturnStatement(this);
}
