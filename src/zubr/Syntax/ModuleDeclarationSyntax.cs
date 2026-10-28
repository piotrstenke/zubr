using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ModuleDeclarationSyntax : MemberDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes => default;

	public override TokenList Modifiers => TokenList.Empty;

	public Token Module { get; }

	public Token Top { get; }

	public NameSyntax? Name { get; }

	public Token Semicolon { get; }

	public SyntaxList<MemberDeclarationSyntax> Members { get; }

	internal ModuleDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token module, 
		Token top,
		NameSyntax? name,
		Token semicolon,
		SyntaxList<MemberDeclarationSyntax> members
	) : base(tree, span, SyntaxKind.ModuleDeclaration)
	{
		Module = module;
		Top = top;
		Name = name;
		Semicolon = semicolon;
		Members = members;

		SetParentIfNotNull(name);
		SetParent(members);
	}

	public override string ToString()
	{
		if (Name is null)
		{
			return $"{Module} {Top}{Semicolon}";
		}

		return $"{Module} {Name}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitModuleDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitModuleDeclaration(this);
}
