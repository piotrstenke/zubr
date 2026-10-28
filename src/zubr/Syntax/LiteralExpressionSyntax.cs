using Zubr.Text;

namespace Zubr.Syntax;

public sealed class LiteralExpressionSyntax : ExpressionSyntax
{
	public Token Value { get; }

	internal LiteralExpressionSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind, Token value) : base(tree, span, kind)
	{
		Value = value;
	}

	public override string ToString()
	{
		return Value.ToString();
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitLiteralExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitLiteralExpression(this);
}
