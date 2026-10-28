using Zubr.Syntax;

namespace Zubr;

public abstract class SyntaxVisitor<TResult>
{
	public virtual TResult? VisitAccessorDeclaration(AccessorDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitAccessorList(AccessorListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitAliasDeclaration(TypeAliasDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitAliasSpecifier(AliasSpecifierSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitArgument(ArgumentSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitArgumentList(ArgumentListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitArrayCreationExpression(ArrayCreationExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitArrayRank(ArrayRankSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitArrayType(ArrayTypeSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitArrowExpressionClause(ArrowExpressionClauseSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitAssignmentExpression(AssignmentExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitAssociatedTypeDeclaration(AssociatedTypeDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitAttribute(AttributeSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitAttributeArgument(AttributeArgumentSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitAttributeArgumentList(AttributeArgumentListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitAttributeTarget(AttributeTargetSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitBaseTypeList(BaseTypeListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitBinaryExpression(BinaryExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitBlock(BlockSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitBracketArgumentList(BracketArgumentListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitBracketParameterList(BracketParameterListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitCastDeclaration(CastDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitClassDeclaration(ClassDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitCompilationUnit(CompilationUnitSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitConstructorDeclaration(ConstructorDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitConstructorInvocationExpression(ConstructorInvocationExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitDestructorDeclaration(DestructorDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitDiscardExpression(DiscardExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitDoWhileStatement(DoWhileStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitElementAccessExpression(ElementAccessExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitElementBindingExpression(ElementBindingExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitElifClause(ElifClauseSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitElseClause(ElseClauseSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitEmptyStatement(EmptyStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitEnumDeclaration(EnumDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitEqualsTypeClause(EqualsTypeClauseSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitEqualsValueClause(EqualsValueClauseSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitExpressionStatement(ExpressionStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitForStatement(ForStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitFunctionDeclaration(FunctionDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitGenericName(GenericNameSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitIdentifierName(IdentifierNameSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitIfStatement(IfStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitIndexerDeclaration(IndexerDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitInitializerExpression(InitializerExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitInstanceKeywordExpression(InstanceKeywordExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitInvocationBindingExpression(InvocationBindingExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitInvocationExpression(InvocationExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitInvokerDeclaration(InvokerDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitJumpStatement(JumpStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitKeywordConstraint(KeywordConstraintSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitLabelStatement(LabelStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitListedUseDirective(ListedUseDirectiveSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitLiteralExpression(LiteralExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitMatchExpression(MatchExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitMatchExpressionBranch(MatchExpressionBranchSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitMatchStatement(MatchStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitMemberAccessExpression(MemberAccessExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitMemberBindingExpression(MemberBindingExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitModuleDeclaration(ModuleDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitAliasQualifiedName(AliasQualifiedNameSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitNextStatement(NextStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitNullableType(NullableTypeSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitObjectCreationExpression(ObjectCreationExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitOperatorDeclaration(OperatorDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitParameter(ParameterSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitParameterList(ParameterListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitParenthesizedExpression(ParenthesizedExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitPointerType(PointerTypeSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitPredefinedType(PredefinedTypeSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitPrimaryBaseType(PrimaryBaseTypeSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitPropertyDeclaration(PropertyDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitQualifiedName(QualifiedNameSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitRangeExpression(RangeExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitReferenceType(ReferenceTypeSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitReturnStatement(ReturnStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitSimpleBaseType(SimpleBaseTypeSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitSimpleUseDirective(SimpleUseDirectiveSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitSkippedArraySizeExpression(SkippedArraySizeExpressionSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitSkippedTypeArgument(SkippedTypeArgumentSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitStopStatement(StopStatementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitStructDeclaration(StructDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitTopQualifiedName(TopQualifiedNameSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitTraitDeclaration(TraitDeclarationSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitTypeArgumentList(TypeArgumentListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitTypeConstraint(TypeConstraintSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitTypeParameter(TypeParameterSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitTypeParameterConstraintList(TypeParameterConstraintListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitTypeParameterInlineConstraintList(TypeParameterInlineConstraintListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitTypeParameterList(TypeParameterListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitTypeSpecifier(TypeSpecifierSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitUseDirectiveElement(UseDirectiveElementSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitUseDirectiveElementList(UseDirectiveElementListSyntax node) => DefaultVisit(node);

	public virtual TResult? VisitWhileStatement(WhileStatementSyntax node) => DefaultVisit(node);


	public virtual TResult? Visit(SyntaxNode? node)
	{
		if (node is not null)
		{
			return node.Accept(this);
		}

		return default;
	}

	protected virtual TResult? DefaultVisit(SyntaxNode node)
	{
		return default;
	}
}

public abstract class SyntaxVisitor
{
	public virtual void VisitAccessorDeclaration(AccessorDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitAccessorList(AccessorListSyntax node) => DefaultVisit(node);

	public virtual void VisitAliasDeclaration(TypeAliasDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitAliasSpecifier(AliasSpecifierSyntax node) => DefaultVisit(node);

	public virtual void VisitArgument(ArgumentSyntax node) => DefaultVisit(node);

	public virtual void VisitArgumentList(ArgumentListSyntax node) => DefaultVisit(node);

	public virtual void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitArrayRank(ArrayRankSyntax node) => DefaultVisit(node);

	public virtual void VisitArrayType(ArrayTypeSyntax node) => DefaultVisit(node);

	public virtual void VisitArrowExpressionClause(ArrowExpressionClauseSyntax node) => DefaultVisit(node);

	public virtual void VisitAssignmentExpression(AssignmentExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitAssociatedTypeDeclaration(AssociatedTypeDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitAttribute(AttributeSyntax node) => DefaultVisit(node);

	public virtual void VisitAttributeArgument(AttributeArgumentSyntax node) => DefaultVisit(node);

	public virtual void VisitAttributeArgumentList(AttributeArgumentListSyntax node) => DefaultVisit(node);

	public virtual void VisitAttributeTarget(AttributeTargetSyntax node) => DefaultVisit(node);

	public virtual void VisitBaseTypeList(BaseTypeListSyntax node) => DefaultVisit(node);

	public virtual void VisitBinaryExpression(BinaryExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitBlock(BlockSyntax node) => DefaultVisit(node);

	public virtual void VisitBracketArgumentList(BracketArgumentListSyntax node) => DefaultVisit(node);

	public virtual void VisitBracketParameterList(BracketParameterListSyntax node) => DefaultVisit(node);

	public virtual void VisitCastDeclaration(CastDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitClassDeclaration(ClassDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitCompilationUnit(CompilationUnitSyntax node) => DefaultVisit(node);

	public virtual void VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitConstructorDeclaration(ConstructorDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitConstructorInvocationExpression(ConstructorInvocationExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitDestructorDeclaration(DestructorDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitDiscardExpression(DiscardExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitDoWhileStatement(DoWhileStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitElementAccessExpression(ElementAccessExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitElementBindingExpression(ElementBindingExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitElifClause(ElifClauseSyntax node) => DefaultVisit(node);

	public virtual void VisitElseClause(ElseClauseSyntax node) => DefaultVisit(node);

	public virtual void VisitEmptyStatement(EmptyStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitEnumDeclaration(EnumDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitEqualsTypeClause(EqualsTypeClauseSyntax node) => DefaultVisit(node);

	public virtual void VisitEqualsValueClause(EqualsValueClauseSyntax node) => DefaultVisit(node);

	public virtual void VisitExpressionStatement(ExpressionStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitForStatement(ForStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitFunctionDeclaration(FunctionDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitGenericName(GenericNameSyntax node) => DefaultVisit(node);

	public virtual void VisitIdentifierName(IdentifierNameSyntax node) => DefaultVisit(node);

	public virtual void VisitIfStatement(IfStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitIndexerDeclaration(IndexerDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitInitializerExpression(InitializerExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitInstanceKeywordExpression(InstanceKeywordExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitInvocationBindingExpression(InvocationBindingExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitInvocationExpression(InvocationExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitInvokerDeclaration(InvokerDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitJumpStatement(JumpStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitKeywordConstraint(KeywordConstraintSyntax node) => DefaultVisit(node);

	public virtual void VisitLabelStatement(LabelStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitListedUseDirective(ListedUseDirectiveSyntax node) => DefaultVisit(node);

	public virtual void VisitLiteralExpression(LiteralExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitMatchExpression(MatchExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitMatchExpressionBranch(MatchExpressionBranchSyntax node) => DefaultVisit(node);

	public virtual void VisitMatchStatement(MatchStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitMemberAccessExpression(MemberAccessExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitMemberBindingExpression(MemberBindingExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitModuleDeclaration(ModuleDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitAliasQualifiedName(AliasQualifiedNameSyntax node) => DefaultVisit(node);

	public virtual void VisitNextStatement(NextStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitNullableType(NullableTypeSyntax node) => DefaultVisit(node);

	public virtual void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitOperatorDeclaration(OperatorDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitParameter(ParameterSyntax node) => DefaultVisit(node);

	public virtual void VisitParameterList(ParameterListSyntax node) => DefaultVisit(node);

	public virtual void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitPointerType(PointerTypeSyntax node) => DefaultVisit(node);

	public virtual void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitPredefinedType(PredefinedTypeSyntax node) => DefaultVisit(node);

	public virtual void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitPrimaryBaseType(PrimaryBaseTypeSyntax node) => DefaultVisit(node);

	public virtual void VisitPropertyDeclaration(PropertyDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitQualifiedName(QualifiedNameSyntax node) => DefaultVisit(node);

	public virtual void VisitRangeExpression(RangeExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitReferenceType(ReferenceTypeSyntax node) => DefaultVisit(node);

	public virtual void VisitReturnStatement(ReturnStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitSimpleBaseType(SimpleBaseTypeSyntax node) => DefaultVisit(node);

	public virtual void VisitSimpleUseDirective(SimpleUseDirectiveSyntax node) => DefaultVisit(node);

	public virtual void VisitSkippedArraySizeExpression(SkippedArraySizeExpressionSyntax node) => DefaultVisit(node);

	public virtual void VisitSkippedTypeArgument(SkippedTypeArgumentSyntax node) => DefaultVisit(node);

	public virtual void VisitStopStatement(StopStatementSyntax node) => DefaultVisit(node);

	public virtual void VisitStructDeclaration(StructDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitTopQualifiedName(TopQualifiedNameSyntax node) => DefaultVisit(node);

	public virtual void VisitTraitDeclaration(TraitDeclarationSyntax node) => DefaultVisit(node);

	public virtual void VisitTypeArgumentList(TypeArgumentListSyntax node) => DefaultVisit(node);

	public virtual void VisitTypeConstraint(TypeConstraintSyntax node) => DefaultVisit(node);

	public virtual void VisitTypeParameter(TypeParameterSyntax node) => DefaultVisit(node);

	public virtual void VisitTypeParameterConstraintList(TypeParameterConstraintListSyntax node) => DefaultVisit(node);

	public virtual void VisitTypeParameterInlineConstraintList(TypeParameterInlineConstraintListSyntax node) => DefaultVisit(node);

	public virtual void VisitTypeParameterList(TypeParameterListSyntax node) => DefaultVisit(node);

	public virtual void VisitTypeSpecifier(TypeSpecifierSyntax node) => DefaultVisit(node);

	public virtual void VisitUseDirectiveElement(UseDirectiveElementSyntax node) => DefaultVisit(node);

	public virtual void VisitUseDirectiveElementList(UseDirectiveElementListSyntax node) => DefaultVisit(node);

	public virtual void VisitWhileStatement(WhileStatementSyntax node) => DefaultVisit(node);

	public virtual void Visit(SyntaxNode? node)
	{
		node?.Accept(this);
	}

	protected virtual void DefaultVisit(SyntaxNode node)
	{
	}
}
