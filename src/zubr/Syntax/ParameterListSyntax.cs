using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ParameterListSyntax : SyntaxNode
{
	public Token OpenParen { get; }

	public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }

	public Token CloseParen { get; }

	internal ParameterListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token openParen,
		SeparatedSyntaxList<ParameterSyntax> parameters,
		Token closeParen
	) : base(tree, span, SyntaxKind.ParameterList)
	{
		OpenParen = openParen;
		Parameters = parameters;
		CloseParen = closeParen;

		SetParent(parameters);
	}

	public override string ToString()
	{
		return $"{OpenParen}{Parameters}{CloseParen}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitParameterList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitParameterList(this);
}
