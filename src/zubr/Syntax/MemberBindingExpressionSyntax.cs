using Zubr.Text;

namespace Zubr.Syntax;

public sealed class MemberBindingExpressionSyntax : ExpressionSyntax
{
	public Token Operator { get; }

	public SimpleNameSyntax Name { get; }

	internal MemberBindingExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token @operator,
		SimpleNameSyntax name
	) : base(tree, span, SyntaxKind.MemberBindingExpression)
	{
		Operator = @operator;
		Name = name;

		SetParent(name);
	}

	public override string ToString()
	{
		return $"{Operator}{Name}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitMemberBindingExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitMemberBindingExpression(this);
}
