using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ArrayCreationExpressionSyntax : ExpressionSyntax
{
	public Token New { get; }

	public TypeSyntax? ElementType { get; }

	public SyntaxList<ArrayRankSyntax> Ranks { get; }

	public InitializerExpressionSyntax? Initializer { get; }

	internal ArrayCreationExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token @new,
		TypeSyntax? elementType,
		SyntaxList<ArrayRankSyntax> ranks,
		InitializerExpressionSyntax? initializer
	) : base(tree, span, SyntaxKind.ArrayCreationExpression)
	{
		New = @new;
		ElementType = elementType;
		Ranks = ranks;
		Initializer = initializer;

		SetParentIfNotNull(elementType);
		SetParent(ranks);
		SetParentIfNotNull(initializer);
	}

	public override string ToString()
	{
		return $"{New}{LeadingSpace(ElementType)}{Ranks}{LeadingSpace(Initializer)}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitArrayCreationExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitArrayCreationExpression(this);
}
