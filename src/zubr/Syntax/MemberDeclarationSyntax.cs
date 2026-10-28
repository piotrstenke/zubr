using Zubr.Text;

namespace Zubr.Syntax;

public abstract class MemberDeclarationSyntax : StatementSyntax
{
	public abstract SyntaxList<AttributeSyntax> Attributes { get; }

	public abstract TokenList Modifiers { get; }

	internal MemberDeclarationSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
