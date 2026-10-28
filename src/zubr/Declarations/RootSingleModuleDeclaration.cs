using Zubr.Collections;
using Zubr.Diagnostics;

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
		ConstArray<SingleDeclaration> children,
		ConstArray<Diagnostic> diagnostics
	) : base(name, @ref, @ref.Location, children, diagnostics)
	{
		HasUses = hasUses;
		HasTopUses = hasTopUses;
	}
}
