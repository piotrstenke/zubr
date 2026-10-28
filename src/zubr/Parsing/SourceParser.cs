using System.Collections.Generic;
using System.Diagnostics;
using Zubr.Diagnostics;
using Zubr.Syntax;
using Zubr.Text;

namespace Zubr.Parsing;

internal sealed class SourceParser
{
	private readonly Token[] _tokens;
	private readonly SyntaxTree _tree;
	private int _current;

	private DiagnosticBag _diagnostics;

	internal SourceParser(SyntaxTree tree, Token[] tokens) : this(tree, tokens, default)
	{
	}

	internal SourceParser(SyntaxTree tree, Token[] tokens, DiagnosticBag diagnostics)
	{
		_tree = tree;
		_tokens = tokens;
		_diagnostics = diagnostics;
	}

	public CompilationUnitSyntax ParseCompilationUnit()
	{
		List<UseDirectiveSyntax> uses = new();
		List<MemberDeclarationSyntax> members = new();
		List<AttributeSyntax> attributes = new();

		Token token;

		while (!(token = Peek()).IsKind(TokenKind.Eof))
		{
			switch (token.Kind)
			{
				case TokenKind.Top:
				case TokenKind.Use:
					uses.Add(ParseUseDirective());
					break;

				case TokenKind.Module:
					members.Add(ParseModuleDeclaration());
					break;

				case TokenKind.OpenBracket:
					attributes.Add(ParseAttribute());
					break;

				//case TokenKind.AliasKeyword:
				//	aliases.Add(ParseAliasDirective());
				//	break;

				default:

					if (TryParseMemberDeclaration() is MemberDeclarationSyntax member)
					{
						members.Add(member);
						break;
					}

					Move();
					break;
			}
		}

		int end = token.Position;

		return new(_tree, new TextSpan(0, end), List(uses), List(attributes), List(members), token);
	}

	private ModuleDeclarationSyntax ParseModuleDeclaration()
	{
		Token moduleKeyword = EatToken();
		int start = moduleKeyword.Position;
		int end;

		Token topKeyword;

		NameSyntax? name;

		Token semicolonToken;

		if (PeekKind(TokenKind.Top))
		{
			name = null;

			topKeyword = EatToken();
			semicolonToken = EatToken(TokenKind.Semicolon);
		}
		else
		{
			topKeyword = default;

			name = ParseName();
			semicolonToken = EatToken(TokenKind.Semicolon);
		}

		List<MemberDeclarationSyntax> members = new();

		while (Peek().IsValid())
		{
			if (TryParseMemberDeclaration() is MemberDeclarationSyntax member)
			{
				members.Add(member);
			}
			else
			{
				break;
			}
		}

		if (members.Count > 0)
		{
			end = members[^1].Span.End;
		}
		else
		{
			end = semicolonToken.Position;
		}

		TextSpan span = GetSpan(start, end);

		return new(_tree, span, moduleKeyword, topKeyword, name, semicolonToken, List(members));
	}

	private UseDirectiveSyntax ParseUseDirective()
	{
		int offset = 1;

		if(PeekKind(TokenKind.Top))
		{
			offset = 2;
		}

		if(PeekKind(TokenKind.OpenBrace, offset))
		{
			return ParseListedUseDirective();
		}

		return ParseSimpleUseDirective();
	}

	private SimpleUseDirectiveSyntax ParseSimpleUseDirective()
	{
		Token topKeyword = PeekKind(TokenKind.Top)
			? EatToken()
			: default;

		Token useKeyword = EatToken(TokenKind.Use);

		SimpleNameSyntax name = ParseSimpleName();
		AliasSpecifierSyntax? alias = PeekKind(TokenKind.As)
			? ParseAliasSpecifier()
			: null;

		Token semicolon = EatToken(TokenKind.Semicolon);
		TextSpan span = topKeyword.Exists
			? GetSpan(topKeyword, semicolon)
			: GetSpan(useKeyword, semicolon);

		return new SimpleUseDirectiveSyntax(_tree, span, topKeyword, useKeyword, name, alias, semicolon);
	}

	private ListedUseDirectiveSyntax ParseListedUseDirective()
	{
		Token topKeyword = PeekKind(TokenKind.Top)
			? EatToken()
			: default;

		Token useKeyword = EatToken(TokenKind.Use);

		UseDirectiveElementListSyntax elements = ParseUseDirectiveElementList();

		Token from = EatToken(TokenKind.From);

		NameSyntax name = ParseName();

		AliasSpecifierSyntax? alias = PeekKind(TokenKind.As)
			? ParseAliasSpecifier()
			: null;

		Token semicolon = EatToken(TokenKind.Semicolon);

		TextSpan span = topKeyword.Exists
			? GetSpan(topKeyword, semicolon)
			: GetSpan(useKeyword, semicolon);

		return new(_tree, span, topKeyword, useKeyword, elements, from, name, alias, semicolon);
	}

	private UseDirectiveElementListSyntax ParseUseDirectiveElementList()
	{
		Token openBrace = EatToken(TokenKind.OpenBrace);

		List<(UseDirectiveElementSyntax element, Token)> elements = new();

		while (true)
		{
			Token token = Peek();

			if (!token.IsValid() || token.IsKind(TokenKind.CloseBrace))
			{
				break;
			}

			UseDirectiveElementSyntax element = ParseUseDirectiveElement();

			token = Peek();

			if (token.IsKind(TokenKind.Comma))
			{
				Move();
				elements.Add((element, token));
				continue;
			}

			elements.Add((element, default));
		}

		Token closeBrace = EatToken(TokenKind.CloseBrace);
		return new(_tree, GetSpan(openBrace, closeBrace), openBrace, List(elements), closeBrace);
	}

	private UseDirectiveElementSyntax ParseUseDirectiveElement()
	{
		SimpleNameSyntax name = ParseSimpleName();
		AliasSpecifierSyntax? alias = PeekKind(TokenKind.As)
			? ParseAliasSpecifier()
			: null;

		return new UseDirectiveElementSyntax(_tree, GetSpan(name, name), name, alias);
	}

	private AliasSpecifierSyntax ParseAliasSpecifier()
	{
		Token @as = EatToken(TokenKind.As);
		SimpleNameSyntax name = ParseSimpleName();
		return new(_tree, GetSpan(@as, name), @as, name);
	}

	private AssociatedTypeDeclarationSyntax ParseAssociatedTypeDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		Token keyword = EatToken(TokenKind.Type);

		Token identifier = EatToken(TokenKind.Identifier);

		EqualsTypeClauseSyntax? typeClause = PeekKind(TokenKind.Equals)
			? ParseEqualsTypeClause()
			: null;

		Token semicolon = EatToken(TokenKind.Semicolon);

