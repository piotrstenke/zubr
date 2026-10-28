using Zubr.Text;

namespace Zubr.Syntax;

public abstract class TypeDeclarationSyntax : CommonTypeDeclarationSyntax
{
	public abstract TypeParameterListSyntax? TypeParameterList { get; }

	public abstract ParameterListSyntax? ParameterList { get; }

	public abstract SyntaxList<TypeParameterConstraintListSyntax> ConstraintList { get; }

	public abstract SyntaxList<MemberDeclarationSyntax> Members { get; }

	public int Arity => TypeParameterList?.Parameters.Count ?? 0;

	internal TypeDeclarationSyntax(SyntaxTree tree, TextSpan span, SyntaxKind kind) : base(tree, span, kind)
	{
	}
}
