using Zubr.Text;

namespace Zubr.Syntax;

public sealed class MatchExpressionSyntax : ExpressionSyntax
{
	public Token Keyword { get; }

	public Token OpenParen { get; }

	public ExpressionSyntax Expression { get; }

	public Token CloseParen { get; }

	public Token OpenBrace { get; }

	public SeparatedSyntaxList<MatchExpressionBranchSyntax> Branches { get; }

	public Token CloseBrace { get; }

	internal MatchExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token keyword,
		Token openParen,
		ExpressionSyntax expression,
		Token closeParen,
		Token openBrace,
		SeparatedSyntaxList<MatchExpressionBranchSyntax> branches,
		Token closeBrace
	) : base(tree, span, SyntaxKind.MatchExpression)
	{
		Keyword = keyword;
		OpenParen = openParen;
		Expression = expression;
		CloseParen = closeParen;
		OpenBrace = openBrace;
		Branches = branches;
		CloseBrace = closeBrace;

		SetParent(expression);
		SetParent(branches);
	}

	public override string ToString()
	{
		return $"{Keyword} {OpenParen}{Expression}{CloseParen} {OpenBrace} ... {CloseBrace}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitMatchExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitMatchExpression(this);
}
