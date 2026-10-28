using System.Diagnostics;
using Zubr.Collections;
using Zubr.Diagnostics;
using Zubr.Syntax;

namespace Zubr.Declarations;

internal sealed class DeclarationTreeBuilder : SyntaxVisitor<SingleDeclaration>
{
	private readonly SyntaxTree _tree;

	private DeclarationTreeBuilder(SyntaxTree tree)
	{
		_tree = tree;
	}

	public static RootSingleModuleDeclaration Build(SyntaxTree tree)
	{
		DeclarationTreeBuilder builder = new(tree);
		return (RootSingleModuleDeclaration)builder.Visit(tree.GetRoot())!;
	}

	public override SingleDeclaration? VisitCompilationUnit(CompilationUnitSyntax node)
	{
		ConstArray<SingleDeclaration> children = GetModuleChildren(node.Members);
		return CreateRoot(node, children);
	}

	public override SingleDeclaration? VisitModuleDeclaration(ModuleDeclarationSyntax node)
	{
		DiagnosticBag diagnostics = new();

		ConstArray<SingleDeclaration> children = GetModuleChildren(node.Members);

		// module top;
		if (node.Name is null)
		{
			return new SingleModuleDeclaration(
				SyntaxFacts.GetText(TokenKind.Top),
				node.GetReference(),
				_tree.GetLocation(node.Top),
				true,
				children,
				diagnostics.ToConstArray()
			);
		}

		NameSyntax name = node.Name;
		SyntaxNode current = node;

		// Retrieve the right-most name as the current module.
		while (name is QualifiedNameSyntax qualified)
		{
			SingleModuleDeclaration declaration = new(
				qualified.Right.Identifier.Text,
				current.GetReference(),
				_tree.GetLocation(qualified.Right),
				false,
				children,
				diagnostics.ToConstArray()
			);

			children = [declaration];
			name = qualified.Left;
			current = name;
		}

		// Module names cannot be generic.
		if (ContainsGeneric(node.Name))
		{
			diagnostics.Push(ErrorCode.ERR_UnexpectedGenericName, node.Name.Location);
		}

		// Aliases are not allowed in module declarations.
		if (ContainsAlias(node.Name))
		{
			diagnostics.Push(ErrorCode.ERR_UnexpectedAliasedName, node.Name.Location);
		}

		// TODO: Handle module attributes.

		return new SingleModuleDeclaration(
			name.GetUnqualifiedName().Identifier.Text,
			current.GetReference(),
			_tree.GetLocation(name),
			false,
			children,
			diagnostics.ToConstArray()
		);
	}

	public override SingleDeclaration? VisitClassDeclaration(ClassDeclarationSyntax node)
	{
		return VisitTypeDeclaration(node, DeclarationKind.Class);
	}

	public override SingleDeclaration? VisitStructDeclaration(StructDeclarationSyntax node)
	{
		return VisitTypeDeclaration(node, DeclarationKind.Struct);
	}

	public override SingleDeclaration? VisitTraitDeclaration(TraitDeclarationSyntax node)
	{
		return VisitTypeDeclaration(node, DeclarationKind.Trait);
	}

	public override SingleDeclaration? VisitEnumDeclaration(EnumDeclarationSyntax node)
	{
		DiagnosticBag diagnostics = new();

		DeclarationModifiers modifiers = GetModifiers(node.Modifiers, diagnostics);

		return new SingleTypeDeclaration(
			DeclarationKind.Enum,
			node.Identifier.Text,
			0,
			modifiers,
			node.GetReference(),
			_tree.GetLocation(node.Identifier),
			[],
			diagnostics.ToConstArray()
		);
	}

	public override SingleDeclaration? VisitAliasDeclaration(TypeAliasDeclarationSyntax node)
	{
		DiagnosticBag diagnostics = new();

		DeclarationModifiers modifiers = GetModifiers(node.Modifiers, diagnostics);

		return new SingleTypeDeclaration(
			DeclarationKind.TypeAlias,
			node.Alias.Text,
			0,
			modifiers,
			node.GetReference(),
			_tree.GetLocation(node.Alias),
			[],
			diagnostics.ToConstArray()
		);
	}

	public override SingleDeclaration? VisitFunctionDeclaration(FunctionDeclarationSyntax node)
	{
		DiagnosticBag diagnostics = new();

		if (node.Arity == 0)
		{
			diagnostics.Push(ErrorCode.ERR_NonGenericConstraints, _tree.GetLocation(node.ConstraintList[0].Where));
		}

		DeclarationModifiers modifiers = GetModifiers(node.Modifiers, diagnostics);

		return new SingleFunctionDeclaration(
			node.Identifier.Text,
			node.Arity,
			modifiers,
			node.GetReference(),
			_tree.GetLocation(node.Identifier),
			diagnostics.ToConstArray()
		);
	}

	private SingleTypeDeclaration VisitTypeDeclaration(TypeDeclarationSyntax node, DeclarationKind kind)
	{
		DiagnosticBag diagnostics = new();

		if(node.Arity == 0)
		{
			diagnostics.Push(ErrorCode.ERR_NonGenericConstraints, _tree.GetLocation(node.ConstraintList[0].Where));
		}

		DeclarationModifiers modifiers = GetModifiers(node.Modifiers, diagnostics);
		ConstArray<SingleTypeDeclaration> children = GetTypeChildren(node.Members);

		return new SingleTypeDeclaration(
			kind,
			node.Identifier.Text,
			node.Arity,
			modifiers,
			node.GetReference(),
			_tree.GetLocation(node.Identifier),
			children,
			diagnostics.ToConstArray()
		);
	}

