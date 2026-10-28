using Zubr.Text;

namespace Zubr.Syntax;

public sealed class TypeSpecifierSyntax : SyntaxNode
{
	public Token Colon { get; }

	public TypeSyntax Type { get; }

	internal TypeSpecifierSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token colon,
		TypeSyntax type
	) : base(tree, span, SyntaxKind.TypeSpecifier)
	{
		Colon = colon;
		Type = type;

		SetParent(type);
	}

	public override string ToString()
	{
		return $"{Colon} {Type}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitTypeSpecifier(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitTypeSpecifier(this);
}
