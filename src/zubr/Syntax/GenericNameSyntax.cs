using Zubr.Text;

namespace Zubr.Syntax;

public sealed class GenericNameSyntax : SimpleNameSyntax
{
	public override Token Identifier { get; }

	public TypeArgumentListSyntax TypeArgumentList { get; }

	internal GenericNameSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token identifier,
		TypeArgumentListSyntax typeArgumentList
	) : base(tree, span, SyntaxKind.GenericName)
	{
		Identifier = identifier;
		TypeArgumentList = typeArgumentList;

		SetParent(typeArgumentList);
	}

	public override string ToString()
	{
		return $"{Identifier}{TypeArgumentList}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitGenericName(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitGenericName(this);
}
