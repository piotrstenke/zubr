using Zubr.Text;

namespace Zubr.Syntax;

public sealed class QualifiedNameSyntax : NameSyntax
{
	public NameSyntax Left { get; }

	public Token Dot { get; }

	public SimpleNameSyntax Right { get; }

	internal QualifiedNameSyntax(
		SyntaxTree tree,
		TextSpan span,
		NameSyntax left,
		Token dot,
		SimpleNameSyntax right
	) : base(tree, span, SyntaxKind.QualifiedName)
	{
		Left = left;
		Dot = dot;
		Right = right;

		SetParent(left);
		SetParent(right);
	}

	public override string ToString()
	{
		return $"{Left}{Dot}{Right}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitQualifiedName(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitQualifiedName(this);

	internal override SimpleNameSyntax GetUnqualifiedName()
	{
		return Right;
	}
}
