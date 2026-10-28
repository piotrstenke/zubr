using Zubr.Text;

namespace Zubr.Syntax;

public sealed class LabelStatementSyntax : StatementSyntax
{
	public Token Identifier { get; }

	public Token Colon { get; }

	public StatementSyntax Statement { get; }

	internal LabelStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token identifier,
		Token colon,
		StatementSyntax statement
	) : base(tree, span, SyntaxKind.LabelStatement)
	{
		Identifier = identifier;
		Colon = colon;
		Statement = statement;

		SetParent(statement);
	}

	public override string ToString()
	{
		return $"{Identifier}{Colon} {Statement}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitLabelStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitLabelStatement(this);
}
