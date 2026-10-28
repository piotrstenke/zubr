using Zubr.Text;

namespace Zubr.Syntax;

public sealed class TypeParameterSyntax : SyntaxNode
{
	public SyntaxList<AttributeSyntax> Attributes { get; }

	public Token Identifier { get; }

	public TypeParameterInlineConstraintListSyntax? ConstraintList { get; }

	public EqualsTypeClauseSyntax? DefaultType { get; }

	internal TypeParameterSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		Token identifier,
		TypeParameterInlineConstraintListSyntax? constraintList,
		EqualsTypeClauseSyntax? defaultType
	) : base(tree, span, SyntaxKind.TypeParameter)
	{
		Attributes = attributes;
		Identifier = identifier;
		ConstraintList = constraintList;
		DefaultType = defaultType;

		SetParent(attributes);
		SetParentIfNotNull(constraintList);
		SetParentIfNotNull(defaultType);
	}

	public override string ToString()
	{
		return $"{Identifier}{LeadingSpace(ConstraintList)}{LeadingSpace(DefaultType)}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitTypeParameter(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitTypeParameter(this);
}
