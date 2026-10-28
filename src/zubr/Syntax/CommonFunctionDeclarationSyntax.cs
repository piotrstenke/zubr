using Zubr.Text;

namespace Zubr.Syntax;

public abstract class CommonFunctionDeclarationSyntax : MemberDeclarationSyntax
{
	public abstract Token Keyword { get; }

	public abstract ParameterListSyntax ParameterList { get; }

	public abstract BlockSyntax? Body { get; }

	public abstract ArrowExpressionClauseSyntax? ExpressionBody { get; }

	public abstract Token Semicolon { get; }

	internal CommonFunctionDeclarationSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
