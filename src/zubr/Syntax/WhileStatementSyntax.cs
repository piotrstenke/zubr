using Zubr.Text;

namespace Zubr.Syntax;

public sealed class WhileStatementSyntax : StatementSyntax
{
	public Token While { get; }

	public Token OpenParen { get; }

	public ExpressionSyntax Condition { get; }

	public Token CloseParen { get; }

	public StatementSyntax Statement { get; }

	internal WhileStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token @while,
		Token openParen,
		ExpressionSyntax condition,
		Token closeParen,
		StatementSyntax statement
	) : base(tree, span, SyntaxKind.WhileStatement)
	{
		While = @while;
		OpenParen = openParen;
		Condition = condition;
		CloseParen = closeParen;
		Statement = statement;

		SetParent(condition);
		SetParent(statement);
	}

	public override string ToString()
	{
		return $"{While} {OpenParen}{Condition}{CloseParen} {Statement}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitWhileStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitWhileStatement(this);
}
