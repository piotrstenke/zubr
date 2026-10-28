using Zubr.Text;

namespace Zubr.Syntax;

public sealed class DoWhileStatementSyntax : StatementSyntax
{
	public Token Do { get; }

	public StatementSyntax Statement { get; }

	public Token While { get; }

	public Token OpenParen { get; }

	public ExpressionSyntax Condition { get; }

	public Token CloseParen { get; }

	public Token Semicolon { get; }

	internal DoWhileStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token @do,
		StatementSyntax statement,
		Token @while,
		Token openParen,
		ExpressionSyntax condition,
		Token closeParen,
		Token semicolon
	) : base(tree, span, SyntaxKind.DoWhileStatement)
	{
		Do = @do;
		Statement = statement;
		While = @while;
		OpenParen = openParen;
		Condition = condition;
		CloseParen = closeParen;
		Semicolon = semicolon;

		SetParent(statement);
		SetParent(condition);
	}

	public override string ToString()
	{
		return $"{Do} {Statement} {While} {OpenParen}{Condition}{CloseParen}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitDoWhileStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitDoWhileStatement(this);
}
