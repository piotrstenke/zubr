using Zubr.Text;

namespace Zubr.Syntax;

public sealed class TypeParameterConstraintListSyntax :  SyntaxNode
{
	public Token Where { get; }

	public NameSyntax Name { get; }

	public Token Colon { get; }

	public SeparatedSyntaxList<TypeParameterConstraintSyntax> Constraints { get; }

	public TypeParameterConstraintListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token where,
		NameSyntax name,
		Token colon,
		SeparatedSyntaxList<TypeParameterConstraintSyntax> constraints
	) : base(tree, span, SyntaxKind.TypeParameterConstraintList)
	{
		Where = where;
		Name = name;
		Colon = colon;
		Constraints = constraints;

		SetParent(constraints);
		SetParent(name);
	}

	public override string ToString()
	{
		return $"{Where} {Name} {Colon} {Constraints}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitTypeParameterConstraintList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitTypeParameterConstraintList(this);
}
