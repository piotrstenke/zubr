using Zubr.Text;

namespace Zubr.Syntax;

public sealed class BlockSyntax : StatementSyntax
{
	public Token OpenBrace { get; }

	public SyntaxList<StatementSyntax> Statements { get; }

	public Token CloseBrace { get; }

	internal BlockSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openBrace,
		SyntaxList<StatementSyntax> statements,
		Token closeBrace
	) : base(tree, span, SyntaxKind.Block)
	{
		OpenBrace = openBrace;
		Statements = statements;
		CloseBrace = closeBrace;

		SetParent(statements);
	}

	public override string ToString()
	{
		return $"{OpenBrace} ... {CloseBrace}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitBlock(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitBlock(this);
}
