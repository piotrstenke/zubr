using System.Diagnostics;
using System.Runtime.CompilerServices;
using Zubr.Syntax;
using Zubr.Text;

namespace Zubr;

[DebuggerDisplay("{ToString(),nq}")]
public abstract class SyntaxNode
{
	public SyntaxNode? Parent { get; private set; }

	public SyntaxTree SyntaxTree { get; }

	public int Position => Span.Start;

	public TextSpan Span { get; }

	public Location Location => SyntaxTree.GetLocation(Span);

	public SyntaxKind Kind { get; }

	internal SyntaxNode(SyntaxTree tree, TextSpan span, SyntaxKind kind)
	{
		SyntaxTree = tree;
		Span = span;
		Kind = kind;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsKind(SyntaxKind kind)
	{
		return Kind == kind;
	}

	public SyntaxRef GetReference()
	{
		return new BasicSyntaxRef(this);
	}

	public abstract override string ToString();

	public abstract void Accept(SyntaxVisitor visitor);

	public abstract TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor);

	private protected void SetParent<TNode>(SyntaxList<TNode> list) where TNode : SyntaxNode
	{
		if (list.IsDefaultOrEmpty)
		{
			return;
		}

		foreach (TNode node in list)
		{
			node.Parent = this;
		}
	}

	private protected void SetParent<TNode>(SeparatedSyntaxList<TNode> list) where TNode : SyntaxNode
	{
		if (list.IsDefaultOrEmpty)
		{
			return;
		}

		foreach (TNode node in list)
		{
			node.Parent = this;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected void SetParent(SyntaxNode node)
	{
		node.Parent = this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected void SetParentIfNotNull(SyntaxNode? node)
	{
		node?.Parent = this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static string TrailingSpace(in Token token)
	{
		if(token.IsNone)
		{
			return string.Empty;
		}

		return $"{token} ";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static string TrailingSpace(TokenList tokens)
	{
		if (tokens.IsDefaultOrEmpty)
		{
			return string.Empty;
		}

		return $"{tokens} ";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static string TrailingSpace<T>(SyntaxList<T> nodes) where T : SyntaxNode
	{
		if (nodes.IsDefaultOrEmpty)
		{
			return string.Empty;
		}

		return $"{nodes} ";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static string TrailingSpace(SyntaxNode? node)
	{
		if (node is null)
		{
			return string.Empty;
		}

		return $"{node} ";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static string LeadingSpace(in Token token)
	{
		if (token.IsNone)
		{
			return string.Empty;
		}

		return $" {token}";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static string LeadingSpace(TokenList tokens)
	{
		if (tokens.IsDefaultOrEmpty)
		{
			return string.Empty;
		}

		return $" {tokens}";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static string LeadingSpace<T>(SyntaxList<T> nodes) where T : SyntaxNode
	{
		if (nodes.IsDefaultOrEmpty)
		{
			return string.Empty;
		}

		return $" {nodes}";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private protected static string LeadingSpace(SyntaxNode? node)
	{
		if (node is null)
		{
			return string.Empty;
		}

		return $" {node}";
	}
}
