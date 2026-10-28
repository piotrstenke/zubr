using Zubr.Text;

namespace Zubr.Syntax;

public sealed class EmptyStatementSyntax : StatementSyntax
{
	public Token Semicolon { get; }

	internal EmptyStatementSyntax(SyntaxTree tree, TextSpan span, Token semicolon) : base(tree, span, SyntaxKind.EmptyStatement)
	{
		Semicolon = semicolon;
	}

	public override string ToString()
	{
		return $"{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitEmptyStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitEmptyStatement(this);
}
