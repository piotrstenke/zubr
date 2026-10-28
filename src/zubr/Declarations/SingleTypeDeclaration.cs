using Zubr.Collections;
using Zubr.Diagnostics;
using Zubr.Text;

namespace Zubr.Declarations;

internal sealed class SingleTypeDeclaration : SingleDeclaration
{
	private readonly ConstArray<SingleTypeDeclaration> _children;

	private readonly ushort _arity;

	public int Arity => _arity;

	public DeclarationModifiers Modifiers { get; }

	internal SingleTypeDeclaration(
		DeclarationKind kind,
		string name,
		int arity,
		DeclarationModifiers modifiers,
		SyntaxRef @ref,
		Location nameLocation,
		ConstArray<SingleTypeDeclaration> children,
		ConstArray<Diagnostic> diagnostics
	) : base(kind, name, @ref, nameLocation, diagnostics)
	{
		_children = children;
		_arity = (ushort)arity;
		Modifiers = modifiers;
	}

	protected override ConstArray<SingleDeclaration> GetChildren()
	{
		return ConstArray<SingleDeclaration>.CastUp(_children);
	}
}
