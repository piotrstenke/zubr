using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ArgumentSyntax : SyntaxNode
{
	public ExpressionSyntax Expression { get; }

	internal ArgumentSyntax(
		SyntaxTree tree,
		TextSpan span,
		ExpressionSyntax expression
	) : base(tree, span, SyntaxKind.Argument)
	{
		Expression = expression;

		SetParent(expression);
	}

	public override string ToString()
	{
		return $"{Expression}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitArgument(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitArgument(this);
}
