using Zubr.Text;

namespace Zubr.Syntax;

public sealed class KeywordConstraintSyntax : TypeParameterConstraintSyntax
{
	public Token Keyword { get; }

	public Token Question { get; }

	internal KeywordConstraintSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxKind kind,
		Token keyword,
		Token question
	) : base(tree, span, kind)
	{
		Keyword = keyword;
		Question = question;
	}

	public override string ToString()
	{
		return $"{Keyword}{Question}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitKeywordConstraint(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitKeywordConstraint(this);
}
