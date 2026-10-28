using Zubr.Text;

namespace Zubr.Syntax;

public sealed class PrimaryBaseTypeSyntax : BaseTypeSyntax
{
	public override TypeSyntax Type { get; }

	public ArgumentListSyntax ArgumentList { get; }

	internal PrimaryBaseTypeSyntax(
		SyntaxTree tree,
		TextSpan span,
		TypeSyntax type,
		ArgumentListSyntax argumentList
	) : base(tree, span, SyntaxKind.PrimaryBaseType)
	{
		Type = type;
		ArgumentList = argumentList;

		SetParent(type);
		SetParent(argumentList);
	}

	public override string ToString()
	{
		return $"{Type}{ArgumentList}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitPrimaryBaseType(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitPrimaryBaseType(this);
}
