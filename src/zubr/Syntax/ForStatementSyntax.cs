using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ForStatementSyntax : StatementSyntax
{
	public Token Keyword { get; }

	public Token OpenParen { get; }

	public Token Identifier { get; }

	public Token Colon { get; }

	public ExpressionSyntax Expression { get; }

	public Token CloseParen { get; }

	public StatementSyntax Statement { get; }

	internal ForStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token keyword,
		Token openParen,
		Token identifier,
		Token colon,
		ExpressionSyntax expression,
		Token closeParen,
		StatementSyntax statement
	) : base(tree, span, SyntaxKind.ForStatement)
	{
		Keyword = keyword;
		OpenParen = openParen;
		Identifier = identifier;
		Colon = colon;
		Expression = expression;
		CloseParen = closeParen;
		Statement = statement;

		SetParent(expression);
		SetParent(statement);
	}

	public override string ToString()
	{
		return $"{Keyword}{OpenParen}{Identifier} {Colon} {Expression}{CloseParen} {Statement}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitForStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitForStatement(this);
}
