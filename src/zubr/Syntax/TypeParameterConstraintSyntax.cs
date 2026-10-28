using Zubr.Text;

namespace Zubr.Syntax;

public abstract class TypeParameterConstraintSyntax : SyntaxNode
{
	internal TypeParameterConstraintSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
