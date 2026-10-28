using Zubr.Text;

namespace Zubr.Syntax;

public abstract class SimpleNameSyntax : NameSyntax
{
	public abstract Token Identifier { get; }

	internal SimpleNameSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}

	internal sealed override SimpleNameSyntax GetUnqualifiedName()
	{
		return this;
	}
}
