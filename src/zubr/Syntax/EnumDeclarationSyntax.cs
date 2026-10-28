using Zubr.Text;

namespace Zubr.Syntax;

public sealed class EnumDeclarationSyntax : CommonTypeDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public override Token Keyword { get; }

	public override Token Identifier { get; }

	public override BaseTypeListSyntax? BaseTypeList { get; }

	public override Token Semicolon { get; }

	public override Token OpenBrace { get; }

	public SeparatedSyntaxList<EnumMemberDeclarationSyntax> Members { get; }

	public override Token CloseBrace { get; }

	internal EnumDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token keyword,
		Token identifier,
		BaseTypeListSyntax? baseTypeList,
		Token semicolon,
		Token openBrace,
		SeparatedSyntaxList<EnumMemberDeclarationSyntax> members,
		Token closeBrace
	) : base(tree, span, SyntaxKind.EnumDeclaration)
	{
		Modifiers = modifiers;
		Keyword = keyword;
		Identifier = identifier;
		BaseTypeList = baseTypeList;
		Semicolon = semicolon;
		OpenBrace = openBrace;
		Members = members;
		CloseBrace = closeBrace;

		SetParent(attributes);
		SetParent(members);
		SetParentIfNotNull(baseTypeList);
	}

	public override string ToString()
	{
		if (Semicolon.Exists)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword} {Identifier}{LeadingSpace(BaseTypeList)}{Semicolon}";
		}

		return $"{TrailingSpace(Modifiers)}{Keyword} {Identifier}{LeadingSpace(BaseTypeList)} {OpenBrace} ... {CloseBrace}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitEnumDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitEnumDeclaration(this);
}
