using Zubr.Text;

namespace Zubr.Syntax;

public sealed class IfStatementSyntax : StatementSyntax
{
	public Token If { get; }

	public Token OpenParen { get; }

	public ExpressionSyntax Condition { get; }

	public StatementSyntax Statement { get; }

	public Token CloseParen { get; }

	public SyntaxList<ElifClauseSyntax> Elifs { get; }

	public ElseClauseSyntax? Else { get; }

	internal IfStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token @if,
		Token openParen,
		ExpressionSyntax condition,
		Token closeParen,
		StatementSyntax statement,
		SyntaxList<ElifClauseSyntax> elifs,
		ElseClauseSyntax? @else
	) : base(tree, span, SyntaxKind.IfStatement)
	{
		If = @if;
		OpenParen = openParen;
		Condition = condition;
		Statement = statement;
		CloseParen = closeParen;
		Elifs = elifs;
		Else = @else;

		SetParent(condition);
		SetParent(statement);
		SetParent(elifs);
		SetParentIfNotNull(@else);
	}

	public override string ToString()
	{
		if (Else is null && Elifs.IsDefaultOrEmpty)
		{
			return $"{If} {OpenParen}{Condition}{CloseParen} {Statement}";
		}

		return $"{If} {OpenParen}{Condition}{CloseParen} {Statement} ...";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitIfStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitIfStatement(this);
}
