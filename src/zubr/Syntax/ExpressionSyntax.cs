using Zubr.Text;

namespace Zubr.Syntax;

public abstract class ExpressionSyntax : SyntaxNode
{
	internal ExpressionSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
