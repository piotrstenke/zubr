using Zubr.Text;

namespace Zubr.Syntax;

public sealed class PredefinedTypeSyntax : TypeSyntax
{
	public Token Keyword { get; }

	internal PredefinedTypeSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token keyword
	) : base(tree, span, SyntaxKind.PredefinedType)
	{
		Keyword = keyword;
	}

	public override string ToString()
	{
		return $"{Keyword}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitPredefinedType(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitPredefinedType(this);
}
