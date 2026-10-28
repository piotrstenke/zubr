using Zubr.Text;

namespace Zubr.Syntax;

public abstract class StatementSyntax : SyntaxNode
{
	internal StatementSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
