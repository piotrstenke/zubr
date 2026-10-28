using Zubr.Text;

namespace Zubr.Syntax;

public sealed class BaseTypeListSyntax : SyntaxNode
{
	public Token Colon { get; }

	public SeparatedSyntaxList<BaseTypeSyntax> Types { get; }

	internal BaseTypeListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token colon,
		SeparatedSyntaxList<BaseTypeSyntax> types
	) : base(tree, span, SyntaxKind.BaseTypeList)
	{
		Colon = colon;
		Types = types;

		SetParent(types);
	}

	public override string ToString()
	{
		return $"{Colon} {Types}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitBaseTypeList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitBaseTypeList(this);
}
