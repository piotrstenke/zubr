using Zubr.Text;

namespace Zubr.Syntax;

public sealed class AttributeArgumentSyntax : SyntaxNode
{
	public ExpressionSyntax Expression { get; }

	internal AttributeArgumentSyntax(
		SyntaxTree tree,
		TextSpan span,
		ExpressionSyntax expression
	) : base(tree, span, SyntaxKind.AttributeArgument)
	{
		Expression = expression;

		SetParent(expression);
	}

	public override string ToString()
	{
		return $"{Expression}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAttributeArgument(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAttributeArgument(this);
}
