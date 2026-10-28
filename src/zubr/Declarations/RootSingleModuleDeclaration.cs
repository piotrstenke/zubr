using System.Collections.Immutable;

namespace Zubr.Declarations;

internal sealed class RootSingleModuleDeclaration : SingleModuleDeclaration
{
	public bool HasUses { get; }

	public bool HasTopUses { get; }

	public RootSingleModuleDeclaration(
		string name,
		SyntaxRef @ref,
		bool hasUses,
		bool hasTopUses,
		ImmutableArray<SingleDeclaration> children
	) : base(name, @ref, @ref.Location, children)
	{
		HasUses = hasUses;
		HasTopUses = hasTopUses;
	}
}
