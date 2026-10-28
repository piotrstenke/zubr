using Zubr.Text;

namespace Zubr.Syntax;

public sealed class StopStatementSyntax : StatementSyntax
{
	public Token Stop { get; }

	public Token Semicolon { get; }

	internal StopStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token stop,
		Token semicolon
	) : base(tree, span, SyntaxKind.StopStatement)
	{
		Stop = stop;
		Semicolon = semicolon;
	}

	public override string ToString()
	{
		return $"{Stop}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitStopStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitStopStatement(this);
}
