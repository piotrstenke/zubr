using Zubr.Text;

namespace Zubr.Syntax;

public sealed class MemberAccessExpressionSyntax : ExpressionSyntax
{
	public ExpressionSyntax Expression { get; }

	public Token Operator { get; }

	public SimpleNameSyntax Name { get; }

	internal MemberAccessExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxKind kind,
		ExpressionSyntax expression,
		Token @operator,
		SimpleNameSyntax name
	) : base(tree, span, kind)
	{
		Expression = expression;
		Operator = @operator;
		Name = name;

		SetParent(expression);
		SetParent(name);
	}

	public override string ToString()
	{
		return $"{Expression}{Operator}{Name}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitMemberAccessExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitMemberAccessExpression(this);
}
