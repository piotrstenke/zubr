using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ElifClauseSyntax : SyntaxNode
{
	public Token Keyword { get; }

	public Token OpenParen { get; }

	public ExpressionSyntax Condition { get; }

	public Token CloseParen { get; }

	public StatementSyntax Statement { get; }

	internal ElifClauseSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token keyword,
		Token openParen,
		ExpressionSyntax condition,
		Token closeParen,
		StatementSyntax statement
	) : base(tree, span, SyntaxKind.ElifClause)
	{
		Keyword = keyword;
		OpenParen = openParen;
		Condition = condition;
		Statement = statement;
		CloseParen = closeParen;

		SetParent(condition);
		SetParent(statement);
	}

	public override string ToString()
	{
		return $"{Keyword} {OpenParen}{Condition}{CloseParen} {Statement}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitElifClause(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitElifClause(this);
}
