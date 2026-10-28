using Zubr.Text;

namespace Zubr.Syntax;

public sealed class IdentifierNameSyntax : SimpleNameSyntax
{
	public override Token Identifier { get; }

	public IdentifierNameSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token identifier
	) : base(tree, span, SyntaxKind.IdentifierName)
	{
		Identifier = identifier;
	}

	public override string ToString()
	{
		return $"{Identifier}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitIdentifierName(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitIdentifierName(this);
}
