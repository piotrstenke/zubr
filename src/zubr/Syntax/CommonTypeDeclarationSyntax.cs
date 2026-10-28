using Zubr.Text;

namespace Zubr.Syntax;

public abstract class CommonTypeDeclarationSyntax : MemberDeclarationSyntax
{
	public abstract Token Keyword { get; }

	public abstract Token Identifier { get; }

	public abstract BaseTypeListSyntax? BaseTypeList { get; }

	public abstract Token OpenBrace { get; }

	public abstract Token CloseBrace { get; }

	public abstract Token Semicolon { get; }

	internal CommonTypeDeclarationSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
