using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ElseClauseSyntax : SyntaxNode
{
	public Token Keyword { get; }

	public StatementSyntax Statement { get; }

	internal ElseClauseSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token keyword,
		StatementSyntax statement
	) : base(tree, span, SyntaxKind.ElseClause)
	{
		Keyword = keyword;
		Statement = statement;

		SetParent(statement);
	}

	public override string ToString()
	{
		return $"{Keyword} {Statement}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitElseClause(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitElseClause(this);
}
