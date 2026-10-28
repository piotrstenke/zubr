using Zubr.Text;

namespace Zubr.Syntax;

public sealed class TypeParameterListSyntax : SyntaxNode
{
	public Token LessThan { get; }

	public SeparatedSyntaxList<TypeParameterSyntax> Parameters { get; }

	public Token GreaterThan { get; }

	internal TypeParameterListSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token lessThan,
		SeparatedSyntaxList<TypeParameterSyntax> parameters,
		Token greaterThan
	) : base(tree, span, SyntaxKind.TypeParameterList)
	{
		LessThan = lessThan;
		Parameters = parameters;
		GreaterThan = greaterThan;

		SetParent(parameters);
	}

	public override string ToString()
	{
		return $"{LessThan}{Parameters}{GreaterThan}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitTypeParameterList(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitTypeParameterList(this);
}
