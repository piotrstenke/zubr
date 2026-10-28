using System.Threading;
using Zubr.Text;

namespace Zubr;

internal sealed class BasicSyntaxRef : SyntaxRef
{
	private readonly SyntaxNode _node;

	public override SyntaxTree SyntaxTree => _node.SyntaxTree;

	public override TextSpan Span => _node.Span;

	internal BasicSyntaxRef(SyntaxNode node)
	{
		_node = node;
	}

	public override SyntaxNode GetSyntax(CancellationToken ct = default)
	{
		return _node;
	}
}
