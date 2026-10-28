using System.Collections.Immutable;
using Zubr.Text;

namespace Zubr.Declarations;

internal sealed class SingleTypeDeclaration : SingleDeclaration
{
	private readonly ImmutableArray<SingleTypeDeclaration> _children;

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
		ImmutableArray<SingleTypeDeclaration> children
	) : base(kind, name, @ref, nameLocation)
	{
		_children = children;
		_arity = (ushort)arity;
		Modifiers = modifiers;
	}

	protected override ImmutableArray<SingleDeclaration> GetChildren()
	{
		return ImmutableArray<SingleDeclaration>.CastUp(_children);
	}
}
