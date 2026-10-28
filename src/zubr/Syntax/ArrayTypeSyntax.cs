using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ArrayTypeSyntax : TypeSyntax
{
	public TypeSyntax ElementType { get; }

	public SyntaxList<ArrayRankSyntax> Ranks { get; }

	internal ArrayTypeSyntax(
		SyntaxTree tree,
		TextSpan span,
		TypeSyntax elementType,
		SyntaxList<ArrayRankSyntax> ranks
	) : base(tree, span, SyntaxKind.ArrayType)
	{
		ElementType = elementType;
		Ranks = ranks;

		SetParent(elementType);
		SetParent(ranks);
	}

	public override string ToString()
	{
		return $"{ElementType}{Ranks}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitArrayType(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitArrayType(this);
}
