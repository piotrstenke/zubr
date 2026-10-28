using Zubr.Text;

namespace Zubr.Syntax;

public sealed class EqualsValueClauseSyntax : SyntaxNode
{
	public Token Operator { get; }

	public ExpressionSyntax Value { get; }

	internal EqualsValueClauseSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token @operator,
		ExpressionSyntax value
	) : base(tree, span, SyntaxKind.EqualsValueClause)
	{
		Operator = @operator;
		Value = value;

		SetParent(value);
	}

	public override string ToString()
	{
		return $"{Operator} {Value}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitEqualsValueClause(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitEqualsValueClause(this);
}
