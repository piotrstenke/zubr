using Zubr.Text;

namespace Zubr.Syntax;

public sealed class UseDirectiveElementListSyntax : SyntaxNode
{
	public Token OpenBrace { get; }

	public SeparatedSyntaxList<UseDirectiveElementSyntax> Elements { get; }

	public Token CloseBrace { get; }

	internal UseDirectiveElementListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openBrace,
		SeparatedSyntaxList<UseDirectiveElementSyntax> elements,
		Token closeBrace
	) : base(tree, span, SyntaxKind.UseDirectiveElementList)
	{
		OpenBrace = openBrace;
		Elements = elements;
		CloseBrace = closeBrace;

		SetParent(Elements);
	}

	public override string ToString()
	{
		return $"{OpenBrace} {Elements} {CloseBrace}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitUseDirectiveElementList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitUseDirectiveElementList(this);
}
