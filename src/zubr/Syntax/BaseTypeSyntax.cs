using Zubr.Text;

namespace Zubr.Syntax;

public abstract class BaseTypeSyntax : SyntaxNode
{
	public abstract TypeSyntax Type { get; }

	internal BaseTypeSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
