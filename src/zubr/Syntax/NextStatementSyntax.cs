using Zubr.Text;

namespace Zubr.Syntax;

public sealed class NextStatementSyntax : StatementSyntax
{
	public Token Next { get; }

	public Token Semicolon { get; }

	internal NextStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token next,
		Token semicolon
	) : base(tree, span, SyntaxKind.NextStatement)
	{
		Next = next;
		Semicolon = semicolon;
	}

	public override string ToString()
	{
		return $"{Next}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitNextStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitNextStatement(this);
}
