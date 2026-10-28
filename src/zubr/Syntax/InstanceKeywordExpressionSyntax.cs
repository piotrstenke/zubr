using Zubr.Text;

namespace Zubr.Syntax;

public sealed class InstanceKeywordExpressionSyntax : ExpressionSyntax
{
	public Token Keyword { get; }

	internal InstanceKeywordExpressionSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind, Token keyword) : base(tree, span, kind)
	{
		Keyword = keyword;
	}

	public override string ToString()
	{
		return $"{Keyword}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitInstanceKeywordExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitInstanceKeywordExpression(this);
}