		return new(_tree, GetSpan(position, semicolon), attributes, modifiers, keyword, identifier, typeClause, semicolon);
	}

	private MemberDeclarationSyntax? TryParseMemberDeclaration()
	{
		SyntaxList<AttributeSyntax> attributes = ParseAttributes();
		TokenList modifiers = ParseModifiers();

		Token token = Peek();
		int position = CalcMemberPosition(attributes, modifiers, token);

		while (true)
		{
			token = Peek();

			return token.GetContextualKind() switch
			{
				TokenKind.Class
					=> ParseClassDeclaration(attributes, modifiers, position),

				TokenKind.Struct
					=> ParseStructDeclaration(attributes, modifiers, position),

				TokenKind.Enum
					=> ParseEnumDeclaration(attributes, modifiers, position),

				TokenKind.Trait
					=> ParseTraitDeclaration(attributes, modifiers, position),

				TokenKind.New
					=> ParseConstructorDeclaration(attributes, modifiers, position),

				TokenKind.Free
					=> ParseDestructorDeclaration(attributes, modifiers, position),

				TokenKind.Identifier
					=> ParsePropertyDeclaration(attributes, modifiers, position),

				TokenKind.Cast
					=> ParseCastDeclaration(attributes, modifiers, position),

				TokenKind.Op
					=> ParseOperatorDeclaration(attributes, modifiers, position),

				TokenKind.Fn
					=> ParseFunctionDeclaration(attributes, modifiers, position),

				TokenKind.Self when PeekKind(TokenKind.OpenParen, 1)
					=> ParseInvokerDeclaration(attributes, modifiers, position),

				TokenKind.Self
					=> ParseIndexerDeclaration(attributes, modifiers, position),

				TokenKind.Type
					=> ParseAssociatedTypeDeclaration(attributes, modifiers, position),

				_ => null,
			};
		}
	}

	private EnumDeclarationSyntax ParseEnumDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		Token keyword = EatToken(TokenKind.Enum);

		Token identifier = EatToken(TokenKind.Identifier);

		BaseTypeListSyntax? baseTypeList = PeekKind(TokenKind.Colon)
			? ParseBaseTypeList()
			: null;

		if(PeekKind(TokenKind.Semicolon))
		{
			Token semicolon = EatToken();

			return new EnumDeclarationSyntax(_tree, GetSpan(position, semicolon), attributes, modifiers, keyword, identifier, baseTypeList, semicolon, default, default, default);
		}

		Token openBrace = EatToken(TokenKind.OpenBrace);
		SeparatedSyntaxList<EnumMemberDeclarationSyntax> members = ParseEnumMembers();

		Token closeBrace = EatToken(TokenKind.CloseBrace);

		return new EnumDeclarationSyntax(_tree, GetSpan(position, closeBrace), attributes, modifiers, keyword, identifier, baseTypeList, default, openBrace, members, closeBrace);
	}

	private SeparatedSyntaxList<EnumMemberDeclarationSyntax> ParseEnumMembers()
	{
		List<(EnumMemberDeclarationSyntax, Token)> members = new();

		Token token;

		while ((token = Peek()).IsValid())
		{
			if (token.IsKind(TokenKind.CloseBrace))
			{
				break;
			}

			EnumMemberDeclarationSyntax member = ParseEnumMemberDeclaration();

			if (PeekKind(TokenKind.Comma))
			{
				members.Add((member, EatToken()));
				continue;
			}

			members.Add((member, default));
		}

		return List(members);
	}

	private EnumMemberDeclarationSyntax ParseEnumMemberDeclaration()
	{
		SyntaxList<AttributeSyntax> attributes = ParseAttributes();
		TokenList modifiers = ParseModifiers();

		Token identifier = EatToken(TokenKind.Identifier);

		int position = CalcMemberPosition(attributes, modifiers, identifier);

		EqualsValueClauseSyntax? initializer = PeekKind(TokenKind.Equals)
			? ParseEqualsValueClause()
			: null;

		TextSpan span = initializer is null
			? GetSpan(position, identifier)
			: GetSpan(position, initializer);

		return new EnumMemberDeclarationSyntax(_tree, span, attributes, modifiers, identifier, initializer);
	}

	private TraitDeclarationSyntax ParseTraitDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		return (ParseTypeDeclaration(SyntaxKind.TraitDeclaration, attributes, modifiers, position) as TraitDeclarationSyntax)!;
	}

	private ClassDeclarationSyntax ParseClassDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		return (ParseTypeDeclaration(SyntaxKind.ClassDeclaration, attributes, modifiers, position) as ClassDeclarationSyntax)!;
	}

	private StructDeclarationSyntax ParseStructDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		foreach (Token modifier in modifiers)
		{
			if (!modifier.IsAccessModifier())
			{
				AddError(ErrorCode.ERR_InvalidModifier, modifier.Position);
			}
		}

		return (ParseTypeDeclaration(SyntaxKind.StructDeclaration, attributes, modifiers, position) as StructDeclarationSyntax)!;
	}

	private TypeDeclarationSyntax ParseTypeDeclaration(SyntaxKind kind, SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		Token keyword = EatToken();

		Token identifier = EatToken(TokenKind.Identifier);

		TypeParameterListSyntax? typeParameterList = PeekKind(TokenKind.LessThan)
			? ParseTypeParameterList()
			: null;

		ParameterListSyntax? parameterList = PeekKind(TokenKind.OpenParen)
			? ParseParameterList()
			: null;

		BaseTypeListSyntax? baseTypeList = PeekKind(TokenKind.Colon)
			? ParseBaseTypeList()
			: null;

		List<TypeParameterConstraintListSyntax>? constraints = null;

		if(PeekKind(TokenKind.Where))
		{
			constraints = new();

			do
			{
				constraints.Add(ParseConstraintList());
			}
			while (PeekKind(TokenKind.Where));
		}

		Token openBrace;
		SyntaxList<MemberDeclarationSyntax> members;
		Token closeBrace;

		Token semicolonToken;
		TextSpan span;

		if (PeekKind(TokenKind.Semicolon))
		{
			semicolonToken = EatToken();
			openBrace = default;
			members = default;
			closeBrace = default;
			span = GetSpan(position, semicolonToken);
		}
		else
		{
			openBrace = EatToken(TokenKind.OpenBrace);
			members = ParseTypeMembers();
			closeBrace = EatToken(TokenKind.CloseBrace);
			semicolonToken = default;
			span = GetSpan(position, closeBrace);
		}

		return kind switch
		{
			SyntaxKind.ClassDeclaration
				=> new ClassDeclarationSyntax(_tree, span, attributes, modifiers, keyword, identifier, typeParameterList, parameterList, baseTypeList, ListIfNotNull(constraints), semicolonToken, openBrace, members, closeBrace),

			SyntaxKind.StructDeclaration
				=> new StructDeclarationSyntax(_tree, span, attributes, modifiers, keyword, identifier, typeParameterList, parameterList, baseTypeList, ListIfNotNull(constraints), semicolonToken, openBrace, members, closeBrace),

			SyntaxKind.TraitDeclaration
				=> new TraitDeclarationSyntax(_tree, span, attributes, modifiers, keyword, identifier, typeParameterList, parameterList, baseTypeList, ListIfNotNull(constraints), semicolonToken, openBrace, members, closeBrace),

			_ => throw new UnreachableException()
		};
	}

	private SyntaxList<MemberDeclarationSyntax> ParseTypeMembers()
	{
		List<MemberDeclarationSyntax> members = new();

		Token token;

		while ((token = Peek()).IsValid() && !token.IsKind(TokenKind.CloseBrace))
		{
			if (TryParseMemberDeclaration() is MemberDeclarationSyntax member)
			{
				members.Add(member);
			}
		}

		return List(members);
	}

	private BaseTypeListSyntax ParseBaseTypeList()
	{
		Token colonToken = EatToken(TokenKind.Colon);

		List<(BaseTypeSyntax, Token)> baseTypes = new();

		Token token;

		while ((token = Peek()).IsValid())
		{
			if (ShouldStop(token))
			{
				break;
			}

			BaseTypeSyntax type = ParseBaseType();

			token = Peek();

			if (token.IsKind(TokenKind.Comma))
			{
				EatToken();
				baseTypes.Add((type, token));
				continue;
			}

			baseTypes.Add((type, default));
		}

		TextSpan span = GetSpan(colonToken, baseTypes);

		return new(_tree, span, colonToken, List(baseTypes));

		static bool ShouldStop(Token token)
		{
			return token.Kind is
				TokenKind.Where or
				TokenKind.OpenBrace or
				TokenKind.Semicolon;
		}
	}

	private BaseTypeSyntax ParseBaseType()
	{
		TypeSyntax type = ParseType();

		if (PeekKind(TokenKind.OpenParen))
		{
			ArgumentListSyntax argumentList = ParseArgumentList();

			TextSpan span = GetSpan(type, argumentList);
			return new PrimaryBaseTypeSyntax(_tree, span, type, argumentList);
		}

		return new SimpleBaseTypeSyntax(_tree, type.Span, type);
	}

	private ConstructorDeclarationSyntax ParseConstructorDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		Token keyword = EatToken(TokenKind.New);

		ParameterListSyntax parameterList = ParseParameterList();

		(BlockSyntax? body, ArrowExpressionClauseSyntax? expressionBody, Token semicolonToken) = ParseFunctionBody(out int end);

		TextSpan span = GetSpan(position, end);
		return new(_tree, span, attributes, modifiers, keyword, parameterList, body, expressionBody, semicolonToken);
	}

	private DestructorDeclarationSyntax ParseDestructorDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		Token keyword = EatContextualKeyword();

		ParameterListSyntax parameterList = ParseParameterList();

		(BlockSyntax? body, ArrowExpressionClauseSyntax? expressionBody, Token semicolonToken) = ParseFunctionBody(out int end);

		TextSpan span = GetSpan(position, end);

		return new(_tree, span, attributes, modifiers, keyword, parameterList, body, expressionBody, semicolonToken);
	}

	private PropertyDeclarationSyntax ParsePropertyDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		Token identifier = EatToken(TokenKind.Identifier);

		TypeSpecifierSyntax? typeSpecifier = PeekKind(TokenKind.Colon)
			? ParseTypeSpecifier()
			: null;

		(AccessorListSyntax? accessorList, ArrowExpressionClauseSyntax? expressionBody, EqualsValueClauseSyntax? initializer, Token semicolon) = ParsePropertyBody(out int end, allowInitializer: true);

		return new(_tree, GetSpan(position, end), attributes, modifiers, identifier, typeSpecifier, expressionBody, accessorList, initializer, semicolon);
	}

	private IndexerDeclarationSyntax ParseIndexerDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		Token keyword = EatToken(TokenKind.Self);
		BracketParameterListSyntax parameterList = ParseBracketParameterList();

		TypeSpecifierSyntax? typeSpecifier = PeekKind(TokenKind.Colon)
			? ParseTypeSpecifier()
			: null;

		(AccessorListSyntax? accessorList, ArrowExpressionClauseSyntax? expressionBody, _, Token semicolon) = ParsePropertyBody(out int end, allowInitializer: false);

		return new(_tree, GetSpan(position, end), attributes, modifiers, keyword, parameterList, typeSpecifier, expressionBody, accessorList, semicolon);
	}

	private AccessorListSyntax ParseAccesorList()
	{
		Token openBrace = EatToken(TokenKind.OpenBrace);

		List<AccessorDeclarationSyntax> accessors = new();

		Token token;

		while ((token = Peek()).IsValid())
		{
			if (token.IsKind(TokenKind.CloseBrace))
			{
				break;
			}

			SyntaxList<AttributeSyntax> attributes = ParseAttributes();
			TokenList modifiers = ParseModifiers();

			Token keyword = EatContextualKeyword();

			int position = CalcMemberPosition(attributes, modifiers, keyword);

			SyntaxKind kind = keyword.GetAccessorKind();

			if (kind == SyntaxKind.None)
			{
				AddError(ErrorCode.ERR_SyntaxError);
				keyword = UnexpectedToken();
			}

			(BlockSyntax? body, ArrowExpressionClauseSyntax? expressionBody, Token semicolon) = ParseFunctionBody(out int end);

			accessors.Add(new(_tree, GetSpan(position, end), kind, attributes, modifiers, keyword, body, expressionBody, semicolon));
		}

		Token closeBrace = EatToken(TokenKind.CloseBrace);

		TextSpan span = GetSpan(openBrace, closeBrace);

		return new(_tree, span, openBrace, List(accessors), closeBrace);
	}

	private CastDeclarationSyntax ParseCastDeclaration(
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		int position
	)
	{
		Token keyword = EatToken(TokenKind.Cast);

		ParameterListSyntax parameterList = ParseParameterList();

		TypeSyntax type = ParseType();

		(BlockSyntax? body, ArrowExpressionClauseSyntax? expressionBody, Token semicolonToken) = ParseFunctionBody(out int end);

		TextSpan span = GetSpan(position, end);

		return new(_tree, span, attributes, modifiers, keyword, parameterList, type, body, expressionBody, semicolonToken);
	}

	private OperatorDeclarationSyntax ParseOperatorDeclaration(
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		int position
	)
	{
		Token keyword = EatToken(TokenKind.Op);
		Token @operator = EatToken();

		if (!@operator.IsOverloadableOperator())
		{
			AddError(ErrorCode.ERR_SyntaxError, keyword.Position);
			@operator = UnexpectedToken(@operator);
		}

		ParameterListSyntax parameterList = ParseParameterList();

		TypeSyntax type = ParseType();

		(BlockSyntax? body, ArrowExpressionClauseSyntax? expressionBody, Token semicolonToken) = ParseFunctionBody(out int end);

		TextSpan span = GetSpan(position, end);

		return new(_tree, span, attributes, modifiers, keyword, @operator, parameterList, type, body, expressionBody, semicolonToken);
	}

	private InvokerDeclarationSyntax ParseInvokerDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		Token keyword = EatToken(TokenKind.Self);
		ParameterListSyntax parameterList = ParseParameterList();

		TypeSyntax? returnType = PeekKind(TokenKind.OpenBrace)
			? null
			: ParseType();

		(BlockSyntax? body, ArrowExpressionClauseSyntax? expressionBody, Token semicolonToken) = ParseFunctionBody(out int end);

		TextSpan span = GetSpan(position, end);

		return new(_tree, span, attributes, modifiers, keyword, parameterList, returnType, body, expressionBody, semicolonToken);
	}

	private FunctionDeclarationSyntax ParseFunctionDeclaration(SyntaxList<AttributeSyntax> attributes, TokenList modifiers, int position)
	{
		Token keyword = EatToken(TokenKind.Fn);
		Token identifier = EatToken(TokenKind.Identifier);

		TypeParameterListSyntax? typeParameterList = PeekKind(TokenKind.LessThan)
			? ParseTypeParameterList()
			: null;

		ParameterListSyntax parameterList = ParseParameterList();

		TypeSpecifierSyntax? returnType = PeekKind(TokenKind.Colon)
			? ParseTypeSpecifier()
			: null;

		List<TypeParameterConstraintListSyntax>? constraints = null;

		if (PeekKind(TokenKind.Where))
		{
			constraints = new();

			do
			{
				constraints.Add(ParseConstraintList());
			}
			while (PeekKind(TokenKind.Where));
		}

		(BlockSyntax? body, ArrowExpressionClauseSyntax? expressionBody, Token semicolonToken) = ParseFunctionBody(out int end);

		TextSpan span = GetSpan(position, end);

		return new(_tree, span, attributes, modifiers, keyword, identifier, typeParameterList, parameterList, returnType, ListIfNotNull(constraints), body, expressionBody, semicolonToken);
	}

	private TypeParameterListSyntax ParseTypeParameterList()
	{
		Token lessThanToken = EatToken(TokenKind.LessThan);

		List<(TypeParameterSyntax, Token)> parameters = new();

		while (true)
		{
			TypeParameterSyntax typeParameter = ParseTypeParameter();

			Token token = Peek();

			if (token.IsKind(TokenKind.GreaterThan))
			{
				parameters.Add((typeParameter, default));
				break;
			}

			if (!EatToken(TokenKind.Comma, out token))
			{
				parameters.Add((typeParameter, default));
				break;
			}

			parameters.Add((typeParameter, token));
		}

		Token greaterThanToken = EatToken(TokenKind.GreaterThan);

		TextSpan span = GetSpan(lessThanToken, greaterThanToken);

		return new(_tree, span, lessThanToken, List(parameters), greaterThanToken);
	}

	private TypeParameterSyntax ParseTypeParameter()
	{
		SyntaxList<AttributeSyntax> attributes = ParseAttributes();
		Token identifier = EatToken(TokenKind.Identifier);

		TypeParameterInlineConstraintListSyntax? constraint = null;
		EqualsTypeClauseSyntax? defaultType = null;

		if (PeekKind(TokenKind.Colon))
		{
			constraint = ParseInlineConstraintList();
		}

		if (PeekKind(TokenKind.Equals))
		{
			defaultType = ParseEqualsTypeClause();
		}

		TextSpan span;

		if (defaultType is not null)
		{
			span = GetSpan(identifier, defaultType);
		}
		else if (constraint is not null)
		{
			span = GetSpan(identifier, constraint);
		}
		else
		{
			span = identifier.Span;
		}

		return new(_tree, span, attributes, identifier, constraint, defaultType);
	}

	private TypeParameterInlineConstraintListSyntax ParseInlineConstraintList()
	{
		Token colon = EatToken(TokenKind.Colon);

		List<(TypeParameterConstraintSyntax, Token)> constrains = new();

		while (true)
		{
			TypeParameterConstraintSyntax? constraint = TryParseConstraint();

			if (constraint is null)
			{
				break;
			}

			if (PeekKind(TokenKind.Comma))
			{
				constrains.Add((constraint, EatToken()));
			}
			else
			{
				constrains.Add((constraint, default));
			}
		}

		return new(_tree, GetSpan(colon, constrains), colon, List(constrains));
	}

	private TypeParameterConstraintListSyntax ParseConstraintList()
	{
		Token whereKeyword = EatToken(TokenKind.Where);
		NameSyntax name = ParseName();
		Token colon = EatToken(TokenKind.Colon);

		List<(TypeParameterConstraintSyntax, Token)> constrains = new();

		while(true)
		{
			TypeParameterConstraintSyntax? constraint = TryParseConstraint();

			if(constraint is null)
			{
				break;
			}

			if(PeekKind(TokenKind.Comma))
			{
				constrains.Add((constraint, EatToken()));
			}
			else
			{
				constrains.Add((constraint, default));
			}
		}

		return new(_tree, GetSpan(whereKeyword, constrains), whereKeyword, name, colon, List(constrains));
	}

	private TypeParameterConstraintSyntax? TryParseConstraint()
	{
		Token token = Peek();

		TokenKind kind = token.GetContextualKind();

		switch (kind)
		{
			case TokenKind.Class:
				return Keyword(token, SyntaxKind.ClassConstraint, true);

			case TokenKind.Struct:
				return Keyword(token, SyntaxKind.StructConstraint, true);

			case TokenKind.Enum:
				return Keyword(token, SyntaxKind.EnumConstraint, true);

			case TokenKind.Identifier:
				NameSyntax name = ParseName();
				return new TypeConstraintSyntax(_tree, name.Span, name);

			default:
				return null;
		}

		KeywordConstraintSyntax Keyword(in Token token, SyntaxKind kind, bool allowQuestion)
		{
			EatToken();

			Token questionToken;

			if (PeekKind(TokenKind.Question))
			{
				if (allowQuestion)
				{
					questionToken = EatToken();
				}
				else
				{
					AddError(ErrorCode.ERR_SyntaxError);
					questionToken = UnexpectedToken();
				}
			}
			else
			{
				questionToken = default;
			}

			return new KeywordConstraintSyntax(_tree, token.Span, kind, token, questionToken);
		}
	}

	private BracketParameterListSyntax ParseBracketParameterList()
	{
		SeparatedSyntaxList<ParameterSyntax> parameters = ParseParameterList(TokenKind.OpenBracket, TokenKind.CloseBracket, out Token openParen, out Token closeParen);
		return new(_tree, GetSpan(openParen, closeParen), openParen, parameters, closeParen);
	}

	private ParameterListSyntax ParseParameterList()
	{
		SeparatedSyntaxList<ParameterSyntax> parameters = ParseParameterList(TokenKind.OpenParen, TokenKind.CloseParen, out Token openParen, out Token closeParen);
		return new(_tree, GetSpan(openParen, closeParen), openParen, parameters, closeParen);
	}

	private SeparatedSyntaxList<ParameterSyntax> ParseParameterList(TokenKind openKind, TokenKind closeKind, out Token openToken, out Token closeToken)
	{
		openToken = EatToken(openKind);

		Token current = Peek();

		if(current.IsKind(closeKind))
		{
			closeToken = EatToken();
			return default;
		}

		List<(ParameterSyntax parameter, Token separator)> parameters = new();

		while (current.IsValid())
		{
			ParameterSyntax parameter = ParseParameter();

			current = Peek();

			if (current.IsKind(closeKind))
			{
				EatToken();
				parameters.Add((parameter, default));

				break;
			}

			if (current.IsKind(TokenKind.Comma))
			{
				EatToken();
				parameters.Add((parameter, current));

				continue;
			}
		}

		closeToken = current;
		return List(parameters);
	}

	private ParameterSyntax ParseParameter()
	{
		SyntaxList<AttributeSyntax> attributes = ParseAttributes();
		TokenList modifiers = ParseModifiers();

		Token identifier = EatToken(TokenKind.Identifier);

		TypeSpecifierSyntax typeSpecifier = ParseTypeSpecifier();

		int position = CalcMemberPosition(attributes, modifiers, identifier);

		EqualsValueClauseSyntax? @default = null;

		TextSpan span;

		if (PeekKind(TokenKind.Equals))
		{
			@default = ParseEqualsValueClause();
			span = GetSpan(position, @default);
		}
		else
		{
			span = GetSpan(position, identifier);
		}

		return new(_tree, span, attributes, modifiers, identifier, typeSpecifier, @default);
	}

	private PredefinedTypeSyntax ParsePredefinedType()
	{
		Token token = EatToken();

		TextSpan span = token.Span;
		return new(_tree, span, token);
	}

	private NameSyntax ParseName()
	{
		NameSyntax name;

		if (PeekKind(TokenKind.Top))
		{
			Token top = EatToken();
			Token token = EatToken(TokenKind.ColonColon);
			SimpleNameSyntax right = ParseSimpleName();
			name = new TopQualifiedNameSyntax(_tree, GetSpan(token, right), top, token, right);
		}
		else if (PeekKind(TokenKind.ColonColon))
		{
			Token token = EatToken();
			SimpleNameSyntax right = ParseSimpleName();
			name = new TopQualifiedNameSyntax(_tree, GetSpan(token, right), default, token, right);
		}
		else
		{
			name = ParseSimpleName();
		}

		while (PeekKind(TokenKind.Dot))
		{
			Token dot = EatToken();

			SimpleNameSyntax right = ParseSimpleName();

			TextSpan span = GetSpan(name, right);

			name = new QualifiedNameSyntax(_tree, span, name, dot, right);
		}

		return name;
	}

	private SimpleNameSyntax ParseSimpleName()
	{
		if (PeekKind(TokenKind.LessThan, 1))
		{
			return ParseGenericName();
		}

		return ParseIdentifierName();
	}

	private IdentifierNameSyntax ParseIdentifierName()
	{
		Token identifier = EatToken(TokenKind.Identifier);

		TextSpan span = identifier.Span;

		return new(_tree, span, identifier);
	}

	private GenericNameSyntax ParseGenericName()
	{
		Token identifier = EatToken(TokenKind.Identifier);
		TypeArgumentListSyntax list = ParseTypeArgumentList();

		TextSpan span = GetSpan(identifier, list);

		return new GenericNameSyntax(_tree, span, identifier, list);
	}

	private TypeArgumentListSyntax ParseTypeArgumentList()
	{
		Token lessThanToken = EatToken(TokenKind.LessThan);

		List<(TypeSyntax, Token)> args = new();

		while (true)
		{
			TypeSyntax type;

			if (PeekKind(TokenKind.GreaterThan) || PeekKind(TokenKind.Comma))
			{
				type = SkippedTypeArgument();
			}
			else
			{
				type = ParseType();
			}

			if (PeekKind(TokenKind.Comma))
			{
				args.Add((type, EatToken()));
				continue;
			}

			if (PeekKind(TokenKind.GreaterThan))
			{
				args.Add((type, default));
				break;
			}
		}

		Token greaterThanToken = EatToken(TokenKind.GreaterThan);

		TextSpan span = GetSpan(lessThanToken, greaterThanToken);

		return new(_tree, span, lessThanToken, List(args), greaterThanToken);

		TypeSyntax SkippedTypeArgument()
		{
			Token skippedToken = SkippedToken();
			return new SkippedTypeArgumentSyntax(_tree, skippedToken.Span, skippedToken);
		}
	}

	private TypeSyntax ParseType()
	{
		//if (PeekKind(TokenKind.OpenParen))
		//{
		//	return ParseTupleType();
		//}

		TypeSyntax type = ParseNameOrTypeKeyword();

		ref readonly Token token = ref Peek();

		switch (token.Kind)
		{
			case TokenKind.Question:
				{
					TextSpan span = GetSpan(type, token);
					return new NullableTypeSyntax(_tree, span, type, token);
				}

			case TokenKind.Asterisk:
				{
					TextSpan span = GetSpan(type, token);
					return new PointerTypeSyntax(_tree, span, type, token);
				}

			case TokenKind.Ampersand:
				{
					TextSpan span = GetSpan(type, token);
					return new ReferenceTypeSyntax(_tree, span, type, token);
				}

			case TokenKind.OpenBracket:
				{
					SyntaxList<ArrayRankSyntax> ranks = ParseArrayRanks();

					TextSpan span = GetSpan(type, ranks);

					return new ArrayTypeSyntax(_tree, span, type, ranks);
				}

			//case TokenKind.Bar:
			//	{
			//		List<(TypeSyntax, Token)> list = new()
			//		{
			//			(type, EatToken())
			//		};

			//		while (true)
			//		{
			//			type = ParseType();

			//			if (PeekKind(TokenKind.Bar))
			//			{
			//				list.Add((type, EatToken()));
			//			}
			//			else
			//			{
			//				list.Add((type, default));
			//				break;
			//			}
			//		}

			//		SeparatedSyntaxList<TypeSyntax> typeElements = List(list);
			//		return new UnionTypeSyntax(_tree, type.Span, typeElements);
			// }

			default:
				return type;
		}

		TypeSyntax ParseNameOrTypeKeyword()
		{
			Token token = Peek();

			if (token.IsPredefinedType())
			{
				return ParsePredefinedType();
			}

			//if (token.IsKind(TokenKind.LetKeyword))
			//{
			//	return ParseLetType();
			//}

			return ParseName();
		}
	}

	private SyntaxList<ArrayRankSyntax> ParseArrayRanks()
	{
		List<ArrayRankSyntax> ranks = new();

		Token token;

		while ((token = Peek()).IsValid())
		{
			if (!token.IsKind(TokenKind.OpenBracket))
			{
				break;
			}

			Move();

			SeparatedSyntaxList<ExpressionSyntax> sizes = ParseArraySizes();

			Token closeBracket = EatToken(TokenKind.CloseBracket);

			TextSpan span = GetSpan(token, closeBracket);

			ranks.Add(new(_tree, span, token, sizes, closeBracket));
		}

		return List(ranks);
	}

	private SeparatedSyntaxList<ExpressionSyntax> ParseArraySizes()
	{
		List<(ExpressionSyntax, Token)> expressions = new();

		Token token;

		while ((token = Peek()).IsValid())
		{
			if (token.IsKind(TokenKind.CloseBracket))
			{
				if (expressions.Count == 0)
				{
					Token skippedToken = SkippedToken(token);
					TextSpan span = skippedToken.Span;
					ExpressionSyntax expr = new SkippedArraySizeExpressionSyntax(_tree, span, skippedToken);

					expressions.Add((expr, default));
				}

				break;
			}

			if (token.IsKind(TokenKind.Comma))
			{
				Move();
				Token skippedToken = SkippedToken(token);
				TextSpan span = skippedToken.Span;
				ExpressionSyntax expr = new SkippedArraySizeExpressionSyntax(_tree, span, skippedToken);

				expressions.Add((expr, token));
				continue;
			}

			expressions.Add((ParseExpression(), default));
		}

		return List(expressions);
	}

	private (AccessorListSyntax? accessorList, ArrowExpressionClauseSyntax? expression, EqualsValueClauseSyntax? initializer, Token semicolon) ParsePropertyBody(out int end, bool allowInitializer)
	{
		ref readonly Token token = ref Peek();

		switch(token.Kind)
		{
			case TokenKind.OpenBrace:
				{
					AccessorListSyntax accessorList = ParseAccesorList();

					if(allowInitializer && PeekKind(TokenKind.Equals))
					{
						EqualsValueClauseSyntax? initializer = ParseEqualsValueClause();
						Token semicolon = EatToken(TokenKind.Semicolon);

						end = semicolon.Span.End;
						return (accessorList, null, initializer, semicolon);
					}
					else
					{
						end = accessorList.Span.End;
						return (accessorList, null, null, default);
					}
				}

			case TokenKind.EqualsGreaterThan:
				{
					EatToken();
					ExpressionSyntax expr = ParseExpression();
					TextSpan span = GetSpan(token, expr);
					ArrowExpressionClauseSyntax exprBody = new(_tree, span, token, expr);

					Token semicolon = EatToken(TokenKind.Semicolon);
					end = semicolon.Span.End;
					return (null, exprBody, null, semicolon);
				}

			case TokenKind.Equals when allowInitializer:
				{
					EqualsValueClauseSyntax initializer = ParseEqualsValueClause();
					Token semicolon = EatToken(TokenKind.Semicolon);
					end = semicolon.Span.End;
					return (null, null, initializer, semicolon);
				}

			case TokenKind.Semicolon:
				{
					EatToken();
					end = token.Span.End;
					return (null, null, null, token);
				}

			default:
				end = token.Span.End;
				return default;
		}
	}

	private (BlockSyntax? block, ArrowExpressionClauseSyntax? expression, Token semicolonToken) ParseFunctionBody(out int end)
	{
		ref readonly Token token = ref Peek();

		switch (token.Kind)
		{
			case TokenKind.OpenBrace:
				BlockSyntax block = ParseBlock();
				end = block.Span.End;
				return (block, null, default);

			case TokenKind.EqualsGreaterThan:
				EatToken();
				ExpressionSyntax expr = ParseExpression();
				TextSpan span = GetSpan(token, expr);
				ArrowExpressionClauseSyntax exprBody = new(_tree, span, token, expr);

				Token semicolonToken = EatToken(TokenKind.Semicolon);
				end = semicolonToken.Span.End;
				return (null, exprBody, semicolonToken);

			case TokenKind.Semicolon:
				EatToken();
				end = token.Span.End;
				return (null, null, token);

			default:
				end = token.Span.End;
				return default;
		}
	}

	private BlockSyntax ParseBlock()
	{
		Token openBrace = EatToken(TokenKind.OpenBrace);

		List<StatementSyntax> statements = new();

		Token token;

		while ((token = Peek()).IsValid())
		{
			if (token.IsKind(TokenKind.CloseBrace))
			{
				break;
			}

			statements.Add(ParseStatement());
		}

		Token closeBrace = EatToken(TokenKind.CloseBrace);
		TextSpan span = GetSpan(openBrace, closeBrace);

		return new(_tree, span, openBrace, List(statements), closeBrace);
	}

	private StatementSyntax ParseStatement()
	{
		Token token = Peek();

		return token.GetContextualKind() switch
		{
			TokenKind.Open => ParseBlock(),
			TokenKind.If => ParseIfStatement(),
			TokenKind.Where => ParseWhileStatement(),
			TokenKind.Do => ParseDoWhileStatement(),
			TokenKind.For => ParseForStatement(),
			TokenKind.Jump => ParseJumpStatement(),
			TokenKind.Identifier => ParseLocalOrExpressionStatement(),
			TokenKind.Return => ParseReturnStatement(),
			TokenKind.Next => ParseNextStatement(),
			TokenKind.Stop => ParseStopStatement(),
			_ => ParseLocalOrExpressionStatement(),
		};
	}

	private ForStatementSyntax ParseForStatement()
	{
		Token forKeyword = EatToken(TokenKind.For);

		Token openParen = EatToken(TokenKind.OpenParen);
		Token identifier = EatToken(TokenKind.Identifier);
		Token colon = EatToken(TokenKind.Colon);

		ExpressionSyntax expression = ParseExpression();

		Token closeParen = EatToken(TokenKind.CloseParen);

		StatementSyntax statement = ParseStatement();

		TextSpan span = GetSpan(forKeyword, statement);
		return new(_tree, span, forKeyword, openParen, identifier, colon, expression, closeParen, statement);
	}

	private ReturnStatementSyntax ParseReturnStatement()
	{
		Token returnKeyword = EatToken(TokenKind.Return);

		ExpressionSyntax? expression = null;

		if (!PeekKind(TokenKind.Semicolon))
		{
			expression = ParseExpression();
		}

		Token semicolon = EatToken(TokenKind.Semicolon);

		TextSpan span = GetSpan(returnKeyword, semicolon);

		return new(_tree, span, returnKeyword, expression, semicolon);
	}

	private NextStatementSyntax ParseNextStatement()
	{
		Token nextKeyword = EatContextualKeyword(TokenKind.Next);
		Token semicolon = EatToken(TokenKind.Semicolon);

		TextSpan span = GetSpan(nextKeyword, semicolon);

		return new(_tree, span, nextKeyword, semicolon);
	}

	private StopStatementSyntax ParseStopStatement()
	{
		Token stopKeyword = EatContextualKeyword(TokenKind.Stop);
		Token semicolon = EatToken(TokenKind.Semicolon);

		TextSpan span = GetSpan(stopKeyword, semicolon);

		return new(_tree, span, stopKeyword, semicolon);
	}

	private StatementSyntax ParseLocalOrExpressionStatement()
	{
		Token token = Peek();

		if (token.IsKind(TokenKind.Identifier) && PeekKind(TokenKind.Colon, 1))
		{
			return ParseLabelStatement();
		}

		if (token.IsPredefinedType())
		{
			if (!PeekKind(TokenKind.Dot, 1))
			{
				return ParseLocalDeclaration();
			}

			if (PeekKind(TokenKind.Colon, 1))
			{
				return ParseLabelStatement();
			}
		}

		ExpressionSyntax expr = ParseExpression();
		Token semicolonToken = EatToken();

		TextSpan span = GetSpan(token, semicolonToken);

		return new ExpressionStatementSyntax(_tree, span, expr, semicolonToken);
	}

	private TokenList ParseModifiers()
	{
		if (!Peek().IsModifier())
		{
			return TokenList.Empty;
		}

		Token token = EatContextualKeyword();

		List<Token> tokens = new()
		{
			token
		};

		while (Peek().IsModifier())
		{
			token = EatContextualKeyword();
			tokens.Add(token);
		}

		return List(tokens);
	}

	private LocalDeclarationStatementSyntax ParseLocalDeclaration()
	{
		TokenList modifiers = ParseModifiers();

		Token identifier = EatToken(TokenKind.Identifier);

		TypeSpecifierSyntax? typeSpecifier = PeekKind(TokenKind.Colon)
			? ParseTypeSpecifier()
			: null;

		EqualsValueClauseSyntax? initializer = PeekKind(TokenKind.Equals)
			? ParseEqualsValueClause()
			: null;

		int position = modifiers.IsDefaultOrEmpty
			? identifier.Position
			: modifiers.Position;

		Token semicolon = EatToken(TokenKind.Semicolon);

		if (typeSpecifier?.Type is PredefinedTypeSyntax p && p.Keyword.IsKind(TokenKind.Void))
		{
			AddError(ErrorCode.ERR_SyntaxError);
		}

		TextSpan span = GetSpan(position, semicolon);

		return new(_tree, span, modifiers, identifier, typeSpecifier, initializer, semicolon);
	}

	private TypeSpecifierSyntax ParseTypeSpecifier()
	{
		Token colon = EatToken(TokenKind.Colon);
		TypeSyntax type = ParseType();
		TextSpan span = GetSpan(colon, type);

		return new(_tree, span, colon, type);
	}

	private EqualsValueClauseSyntax ParseEqualsValueClause()
	{
		Token equalsToken = EatToken(TokenKind.Equals);
		ExpressionSyntax expr = ParseExpression();

		TextSpan span = GetSpan(equalsToken, expr);

		return new(_tree, span, equalsToken, expr);
	}

	private EqualsTypeClauseSyntax ParseEqualsTypeClause()
	{
		Token equalsToken = EatToken(TokenKind.Equals);
		TypeSyntax type = ParseType();

		return new(_tree, GetSpan(equalsToken, type), equalsToken, type);
	}

	private WhileStatementSyntax ParseWhileStatement()
	{
		Token whileKeyword = EatToken(TokenKind.While);
		Token openParen = EatToken(TokenKind.OpenParen);

		ExpressionSyntax condition = ParseExpression();

		Token closeParen = EatToken(TokenKind.CloseParen);

		StatementSyntax statement = ParseStatement();

		TextSpan span = GetSpan(whileKeyword, statement);

		return new(_tree, span, whileKeyword, openParen, condition, closeParen, statement);
	}

	private DoWhileStatementSyntax ParseDoWhileStatement()
	{
		Token doKeyword = EatToken(TokenKind.Do);

		StatementSyntax statement = ParseStatement();

		Token whileKeyword = EatToken(TokenKind.While);
		Token openParen = EatToken(TokenKind.OpenParen);

		ExpressionSyntax condition = ParseExpression();

		Token closeParen = EatToken(TokenKind.CloseParen);
		Token semicolon = EatToken(TokenKind.Semicolon);

		TextSpan span = GetSpan(doKeyword, semicolon);

		return new(_tree, span, doKeyword, statement, whileKeyword, openParen, condition, closeParen, semicolon);
	}

	private JumpStatementSyntax ParseJumpStatement()
	{
		Token keyword = EatToken(TokenKind.Jump);

		Token identifier = EatToken(TokenKind.Identifier);

		Token semicolon = EatToken(TokenKind.Semicolon);

		TextSpan span = GetSpan(keyword, semicolon);
		return new(_tree, span, keyword, identifier, semicolon);
	}

	private LabelStatementSyntax ParseLabelStatement()
	{
		Token identifier = EatToken(TokenKind.Identifier);

		Token colon = EatToken(TokenKind.Colon);

		StatementSyntax statement = ParseStatement();

		TextSpan span = GetSpan(identifier, statement);

		return new(_tree, span, identifier, colon, statement);
	}

	private IfStatementSyntax ParseIfStatement()
	{
		Token ifKeyword = EatToken(TokenKind.If);
		Token openParen = EatToken(TokenKind.OpenParen);

		ExpressionSyntax condition = ParseExpression();

		Token closeParen = EatToken(TokenKind.CloseParen);

		StatementSyntax statement = ParseStatement();

		List<ElifClauseSyntax>? elifs = null;
		ElseClauseSyntax? @else = null;

		if (PeekKind(TokenKind.Elif))
		{
			elifs = new()
			{
				ParseElifClause()
			};

			while (PeekKind(TokenKind.Elif))
			{
				elifs.Add(ParseElifClause());
			}
		}

		if (PeekKind(TokenKind.Else))
		{
			@else = ParseElseClause();
		}

		TextSpan span;

		if (@else is not null)
		{
			span = GetSpan(ifKeyword, @else);
		}
		else if (elifs is not null)
		{
			span = GetSpan(ifKeyword, elifs);
		}
		else
		{
			span = GetSpan(ifKeyword, statement);
		}

		return new(_tree, span, ifKeyword, openParen, condition, closeParen, statement, ListIfNotNull(elifs), @else);
	}

	private ElifClauseSyntax ParseElifClause()
	{
		Token elifKeyword = EatToken(TokenKind.Elif);
		Token openParen = EatToken(TokenKind.OpenParen);

		ExpressionSyntax condition = ParseExpression();

		Token closeParen = EatToken(TokenKind.CloseParen);

		StatementSyntax statement = ParseStatement();

		TextSpan span = GetSpan(elifKeyword, statement);

		return new(_tree, span, elifKeyword, openParen, condition, closeParen, statement);
	}

	private ElseClauseSyntax ParseElseClause()
	{
		Token elseKeyword = EatToken();

		StatementSyntax statement = ParseStatement();

		if (statement.IsKind(SyntaxKind.IfStatement))
		{
			AddError(ErrorCode.ERR_ElseIfNotSupported, statement.Position);
		}

		TextSpan span = GetSpan(elseKeyword, statement);

		return new(_tree, span, elseKeyword, statement);
	}

	private SyntaxList<AttributeSyntax> ParseAttributes()
	{
		Token token = Peek();

		if (!token.IsKind(TokenKind.OpenBracket))
		{
			return default;
		}

		List<AttributeSyntax> attributes = new();

		while (PeekKind(TokenKind.OpenBracket))
		{
			attributes.Add(ParseAttribute());
		}

		return List(attributes);
	}

	private AttributeSyntax ParseAttribute()
	{
		Token openBracket = EatToken(TokenKind.OpenBracket);
		AttributeTargetSyntax? target = TryParseAttributeTarget();

		NameSyntax name = ParseName();

		AttributeArgumentListSyntax? argumentList = TryParseAttributeArgumentList();

		Token closeBracket = EatToken(TokenKind.CloseBracket);

		TextSpan span = GetSpan(openBracket, closeBracket);

		return new(_tree, span, openBracket, target, name, argumentList, closeBracket);
	}

	private AttributeTargetSyntax? TryParseAttributeTarget()
	{
		Token token = Peek();

		TokenKind kind = token.GetContextualKind();

		// TODO: Handle all attribute target specifiers.
		switch (kind)
		{
			case TokenKind.Return:
			case TokenKind.Field:
				Token targetKeyword = AcceptContextualKeyword(kind);
				Token colonToken = EatToken(TokenKind.Colon);

				TextSpan span = GetSpan(targetKeyword, colonToken);

				return new(_tree, span, targetKeyword, colonToken);

			default:
				return null;
		}
	}

	private AttributeArgumentListSyntax? TryParseAttributeArgumentList()
	{
		Token openParen = Peek();

		if (!openParen.IsKind(TokenKind.OpenParen))
		{
			return null;
		}

		List<(AttributeArgumentSyntax, Token)> args = new();

		while (true)
		{
			Token token = Peek();

			if (!token.IsValid() || token.IsKind(TokenKind.CloseParen))
			{
				break;
			}

			AttributeArgumentSyntax arg = ParseAttributeArgument();

			token = Peek();

			if (token.IsKind(TokenKind.Comma))
			{
				args.Add((arg, token));
				continue;
			}

			args.Add((arg, default));
		}

		Token closeParen = EatToken(TokenKind.CloseParen);
		TextSpan span = GetSpan(openParen, closeParen);

		return new(_tree, span, openParen, List(args), closeParen);
	}

	private AttributeArgumentSyntax ParseAttributeArgument()
	{
		ExpressionSyntax expr = ParseExpression();

		return new(_tree, expr.Span, expr);
	}

	private ExpressionSyntax ParseExpression(Precedence precedence = default)
	{
		Token token = Peek();
		SyntaxKind kind = SyntaxFacts.GetPrefixUnaryExpressionKind(token.Kind);

		ExpressionSyntax expr;

		if (kind != default)
		{
			Move();
			expr = ParseExpression(GetPrecedence(kind));
			TextSpan span = GetSpan(token, expr);
			return new PrefixUnaryExpressionSyntax(_tree, span, kind, token, expr);
		}

		ExpressionSyntax primary = ParsePrimaryExpression();
		primary = ParsePostfixExpression(primary);

		expr = primary;

		while (TryParseSubExpression(expr, precedence) is ExpressionSyntax sub)
		{
			expr = sub;
		}

		//if (PeekKind(TokenKind.Question) && precedence <= Precedence.Conditional)
		//{
		//	Token questionToken = EatToken();
		//	ExpressionSyntax trueExpression = ParseExpression();

		//	Token colonToken = EatToken(TokenKind.Colon);
		//	ExpressionSyntax falseExpression = ParseExpression();

		//	TextSpan span = GetSpan(expr, falseExpression);

		//	expr = new ConditionalExpressionSyntax(_tree, span, expr, questionToken, trueExpression, colonToken, falseExpression);
		//}

		return expr;
	}

	private ExpressionSyntax? TryParseSubExpression(ExpressionSyntax left, Precedence precedence)
	{
		SyntaxKind exprKind = SyntaxFacts.GetBinaryExpressionKind(PeekKind());

		if (exprKind == SyntaxKind.None)
		{
			return null;
		}

		Precedence newPrecedence = GetPrecedence(exprKind);

		if (newPrecedence < precedence)
		{
			return null;
		}

		if (newPrecedence == precedence && !IsRightAssociative(exprKind))
		{
			return null;
		}

		if (exprKind == SyntaxKind.RangeExpression)
		{
			return ParseRangeExpression(left, newPrecedence);
		}

		Token operatorToken = EatToken();

		ExpressionSyntax right = ParseExpression(newPrecedence);

		TextSpan span = GetSpan(left, right);

		if (operatorToken.IsAssignmentOperator())
		{
			return new AssignmentExpressionSyntax(_tree, span, left, operatorToken, right);
		}

		return new BinaryExpressionSyntax(_tree, span, exprKind, left, operatorToken, right);
	}

	private RangeExpressionSyntax ParseRangeExpression(ExpressionSyntax left, Precedence newPrecedence)
	{
		Token operatorToken = EatToken(TokenKind.DotDot);

		Token comparisonToken = Peek();

		if (comparisonToken.IsComparisonOperator())
		{
			Move();
		}
		else
		{
			comparisonToken = default;
		}

		ExpressionSyntax right = ParseExpression(newPrecedence);

		TextSpan span = GetSpan(left, right);

		return new RangeExpressionSyntax(_tree, span, left, operatorToken, comparisonToken, right);
	}

	private ExpressionSyntax ParsePrimaryExpression()
	{
		ref readonly Token token = ref Peek();

		switch (token.Kind)
		{
			case TokenKind.Identifier:
				return ParseSimpleName();

			case TokenKind.Self:
				Move();

				return new InstanceKeywordExpressionSyntax(_tree, token.Span, SyntaxKind.SelfExpression, token);

			case TokenKind.Base:
				Move();

				return new InstanceKeywordExpressionSyntax(_tree, token.Span, SyntaxKind.BaseExpression, token);

			case TokenKind.New:
				return ParseNewExpression();

			//case TokenKind.StackallocKeyword:
			//	return ParseStackallocExpression();

			case TokenKind.False:
			case TokenKind.True:
			case TokenKind.Null:
			case TokenKind.NumericLiteral:
			case TokenKind.StringLiteral:
			//case TokenKind.CharLiteral:
				Move();

				return new LiteralExpressionSyntax(_tree, token.Span, SyntaxFacts.GetLiteralExpressionKind(token.Kind), token);

			//case TokenKind.OpenBracket:
			//	return ParseCollectionExpression();

			case TokenKind.OpenParen:

				//if (TryParseCastExpression() is CastExpressionSyntax castExpr)
				//{
				//	return castExpr;
				//}

				//if (TryParseTupleExpression() is TupleExpressionSyntax tupleExpr)
				//{
				//	return tupleExpr;
				//}

				Token openParen = EatToken();
				ExpressionSyntax expr = ParseExpression();
				Token closeParen = EatToken(TokenKind.CloseParen);

				TextSpan span = GetSpan(openParen, closeParen);

				return new ParenthesizedExpressionSyntax(_tree, span, openParen, expr, closeParen);

			default:
				if (token.IsPredefinedType())
				{
					return new PredefinedTypeSyntax(_tree, token.Span, token);
				}

				return new IdentifierNameSyntax(_tree, token.Span, MissingToken(token));
		}
	}

	private ExpressionSyntax ParsePostfixExpression(ExpressionSyntax left)
	{
		ExpressionSyntax expr = left;

		while (true)
		{
			Token token = Peek();

			switch (token.Kind)
			{
				case TokenKind.OpenParen:
					{
						ArgumentListSyntax argumentList = ParseArgumentList();
						TextSpan span = GetSpan(expr, argumentList);
						expr = new InvocationExpressionSyntax(_tree, span, expr, argumentList);
					}

					break;

				case TokenKind.OpenBracket:
					{
						BracketArgumentListSyntax argumentList = ParseBracketArgumentList();
						TextSpan span = GetSpan(expr, argumentList);
						expr = new ElementAccessExpressionSyntax(_tree, span, expr, argumentList);
					}

					break;

				case TokenKind.PlusPlus:
				case TokenKind.MinusMinus:
					{
						TextSpan span = GetSpan(expr, token);
						expr = new PostfixUnaryExpressionSyntax(_tree, span, SyntaxFacts.GetPostfixUnaryExpressionKind(token.Kind), expr, EatToken());
					}

					break;

				case TokenKind.Dot:
					{
						Move();

						SimpleNameSyntax name = ParseSimpleName();
						TextSpan span = GetSpan(expr, name);
						expr = new MemberAccessExpressionSyntax(_tree, span, SyntaxKind.SimpleMemberAccessExpression, expr, token, name);
					}

					break;

				case TokenKind.MinusGreaterThan:
					{
						Move();

						SimpleNameSyntax name = ParseSimpleName();
						TextSpan span = GetSpan(expr, name);
						expr = new MemberAccessExpressionSyntax(_tree, span, SyntaxKind.PointerMemberAccessExpression, expr, token, name);
					}

					break;

				case TokenKind.Question:
					{
						Move();

						if (PeekKind(TokenKind.Dot))
						{
							Token dot = EatToken();
							SimpleNameSyntax name = ParseSimpleName();
	
							MemberBindingExpressionSyntax right = new(_tree, GetSpan(dot, name), dot, name);
							expr = new ConditionalAccessExpressionSyntax(_tree, GetSpan(expr, right), expr, token, right);
						}
						else if(PeekKind(TokenKind.OpenParen))
						{
							ArgumentListSyntax argumentList = ParseArgumentList();

							InvocationBindingExpressionSyntax right = new(_tree, argumentList.Span, argumentList);
							expr = new ConditionalAccessExpressionSyntax(_tree, GetSpan(expr, right), expr, token, right);
						}
						else if(PeekKind(TokenKind.OpenBracket))
						{
							BracketArgumentListSyntax argumentList = ParseBracketArgumentList();

							ElementBindingExpressionSyntax right = new(_tree, argumentList.Span, argumentList);
							expr = new ConditionalAccessExpressionSyntax(_tree, GetSpan(expr, right), expr, token, right);
						}
					}

					break;

				default:
					return expr;
			}
		}
	}

	private ArgumentListSyntax ParseArgumentList()
	{
		Token openParen = EatToken(TokenKind.OpenParen);

		List<(ArgumentSyntax, Token)> args = new();

		while (true)
		{
			Token token = Peek();

			if (token.IsKind(TokenKind.CloseParen))
			{
				break;
			}

			ArgumentSyntax arg = ParseArgument();

			token = Peek();

			if (token.IsKind(TokenKind.Comma))
			{
				Move();
				args.Add((arg, token));
				continue;
			}

			args.Add((arg, default));
		}

		Token closeParen = EatToken(TokenKind.CloseParen);

		TextSpan span = GetSpan(openParen, closeParen);

		return new(_tree, span, openParen, List(args), closeParen);
	}

	private BracketArgumentListSyntax ParseBracketArgumentList()
	{
		Token openBracket = EatToken(TokenKind.OpenBracket);

		List<(ArgumentSyntax, Token)> args = new();

		while (true)
		{
			Token token = Peek();

			if (token.IsKind(TokenKind.CloseBracket))
			{
				break;
			}

			ArgumentSyntax arg = ParseArgument();

			token = Peek();

			if (token.IsKind(TokenKind.Comma))
			{
				args.Add((arg, token));
				continue;
			}

			args.Add((arg, default));
		}

		Token closeBracket = EatToken(TokenKind.CloseBracket);

		TextSpan span = GetSpan(openBracket, closeBracket);

		return new(_tree, span, openBracket, List(args), closeBracket);
	}

	private ArgumentSyntax ParseArgument()
	{
		ExpressionSyntax expr = ParseExpression();

		TextSpan span = expr.Span;

		return new(_tree, span, expr);
	}

	private ExpressionSyntax ParseNewExpression()
	{
		Token keyword = EatToken(TokenKind.New);

		Token token = Peek();

		switch (token.Kind)
		{
			// Array with no element type.
			case TokenKind.OpenBracket:
				{
					SyntaxList<ArrayRankSyntax> ranks = ParseArrayRanks();

					InitializerExpressionSyntax? initializer = TryParseInitializer();

					TextSpan span = initializer is null
						? GetSpan(keyword, ranks)
						: GetSpan(keyword, initializer);

					return new ArrayCreationExpressionSyntax(_tree, span, keyword, null, ranks, initializer);
				}

			// Object with no type, but with argument list.
			case TokenKind.OpenParen:
				{
					ArgumentListSyntax argumentList = ParseArgumentList();
					InitializerExpressionSyntax? initializer = TryParseInitializer();

					TextSpan span = initializer is null
						? GetSpan(keyword, argumentList)
						: GetSpan(keyword, initializer);

					return new ObjectCreationExpressionSyntax(_tree, span, keyword, null, argumentList, initializer);
				}

			// Anonymous object.
			case TokenKind.OpenBrace:
				{
					InitializerExpressionSyntax? initializer = TryParseInitializer();

					TextSpan span = initializer is null
						? keyword.Span
						: GetSpan(keyword, initializer);

					return new ObjectCreationExpressionSyntax(_tree, span, keyword, null, null, initializer);
				}

			default:
				{
					TypeSyntax type = ParseType();

					token = Peek();

					if (token.IsKind(TokenKind.OpenBracket))
					{
						SyntaxList<ArrayRankSyntax> ranks = ParseArrayRanks();

						InitializerExpressionSyntax? initializer = TryParseInitializer();

						TextSpan span = initializer is null
							? GetSpan(keyword, ranks)
							: GetSpan(keyword, initializer);

						return new ArrayCreationExpressionSyntax(_tree, span, keyword, type, ranks, initializer);
					}
					else
					{
						ArgumentListSyntax argumentList = ParseArgumentList();

						InitializerExpressionSyntax? initializer = TryParseInitializer();

						TextSpan span = initializer is null
							? GetSpan(keyword, argumentList)
							: GetSpan(keyword, initializer);

						return new ObjectCreationExpressionSyntax(_tree, span, keyword, type, argumentList, initializer);
					}
				}
		}
	}

	private InitializerExpressionSyntax? TryParseInitializer()
	{
		Token openBrace = Peek();

		if (!openBrace.IsKind(TokenKind.OpenBrace))
		{
			return null;
		}

		List<(ExpressionSyntax, Token)> expressions = new();

		Token token;

		while ((token = Peek()).IsValid())
		{
			if (token.IsKind(TokenKind.CloseBrace))
			{
				break;
			}

			ExpressionSyntax expression = ParseExpression();

			token = Peek();

			if (token.IsKind(TokenKind.Comma))
			{
				Move();
				expressions.Add((expression, token));
				continue;
			}

			expressions.Add((expression, default));
		}

		Token closeBrace = EatToken(TokenKind.CloseBrace);

		TextSpan span = GetSpan(openBrace, closeBrace);

		return new(_tree, span, openBrace, List(expressions), closeBrace);
	}

	internal DiagnosticBag GetDiagnostics()
	{
		return _diagnostics;
	}

	private ref readonly Token Peek()
	{
		return ref _tokens[_current];
	}

	private ref readonly Token Peek(int dist)
	{
		int next = _current + dist;

		// Return EOF if pos is too big.
		if (next > _tokens.Length - 1)
		{
			return ref _tokens[^1];
		}

		return ref _tokens[_current + dist];
	}

	private TokenKind PeekKind()
	{
		ref readonly Token token = ref Peek();
		return token.Kind;
	}

	private bool PeekKind(TokenKind kind)
	{
		ref readonly Token token = ref Peek();
		return token.Kind == kind;
	}

	private bool PeekKind(TokenKind kind, int dist)
	{
		ref readonly Token token = ref Peek(dist);
		return token.Kind == kind;
	}

	private ref readonly Token EatToken()
	{
		return ref _tokens[_current++];
	}

	private Token EatToken(TokenKind kind)
	{
		ref readonly Token token = ref Peek();

		if (token.Kind != kind)
		{
			return MissingTokenWithError(token);
		}

		Move();

		return token;
	}

	private bool EatToken(TokenKind kind, out Token token)
	{
		ref readonly Token current = ref Peek();

		if (current.Kind != kind)
		{
			token = MissingTokenWithError(current);
			return false;
		}

		EatToken();
		token = current;
		return true;
	}

	private void Move()
	{
		_current++;
	}

	private Token EatContextualKeyword()
	{
		Token token = EatToken();
		AcceptContextualKeyword(ref token);
		return token;
	}

	private Token EatContextualKeyword(TokenKind kind)
	{
		Token token = Peek();

		if (token.Kind != kind)
		{
			return MissingTokenWithError(token);
		}

		AcceptContextualKeyword(ref token);
		return token;
	}

	private static void AcceptContextualKeyword(ref Token token)
	{
		TokenKind kind = token.GetContextualKind();

		if (kind == token.Kind)
		{
			return;
		}

		ChangeKind(ref token, kind);
	}

	private Token AcceptContextualKeyword(TokenKind kind)
	{
		Token token = Peek();
		ChangeKind(ref token, kind);
		return token;
	}

	private static void ChangeKind(ref Token token, TokenKind kind)
	{
		token = new(kind, token.Text, token.Position, token.Value);
	}

	private static Precedence GetPrecedence(SyntaxKind kind)
	{
		return kind switch
		{
			//SyntaxKind.CharLiteralExpression or
			SyntaxKind.FalseLiteralExpression or
			SyntaxKind.TrueLiteralExpression or
			SyntaxKind.StringLiteralExpression or
			SyntaxKind.NumericLiteralExpression or
			SyntaxKind.NullLiteralExpression or
			SyntaxKind.ParenthesizedExpression or
			SyntaxKind.IdentifierName or
			SyntaxKind.GenericName or
			SyntaxKind.PredefinedType or
			SyntaxKind.InvocationExpression or
			SyntaxKind.PostDecrementExpression or
			SyntaxKind.PostIncrementExpression or
			SyntaxKind.SelfExpression or
			SyntaxKind.BaseExpression or
			SyntaxKind.ArrayCreationExpression or
			SyntaxKind.ObjectCreationExpression or
			//SyntaxKind.CollectionExpression or
			SyntaxKind.ElementAccessExpression or
			SyntaxKind.PointerMemberAccessExpression
			//SyntaxKind.TupleExpression or
			//SyntaxKind.StackallocExpression
				=> Precedence.Primary,

			SyntaxKind.AddressOfExpression
				=> Precedence.AddressOf,

			SyntaxKind.PointerIndirectionExpression
				=> Precedence.PointerIndirection,

			//SyntaxKind.CastExpression
			//	=> Precedence.Cast,

			SyntaxKind.UnaryPlusExpression or
			SyntaxKind.UnaryMinusExpression or
			SyntaxKind.BitwiseNotExpression or
			SyntaxKind.LogicalNotExpression or
			SyntaxKind.PreIncrementExpression or
			SyntaxKind.PreDecrementExpression or
			SyntaxKind.AddressOfExpression or
			SyntaxKind.PointerIndirectionExpression
			//SyntaxKind.SpreadExpression
				=> Precedence.Unary,

			SyntaxKind.RangeExpression
				=> Precedence.Range,

			SyntaxKind.MultiplyExpression or
			SyntaxKind.DivideExpression or
			SyntaxKind.ModuloExpression
				=> Precedence.Multiplicative,

			SyntaxKind.AddExpression or
			SyntaxKind.SubtractExpression
				=> Precedence.Additive,

			SyntaxKind.LeftShiftExpression or
			SyntaxKind.RightShiftExpression or
			SyntaxKind.UnsignedRightShiftExpression
				=> Precedence.Shift,

			SyntaxKind.LessThanExpression or
			SyntaxKind.LessThanOrEqualExpression or
			SyntaxKind.GreaterThanExpression or
			SyntaxKind.GreaterThanOrEqualExpression
				=> Precedence.Relational,

			SyntaxKind.EqualsExpression or
			SyntaxKind.NotEqualsExpression or
			SyntaxKind.ReferenceEqualsExpression
				=> Precedence.Equality,

			SyntaxKind.BitwiseAndExpression
				=> Precedence.BitwiseAnd,

			SyntaxKind.ExclusiveOrExpression
				=> Precedence.BitwiseXor,

			SyntaxKind.BitwiseOrExpression
				=> Precedence.BitwiseOr,

			SyntaxKind.LogicalAndExpression
				=> Precedence.ConditionalAnd,

			SyntaxKind.LogicalOrExpression
				=> Precedence.ConditionalOr,

			SyntaxKind.CoalesceExpression
				=> Precedence.NullCoalescing,

			SyntaxKind.AssignmentExpression or
			SyntaxKind.AddAssignmentExpression or
			SyntaxKind.SubtractAssignmentExpression or
			SyntaxKind.MultiplyAssignmentExpression or
			SyntaxKind.DivideAssignmentExpression or
			SyntaxKind.ModuloAssignmentExpression or
			SyntaxKind.AndAssignmentExpression or
			SyntaxKind.ExclusiveOrAssignmentExpression or
			SyntaxKind.OrAssignmentExpression or
			SyntaxKind.LeftShiftAssignmentExpression or
			SyntaxKind.RightShiftAssignmentExpression or
			SyntaxKind.UnsignedRightShiftAssignmentExpression
				=> Precedence.Assignment,

			//SyntaxKind.ConditionalExpression
			//	=> Precedence.Low,

			_ => throw new UnreachableException(),
		};
	}

	private static bool IsRightAssociative(SyntaxKind kind)
	{
		return
			kind == SyntaxKind.AssignmentExpression ||
			kind == SyntaxKind.AddAssignmentExpression ||
			kind == SyntaxKind.SubtractAssignmentExpression ||
			kind == SyntaxKind.MultiplyAssignmentExpression ||
			kind == SyntaxKind.DivideAssignmentExpression ||
			kind == SyntaxKind.ModuloAssignmentExpression ||
			kind == SyntaxKind.AndAssignmentExpression ||
			kind == SyntaxKind.ExclusiveOrAssignmentExpression ||
			kind == SyntaxKind.OrAssignmentExpression ||
			kind == SyntaxKind.LeftShiftAssignmentExpression ||
			kind == SyntaxKind.RightShiftAssignmentExpression ||
			kind == SyntaxKind.UnsignedRightShiftAssignmentExpression ||
			kind == SyntaxKind.CoalesceExpression ||
			kind == SyntaxKind.ConditionalAssignmentExpression;
	}

	private static int CalcMemberPosition(in SyntaxList<AttributeSyntax> attributes, in TokenList modifiers, in Token token)
	{
		if (!attributes.IsDefaultOrEmpty)
		{
			return attributes.Position;
		}

		if (!modifiers.IsDefaultOrEmpty)
		{
			return modifiers.Position;
		}

		return token.Position;
	}

	private static TextSpan GetSpan(int start, in Token end)
	{
		int endPosition = end.Position + end.Length;
		int length = endPosition - start;
		return new(start, length);
	}

	private static TextSpan GetSpan(in Token start, in Token end)
	{
		return GetSpan(start.Position, end);
	}

	private static TextSpan GetSpan(int start, int end)
	{
		int length = end - start + 1;
		return new(start, length);
	}

	private static TextSpan GetSpan(int start, SyntaxNode end)
	{
		return GetSpan(start, end.Span.End);
	}

	private static TextSpan GetSpan(in Token start, SyntaxNode end)
	{
		return GetSpan(start.Position, end.Span.End);
	}

	private static TextSpan GetSpan(SyntaxNode start, in Token end)
	{
		return GetSpan(start.Position, end.Span.End);
	}

	private static TextSpan GetSpan(SyntaxNode start, SyntaxNode end)
	{
		return GetSpan(start.Span.Start, end.Span.End);
	}

	private static TextSpan GetSpan<TNode>(in Token start, SyntaxList<TNode> end) where TNode : SyntaxNode
	{
		return end.IsDefaultOrEmpty
			? start.Span
			: GetSpan(start, end[^1]);
	}

	private static TextSpan GetSpan<TNode>(SyntaxNode start, SyntaxList<TNode> end) where TNode : SyntaxNode
	{
		return end.IsDefaultOrEmpty
			? start.Span
			: GetSpan(start, end[^1]);
	}

	private static TextSpan GetSpan<TNode>(in Token start, List<TNode>? end) where TNode : SyntaxNode
	{
		return end is null || end.Count == 0
			? start.Span
			: GetSpan(start, end[^1]);
	}

	private static TextSpan GetSpan<TNode>(SyntaxNode start, List<TNode>? end) where TNode : SyntaxNode
	{
		return end is null || end.Count == 0
			? start.Span
			: GetSpan(start, end[^1]);
	}

	private static TextSpan GetSpan<TNode>(in Token start, List<(TNode node, Token)>? end) where TNode : SyntaxNode
	{
		return end is null || end.Count == 0
			? start.Span
			: GetSpan(start, end[^1].node);
	}

	private static TextSpan GetSpan<TNode>(SyntaxNode start, List<(TNode node, Token)>? end) where TNode : SyntaxNode
	{
		return end is null || end.Count == 0
			? start.Span
			: GetSpan(start, end[^1].node);
	}

	private static SyntaxList<TNode> List<TNode>(List<TNode> nodes) where TNode : SyntaxNode
	{
		return new(nodes.ToArray());
	}

	private static SyntaxList<TNode> ListIfNotNull<TNode>(List<TNode>? nodes) where TNode : SyntaxNode
	{
		if (nodes is null)
		{
			return default;
		}

		return List(nodes);
	}

	private static SyntaxList<TNode> List<TNode>(TNode[] nodes) where TNode : SyntaxNode
	{
		return new(nodes);
	}

	private static TokenList List(List<Token> tokens)
	{
		return new(tokens.ToArray());
	}

	private static SeparatedSyntaxList<TNode> List<TNode>(List<(TNode node, Token seprator)> nodes) where TNode : SyntaxNode
	{
		return new(nodes.ToArray());
	}

	private Token MissingTokenWithError(in Token token)
	{
		if (token.Kind == TokenKind.Eof)
		{
			AddError(ErrorCode.ERR_UnexpectedEndOfFile);
		}
		else
		{
			AddError(ErrorCode.ERR_SyntaxError);
		}

		return MissingToken(token);
	}

	private static Token MissingToken(in Token current)
	{
		return new(TokenKind.MissingToken, string.Empty, current.Position);
	}

	private Token UnexpectedToken()
	{
		ref readonly Token current = ref EatToken();
		return UnexpectedToken(in current);
	}

	private static Token UnexpectedToken(in Token token)
	{
		return new(TokenKind.BadToken, token.Text, token.Position);
	}

	private Token SkippedToken()
	{
		return SkippedToken(Peek());
	}

	private static Token SkippedToken(in Token token)
	{
		return new(TokenKind.SkippedToken, string.Empty, token.Position);
	}

	private void AddError(ErrorCode code)
	{
		int position = _tokens[_current].Position;
		AddError(code, position);
	}

	private void AddError(ErrorCode code, int position)
	{
		_diagnostics.Push(code, _tree.GetLocation(position));
	}

	private enum Precedence : uint
	{
		Low = 0,

		// a = b, a += b, a >>= b etc.
		Assignment,

		// a ? b : c
		Conditional,

		// a ?? b
		NullCoalescing,

		// a || b
		ConditionalOr,

		// a && b
		ConditionalAnd,

		// a | b
		BitwiseOr,

		// a ^ b
		BitwiseXor,

		// a & b
		BitwiseAnd,

		// a == b, a != b
		Equality,

		// a < b, a > b, etc.
		Relational,

		// a << b, a >>> b etc.
		Shift,

		// a + b, a - b
		Additive,

		// a * b, a / b
		Multiplicative,

		// match (a)
		Match,

		// a..b
		Range,

		// ++a, -a, !a, true/false etc.
		Unary,

		// (int) a
		Cast,

		// *a
		PointerIndirection,

		// &a
		AddressOf,

		// a.b, a++, etc.
		Primary
	}
}
