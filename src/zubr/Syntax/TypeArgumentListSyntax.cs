using Zubr.Text;

namespace Zubr.Syntax;

public sealed class TypeArgumentListSyntax : SyntaxNode
{
	public Token LessThan { get; }

	public SeparatedSyntaxList<TypeSyntax> Arguments { get; }

	public Token GreaterThan { get; }

	public TypeArgumentListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token lessThan,
		SeparatedSyntaxList<TypeSyntax> arguments,
		Token greaterThan
	) : base(tree, span, SyntaxKind.TypeArgumentList)
	{
		Arguments = arguments;
		LessThan = lessThan;
		GreaterThan = greaterThan;

		SetParent(arguments);
	}

	public override string ToString()
	{
		return $"{LessThan}{Arguments}{GreaterThan}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitTypeArgumentList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitTypeArgumentList(this);
}
