using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ReferenceTypeSyntax : TypeSyntax
{
	public TypeSyntax ElementType { get; }

	public Token Ampersand { get; }

	internal ReferenceTypeSyntax(
		SyntaxTree tree,
		TextSpan span,
		TypeSyntax elementType,
		Token ampersand
	) : base(tree, span, SyntaxKind.ReferenceType)
	{
		ElementType = elementType;
		Ampersand = ampersand;

		SetParent(elementType);
	}

	public override string ToString()
	{
		return $"{ElementType}{Ampersand}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitReferenceType(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitReferenceType(this);
}
