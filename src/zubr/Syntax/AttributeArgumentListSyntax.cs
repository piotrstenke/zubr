using Zubr.Text;

namespace Zubr.Syntax;

public sealed class AttributeArgumentListSyntax : SyntaxNode
{
	public Token OpenParen { get; }

	public SeparatedSyntaxList<AttributeArgumentSyntax> Arguments { get; }

	public Token CloseParen { get; }

	internal AttributeArgumentListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openParen,
		SeparatedSyntaxList<AttributeArgumentSyntax> arguments,
		Token closeParen
	) : base(tree, span, SyntaxKind.AttributeArgumentList)
	{
		OpenParen = openParen;
		Arguments = arguments;
		CloseParen = closeParen;

		SetParent(arguments);
	}

	public override string ToString()
	{
		return $"{OpenParen}{Arguments}{CloseParen}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAttributeArgumentList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAttributeArgumentList(this);
}
