using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ArrayRankSyntax : SyntaxNode
{
	public Token OpenBracket { get; }

	public SeparatedSyntaxList<ExpressionSyntax> Sizes { get; }

	public Token CloseBracket { get; }

	internal ArrayRankSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openBracket,
		SeparatedSyntaxList<ExpressionSyntax> sizes,
		Token closeBracket
	) : base(tree, span, SyntaxKind.ArrayRank)
	{
		OpenBracket = openBracket;
		Sizes = sizes;
		CloseBracket = closeBracket;

		SetParent(sizes);
	}

	public override string ToString()
	{
		return $"{OpenBracket}{Sizes}{CloseBracket}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitArrayRank(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitArrayRank(this);
}
