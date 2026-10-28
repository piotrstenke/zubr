using Zubr.Text;

namespace Zubr.Syntax;

public sealed class MatchExpressionBranchSyntax : SyntaxNode
{
	public ExpressionSyntax Left { get; }

	public Token Arrow { get; }

	public ExpressionSyntax Right { get; }

	internal MatchExpressionBranchSyntax(
		SyntaxTree tree,
		TextSpan span,
		ExpressionSyntax left,
		Token arrow,
		ExpressionSyntax right
	) : base(tree, span, SyntaxKind.MatchExpressionBranch)
	{
		Left = left;
		Arrow = arrow;
		Right = right;

		SetParent(left);
		SetParent(right);
	}

	public override string ToString()
	{
		return $"{Left} {Arrow} {Right}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitMatchExpressionBranch(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitMatchExpressionBranch(this);
}
