using Zubr.Text;

namespace Zubr.Syntax;

public abstract class CommonPropertyDeclarationSyntax : MemberDeclarationSyntax
{
	public abstract TypeSpecifierSyntax? TypeSpecifier { get; }

	public abstract ArrowExpressionClauseSyntax? ExpressionBody { get; }

	public abstract AccessorListSyntax? AccessorList { get; }

	public abstract Token Semicolon { get; }

	internal CommonPropertyDeclarationSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
