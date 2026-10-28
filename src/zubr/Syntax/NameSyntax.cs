using Zubr.Text;

namespace Zubr.Syntax;

public abstract class NameSyntax : TypeSyntax
{
	internal NameSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}

	internal abstract SimpleNameSyntax GetUnqualifiedName();
}
