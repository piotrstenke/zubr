using Zubr.Text;

namespace Zubr.Syntax;

public sealed class BracketArgumentListSyntax : SyntaxNode
{
	public Token OpenBracket { get; }

	public SeparatedSyntaxList<ArgumentSyntax> Arguments { get; }

	public Token CloseBracket { get; }

	internal BracketArgumentListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openBracketToken,
		SeparatedSyntaxList<ArgumentSyntax> arguments,
		Token closeBracketToken
	) : base(tree, span, SyntaxKind.BracketArgumentList)
	{
		OpenBracket = openBracketToken;
		Arguments = arguments;
		CloseBracket = closeBracketToken;

		SetParent(arguments);
	}

	public override string ToString()
	{
		return $"{OpenBracket}{Arguments}{CloseBracket}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitBracketArgumentList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitBracketArgumentList(this);
}
