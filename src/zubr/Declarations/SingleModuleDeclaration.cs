using Zubr.Collections;
using Zubr.Diagnostics;
using Zubr.Text;

namespace Zubr.Declarations;

internal class SingleModuleDeclaration : SingleDeclaration
{
	private readonly ConstArray<SingleDeclaration> _children;

	public bool IsTop { get; }

	protected SingleModuleDeclaration(
 		string name,
		SyntaxRef @ref,
		Location nameLocation,
		ConstArray<SingleDeclaration> children,
		ConstArray<Diagnostic> diagnostics
	) : this(name, @ref, nameLocation, false, children, diagnostics)
	{
	}

	internal SingleModuleDeclaration(
		string name,
		SyntaxRef @ref,
		Location nameLocation,
		bool isTop,
		ConstArray<SingleDeclaration> children,
		ConstArray<Diagnostic> diagnostics
	) : base(DeclarationKind.Module, name, @ref, nameLocation, diagnostics)
	{
		_children = children;

		IsTop = isTop;
	}

	protected sealed override ConstArray<SingleDeclaration> GetChildren()
	{
		return _children;
	}
}
