using Zubr.Text;

namespace Zubr.Syntax;

public abstract class TypeSyntax : ExpressionSyntax
{
	internal TypeSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
