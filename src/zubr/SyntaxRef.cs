using System.Threading;
using Zubr.Text;

namespace Zubr;

public abstract class SyntaxRef
{
	public abstract SyntaxTree SyntaxTree { get; }

	public abstract TextSpan Span { get; }

	public Location Location => SyntaxTree.GetLocation(Span);

	protected SyntaxRef()
	{
	}

	public abstract SyntaxNode GetSyntax(CancellationToken ct = default);
}
