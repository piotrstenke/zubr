using Zubr.Text;

namespace Zubr.Syntax;

public sealed class AccessorListSyntax : SyntaxNode
{
	public Token OpenBrace { get; }

	public SyntaxList<AccessorDeclarationSyntax> Accessors { get; }

	public Token CloseBrace { get; }

	internal AccessorListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openBrace,
		SyntaxList<AccessorDeclarationSyntax> accessors,
		Token closeBrace
	) : base(tree, span, SyntaxKind.AccessorList)
	{
		OpenBrace = openBrace;
		Accessors = accessors;
		CloseBrace = closeBrace;

		SetParent(accessors);
	}

	public override string ToString()
	{
		return $"{OpenBrace} {Accessors} {CloseBrace}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAccessorList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAccessorList(this);
}
