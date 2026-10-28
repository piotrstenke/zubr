using Zubr.Text;

namespace Zubr.Syntax;

public sealed class SkippedTypeArgumentSyntax : TypeSyntax
{
	public Token Token { get; }

	internal SkippedTypeArgumentSyntax(SyntaxTree tree, TextSpan span, Token token) : base(tree, span, SyntaxKind.SkippedTypeArgument)
	{
		Token = token;
	}

	public override string ToString()
	{
		return string.Empty;
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitSkippedTypeArgument(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitSkippedTypeArgument(this);
}
