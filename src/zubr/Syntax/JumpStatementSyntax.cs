using Zubr.Text;

namespace Zubr.Syntax;

public sealed class JumpStatementSyntax : StatementSyntax
{
	public Token Keyword { get; }

	public Token Identifier { get; }

	public Token Semicolon { get; }

	internal JumpStatementSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token keyword,
		Token identifier,
		Token semicolon
	) : base(tree, span, SyntaxKind.JumpStatement)
	{
		Keyword = keyword;
		Identifier = identifier;
		Semicolon = semicolon;
	}

	public override string ToString()
	{
		return $"{Keyword} {Identifier}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitJumpStatement(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitJumpStatement(this);
}
