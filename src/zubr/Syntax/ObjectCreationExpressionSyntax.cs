using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ObjectCreationExpressionSyntax : ExpressionSyntax
{
	public Token New { get; }

	public TypeSyntax? Type { get; }

	public ArgumentListSyntax? ArgumentList { get; }

	public InitializerExpressionSyntax? Initializer { get; }

	internal ObjectCreationExpressionSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token newKeyword,
		TypeSyntax? type,
		ArgumentListSyntax? argumentList,
		InitializerExpressionSyntax? initializer
	) : base(tree, span, SyntaxKind.ObjectCreationExpression)
	{
		New = newKeyword;
		Type = type;
		ArgumentList = argumentList;
		Initializer = initializer;

		SetParentIfNotNull(type);
		SetParentIfNotNull(argumentList);
		SetParentIfNotNull(initializer);
	}

	public override string ToString()
	{
		return $"{New}{LeadingSpace(Type)}{ArgumentList}{LeadingSpace(Initializer)}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitObjectCreationExpression(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitObjectCreationExpression(this);
}
