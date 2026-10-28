using Zubr.Text;

namespace Zubr.Syntax;

public sealed class TypeParameterInlineConstraintListSyntax : SyntaxNode
{
	public Token Colon { get; }

	public SeparatedSyntaxList<TypeParameterConstraintSyntax> Constraints { get; }

	internal TypeParameterInlineConstraintListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token colon,
		SeparatedSyntaxList<TypeParameterConstraintSyntax> constraints
	) : base(tree, span, SyntaxKind.TypeParameterInlineConstraintList)
	{
		Colon = colon;
		Constraints = constraints;

		SetParent(constraints);
	}

	public override string ToString()
	{
		return $"{Colon} {Constraints}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitTypeParameterInlineConstraintList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitTypeParameterInlineConstraintList(this);
}