	// Only inner types are retrieved in the declaration stage.
	private ConstArray<SingleTypeDeclaration> GetTypeChildren(SyntaxList<MemberDeclarationSyntax> members)
	{
		if (members.IsDefaultOrEmpty)
		{
			return [];
		}

		ArrayBuilder<SingleTypeDeclaration> children = new();

		foreach (MemberDeclarationSyntax member in members)
		{
			// Type methods are not visited at this stage.
			if(member is FunctionDeclarationSyntax)
			{
				continue;
			}

			SingleTypeDeclaration? declaration = Visit(member) as SingleTypeDeclaration;

			if (declaration is not null)
			{
				children.Add(declaration);
			}
		}

		return children.ToConstArray();
	}

	private ConstArray<SingleDeclaration> GetModuleChildren(SyntaxList<MemberDeclarationSyntax> members)
	{
		if(members.IsDefaultOrEmpty)
		{
			return [];
		}

		ArrayBuilder<SingleDeclaration> children = new(members.Count);

		foreach (MemberDeclarationSyntax member in members)
		{
			SingleDeclaration? declaration = Visit(member);

			if(declaration is not null)
			{
				children.Add(declaration);
			}
		}

		return children.ToConstArray();
	}

	private static RootSingleModuleDeclaration CreateRoot(CompilationUnitSyntax root, ConstArray<SingleDeclaration> children)
	{
		bool hasUses = false;
		bool hasTopUses = false;
		bool reportedTopOrder = false;

		DiagnosticBag diagnostics = new();

		foreach (UseDirectiveSyntax directive in root.Uses)
		{
			if(directive.Top.Exists)
			{
				hasTopUses = true;

				// Top uses must be specified before regular uses.
				if(hasUses && !reportedTopOrder)
				{
					reportedTopOrder = true;
					diagnostics.Push(ErrorCode.ERR_TopUsesOutOfOrder, root.SyntaxTree.GetLocation(directive.Top));
				}
			}
			else
			{
				hasUses = true;
			}
		}

		return new(
			SyntaxFacts.GetText(TokenKind.Top),
			root.GetReference(),
			hasUses,
			hasTopUses,
			children,
			diagnostics.ToConstArray()
		);
	}

	private DeclarationModifiers GetModifiers(TokenList list, DiagnosticBag diagnostics)
	{
		DeclarationModifiers all = DeclarationModifiers.None;

		bool hasDuplicates = false;

		foreach (Token token in list)
		{
			DeclarationModifiers modifier = ToModifier(token.Kind);

			if((all & modifier) != 0 && !hasDuplicates)
			{
				diagnostics.Push(ErrorCode.ERR_DuplicateModifier, _tree.GetLocation(token));
				hasDuplicates = true;
			}

			all |= modifier;
		}

		return all;
	}

	private static DeclarationModifiers ToModifier(TokenKind kind)
	{
		return kind switch
		{
			TokenKind.Priv => DeclarationModifiers.Private,
			TokenKind.Prot => DeclarationModifiers.Protected,
			TokenKind.Pub => DeclarationModifiers.Public,
			TokenKind.File => DeclarationModifiers.File,
			TokenKind.Rest => DeclarationModifiers.Restricted,
			TokenKind.Over => DeclarationModifiers.Override,
			TokenKind.Limit => DeclarationModifiers.Limit,
			TokenKind.Base => DeclarationModifiers.Base,
			TokenKind.Open => DeclarationModifiers.Open,
			TokenKind.Final => DeclarationModifiers.Final,
			TokenKind.Data => DeclarationModifiers.Data,
			TokenKind.Const => DeclarationModifiers.Const,
			TokenKind.Stat => DeclarationModifiers.Static,
			TokenKind.Mut => DeclarationModifiers.Mutable,
			TokenKind.Init => DeclarationModifiers.Init,
			TokenKind.Extern => DeclarationModifiers.Extern,

			_ => throw new UnreachableException()
		};
	}

	private static bool ContainsAlias(NameSyntax name)
	{
		switch (name.Kind)
		{
			case SyntaxKind.GenericName:
				return false;

			case SyntaxKind.AliasQualifiedName:
			case SyntaxKind.TopQualifiedName:
				return true;

			case SyntaxKind.QualifiedName:
				QualifiedNameSyntax qualifiedName = (QualifiedNameSyntax)name;
				return ContainsAlias(qualifiedName.Left);
		}

		return false;
	}

	private static bool ContainsGeneric(NameSyntax name)
	{
		switch (name.Kind)
		{
			case SyntaxKind.GenericName:
				return true;

			case SyntaxKind.AliasQualifiedName:
				return ContainsGeneric(((AliasQualifiedNameSyntax)name).Alias);

			case SyntaxKind.TopQualifiedName:
				return ContainsGeneric(((TopQualifiedNameSyntax)name).Name);

			case SyntaxKind.QualifiedName:
				QualifiedNameSyntax qualifiedName = (QualifiedNameSyntax)name;
				return ContainsGeneric(qualifiedName.Left) || ContainsGeneric(qualifiedName.Right);
		}

		return false;
	}
}
