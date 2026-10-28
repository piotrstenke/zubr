using Zubr.Text;

namespace Zubr.Syntax;

public sealed class SkippedArraySizeExpressionSyntax : ExpressionSyntax
{
	public Token Token { get; }

	internal SkippedArraySizeExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token token
	) : base(tree, span, SyntaxKind.SkippedArraySizeExpression)
	{
		Token = token;
	}

	public override string ToString()
	{
		return string.Empty;
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitSkippedArraySizeExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitSkippedArraySizeExpression(this);
}
