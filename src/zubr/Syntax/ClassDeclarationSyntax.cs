using Zubr.Text;

namespace Zubr.Syntax;

public sealed class ClassDeclarationSyntax : TypeDeclarationSyntax
{
	public override SyntaxList<AttributeSyntax> Attributes { get; }

	public override TokenList Modifiers { get; }

	public override Token Keyword { get; }

	public override Token Identifier { get; }

	public override TypeParameterListSyntax? TypeParameterList { get; }

	public override ParameterListSyntax? ParameterList { get; }

	public override BaseTypeListSyntax? BaseTypeList { get; }

	public override SyntaxList<TypeParameterConstraintListSyntax> ConstraintList { get; }

	public override Token Semicolon { get; }

	public override Token OpenBrace { get; }

	public override SyntaxList<MemberDeclarationSyntax> Members { get; }

	public override Token CloseBrace { get; }

	internal ClassDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token keyword,
		Token identifier,
		TypeParameterListSyntax? typeParameterList,
		ParameterListSyntax? parameterList,
		BaseTypeListSyntax? baseTypeList,
		SyntaxList<TypeParameterConstraintListSyntax> constraintList,
		Token semicolon,
		Token openBrace,
		SyntaxList<MemberDeclarationSyntax> members,
		Token closeBrace
	) : base(tree, span, SyntaxKind.ClassDeclaration)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Keyword = keyword;
		Identifier = identifier;
		TypeParameterList = typeParameterList;
		ParameterList = parameterList;
		BaseTypeList = baseTypeList;
		ConstraintList = constraintList;
		Semicolon = semicolon;
		OpenBrace = openBrace;
		Members = members;
		CloseBrace = closeBrace;

		SetParent(attributes);
		SetParentIfNotNull(typeParameterList);
		SetParentIfNotNull(parameterList);
		SetParentIfNotNull(baseTypeList);
		SetParent(constraintList);
		SetParent(members);
	}

	public override string ToString()
	{
		if (Semicolon.Exists)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword} {Identifier}{TypeParameterList}{ParameterList}{LeadingSpace(BaseTypeList)}{LeadingSpace(ConstraintList)}{Semicolon}";
		}

		return $"{TrailingSpace(Modifiers)}{Keyword} {Identifier}{TypeParameterList}{ParameterList}{LeadingSpace(BaseTypeList)}{LeadingSpace(ConstraintList)} {OpenBrace} ... {CloseBrace}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitClassDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitClassDeclaration(this);
}
