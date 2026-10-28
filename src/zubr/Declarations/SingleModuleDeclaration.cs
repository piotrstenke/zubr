using System.Collections.Immutable;
using Zubr.Text;

namespace Zubr.Declarations;

internal class SingleModuleDeclaration : SingleDeclaration
{
	private readonly ImmutableArray<SingleDeclaration> _children;

	public bool IsTop { get; }

	protected SingleModuleDeclaration(
 		string name,
		SyntaxRef @ref,
		Location nameLocation,
		ImmutableArray<SingleDeclaration> children
	) : this(name, @ref, nameLocation, false, children)
	{
	}

	internal SingleModuleDeclaration(
		string name,
		SyntaxRef @ref,
		Location nameLocation,
		bool isTop,
		ImmutableArray<SingleDeclaration> children
	) : base(DeclarationKind.Module, name, @ref, nameLocation)
	{
		_children = children;

		IsTop = isTop;
	}

	protected sealed override ImmutableArray<SingleDeclaration> GetChildren()
	{
		return _children;
	}
}
