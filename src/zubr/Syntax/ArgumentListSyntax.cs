using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ArgumentListSyntax : SyntaxNode
{
	public Token OpenParen { get; }

	public SeparatedSyntaxList<ArgumentSyntax> Arguments { get; }

	public Token CloseParen { get; }

	internal ArgumentListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openParen,
		SeparatedSyntaxList<ArgumentSyntax> arguments,
		Token closeParen
	) : base(tree, span, SyntaxKind.ArgumentList)
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

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitArgumentList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitArgumentList(this);
}
