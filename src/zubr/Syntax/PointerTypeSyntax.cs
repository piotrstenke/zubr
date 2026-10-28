using Zubr.Text;

namespace Zubr.Syntax;

public sealed class PointerTypeSyntax : TypeSyntax
{
	public TypeSyntax Type { get; }

	public Token Asterisk { get; }

	internal PointerTypeSyntax(
		SyntaxTree tree,
		TextSpan span,
		TypeSyntax type,
		Token asterisk
	) : base(tree, span, SyntaxKind.PointerType)
	{
		Type = type;
		Asterisk = asterisk;

		SetParent(Type);
	}

	public override string ToString()
	{
		return $"{Type}{Asterisk}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitPointerType(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitPointerType(this);
}
