using Zubr.Collections;
using Zubr.Diagnostics;
using Zubr.Text;

namespace Zubr.Declarations;

internal sealed class SingleFunctionDeclaration : SingleDeclaration
{
	private readonly ushort _arity;

	public int Arity => _arity;

	public DeclarationModifiers Modifiers { get; }

	internal SingleFunctionDeclaration(
		string name,
		int arity,
		DeclarationModifiers modifiers,
		SyntaxRef @ref,
		Location nameLocation,
		ConstArray<Diagnostic> diagnostics
	) : base(DeclarationKind.Function, name, @ref, nameLocation, diagnostics)
	{
		_arity = (ushort)arity;
		Modifiers = modifiers;
	}

	protected override ConstArray<SingleDeclaration> GetChildren()
	{
		return [];
	}
}
