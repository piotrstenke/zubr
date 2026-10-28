using Zubr.Text;

namespace Zubr.Syntax;

public sealed class BracketParameterListSyntax : SyntaxNode
{
	public Token OpenBracket { get; }

	public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }

	public Token CloseBracket { get; }

	internal BracketParameterListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openBracket,
		SeparatedSyntaxList<ParameterSyntax> parameters,
		Token closeBracket
	) : base(tree, span, SyntaxKind.BracketParameterList)
	{
		OpenBracket = openBracket;
		Parameters = parameters;
		CloseBracket = closeBracket;

		SetParent(parameters);
	}

	public override string ToString()
	{
		return $"{OpenBracket}{Parameters}{CloseBracket}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitBracketParameterList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitBracketParameterList(this);
}
