using Zubr.Text;

namespace Zubr.Syntax;

public sealed class TypeConstraintSyntax : TypeParameterConstraintSyntax
{
	public TypeSyntax Type { get; }

	internal TypeConstraintSyntax(SyntaxTree tree, TextSpan span, TypeSyntax type) : base(tree, span, SyntaxKind.TypeConstraint)
	{
		Type = type;

		SetParent(type);
	}

	public override string ToString()
	{
		return $"{Type}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitTypeConstraint(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitTypeConstraint(this);
}
