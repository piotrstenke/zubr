using Zubr.Text;

namespace Zubr.Syntax;

public abstract class UseDirectiveSyntax : SyntaxNode
{
	public abstract Token Top { get; }

	public abstract Token Use { get; }

	public abstract AliasSpecifierSyntax? Alias { get; }

	public abstract Token Semicolon { get; }

	internal UseDirectiveSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
