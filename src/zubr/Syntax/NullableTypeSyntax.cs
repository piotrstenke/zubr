using Zubr.Text;

namespace Zubr.Syntax;

public sealed class NullableTypeSyntax : TypeSyntax
{
	public TypeSyntax Type { get; }

	public Token Question { get; }

	internal NullableTypeSyntax(
		SyntaxTree tree,
		TextSpan span,
		TypeSyntax type,
		Token question
	) : base(tree, span, SyntaxKind.NullableType)
	{
		Type = type;
		Question = question;

		SetParent(Type);
	}

	public override string ToString()
	{
		return $"{Type}{Question}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitNullableType(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitNullableType(this);
}
