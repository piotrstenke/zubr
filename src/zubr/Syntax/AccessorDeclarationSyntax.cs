using System;
using Zubr.Text;

namespace Zubr.Syntax;

public sealed class AccessorDeclarationSyntax : SyntaxNode
{
	public SyntaxList<AttributeSyntax> Attributes { get; }

	public TokenList Modifiers { get; }

	public Token Keyword { get; }

	public BlockSyntax? Block { get; }

	public ArrowExpressionClauseSyntax? ExpressionBody { get; }

	public Token Semicolon { get; }

	internal AccessorDeclarationSyntax(
		SyntaxTree tree,
		TextSpan span,
		SyntaxKind kind,
		SyntaxList<AttributeSyntax> attributes,
		TokenList modifiers,
		Token keyword,
		BlockSyntax? block,
		ArrowExpressionClauseSyntax? expressionBody,
		Token semicolon
	) : base(tree, span, kind)
	{
		Attributes = attributes;
		Modifiers = modifiers;
		Keyword = keyword;
		Block = block;
		ExpressionBody = expressionBody;
		Semicolon = semicolon;

		SetParent(attributes);
		SetParentIfNotNull(block);
		SetParentIfNotNull(expressionBody);
	}

	public override string ToString()
	{
		if (Block is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword} {Block}";
		}

		if (ExpressionBody is not null)
		{
			return $"{TrailingSpace(Modifiers)}{Keyword} {ExpressionBody}{Semicolon}";
		}

		return $"{TrailingSpace(Modifiers)}{Keyword}{Semicolon}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitAccessorDeclaration(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitAccessorDeclaration(this);
}
