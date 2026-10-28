using System.Collections.Immutable;
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
		Location nameLocation
	) : base(DeclarationKind.Function, name, @ref, nameLocation)
	{
		_arity = (ushort)arity;
		Modifiers = modifiers;
	}

	protected override ImmutableArray<SingleDeclaration> GetChildren()
	{
		return [];
	}
}
