using Zubr.Text;

namespace Zubr.Syntax;

public sealed class CompilationUnitSyntax : SyntaxNode
{
	public SyntaxList<UseDirectiveSyntax> Uses { get; }

	public SyntaxList<AttributeSyntax> Attributes { get; }

	public SyntaxList<MemberDeclarationSyntax> Members { get; }

	public Token Eof { get; }

	internal CompilationUnitSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<UseDirectiveSyntax> uses,
		SyntaxList<AttributeSyntax> attributes,
		SyntaxList<MemberDeclarationSyntax> members,
		Token eof
	) : base(tree, span, SyntaxKind.CompilationUnit)
	{
		Uses = uses;
		Attributes = attributes;
		Members = members;
		Eof = eof;

		SetParent(uses);
		SetParent(attributes);
		SetParent(members);
	}

	public override string ToString()
	{
		return "root";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitCompilationUnit(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitCompilationUnit(this);
}
