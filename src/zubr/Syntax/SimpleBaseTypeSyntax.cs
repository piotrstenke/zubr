using Zubr.Text;

namespace Zubr.Syntax;

public sealed class SimpleBaseTypeSyntax : BaseTypeSyntax
{
	public override TypeSyntax Type { get; }

	internal SimpleBaseTypeSyntax(
		SyntaxTree tree,
		TextSpan span,
		TypeSyntax type
	) : base(tree, span, SyntaxKind.SimpleBaseType)
	{
		Type = type;

		SetParent(type);
	}

	public override string ToString()
	{
		return $"{Type}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitSimpleBaseType(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitSimpleBaseType(this);
}
