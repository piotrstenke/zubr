using Zubr.Text;

namespace Zubr.Syntax;

public sealed class EqualsTypeClauseSyntax : SyntaxNode
{
	public Token Operator { get; }

	public TypeSyntax Type { get; }

	internal EqualsTypeClauseSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token @operator,
		TypeSyntax type
	) : base(tree, span, SyntaxKind.EqualsTypeClause)
	{
		Operator = @operator;
		Type = type;

		SetParent(type);
	}

	public override string ToString()
	{
		return $"{Operator} {Type}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitEqualsTypeClause(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitEqualsTypeClause(this);
}
